
using DesignPatterns.EventBusPattern;
using DesignPatterns.StatePattern;
using Mechanics.CourseGeneration;
using UnityEngine;

namespace Player.StateMachines
{
    public class PlayerSwingingMainMenuState : State
    {
        GrapplingEventBus _grapplingEventBus;
        GameplayEventBus _gameplayEventBus;
        PlayerController _player;
        MainMenuGrapplingOverrideSettings _settings;
        TilemapParameters _tilemapParameters;

        Vector3 _hangingAimPosition;
        float _aimHeight;
        bool _waitingToGrapple;

        Vector3 _launchAimPosition;

        public PlayerSwingingMainMenuState(IStateMachine statemachine, PlayerController player, GrapplingEventBus grapplingEventbus, 
            GameplayEventBus gameplayEventBus, TilemapParameters tilemapParameters) 
            : base(statemachine)
        {
            _player = player;
            _grapplingEventBus = grapplingEventbus;
            _gameplayEventBus = gameplayEventBus;
            _settings = _player.mainMenuGrapplingOverrideSettings;
            _tilemapParameters = tilemapParameters;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            CalculateAimProperties();
            MainMenuAimOverride();
            _gameplayEventBus.Subscribe<CameraTargetReachedTweenPositionGameplayEvent>(LaunchAimOverride);
        }

        public override void Update()
        {
            base.Update();
            if (_waitingToGrapple && _player.bodyParts.abdomen.transform.position.y <= _aimHeight)
            {
                _player.grapplingManager.aimController.AimTowards(_hangingAimPosition);
                _waitingToGrapple = false;
            }
        }

        void MainMenuAimOverride()
        {
            _grapplingEventBus.Subscribe<GrapplerAimedGrapplingEvent>(MainMenuGrapplingOverride);
            _waitingToGrapple = true;
        }

        void MainMenuGrapplingOverride()
        {
            _player.grapplingManager.ropeController.StartGrappling();
            _grapplingEventBus.Unsubscribe<GrapplerAimedGrapplingEvent>(MainMenuGrapplingOverride);
            _grapplingEventBus.Publish<GrapplerFiredGrapplingEvent>();
        }

        void LaunchAimOverride()
        {
            _player.grapplingManager.ropeController.EndGrappling();
            _grapplingEventBus.Subscribe<GrapplerAimedGrapplingEvent>(LaunchGrapplingOverride);
            _player.grapplingManager.aimController.AimTowards(_launchAimPosition);
        }

        void LaunchGrapplingOverride()
        {
            _player.grapplingManager.ropeController.StartGrappling();
            _grapplingEventBus.Publish<GrapplerFiredGrapplingEvent>();
            _player.grapplingManager.ropeController.EndGrapplingWithDelay(_settings.forwardRopeReleaseDelay, 
                () => _gameplayEventBus.Publish<LaunchSequenceCompletedGameplayEvent>());
            _grapplingEventBus.Unsubscribe<GrapplerAimedGrapplingEvent>(LaunchGrapplingOverride);
        }

        protected override void SubscribeToTransitionEvents()
        {
            _gameplayEventBus.Subscribe<LaunchSequenceCompletedGameplayEvent>(TransitionToIdleState);
        }

        protected override void UnsubscribeFromTransitionEvents()
        {
            _gameplayEventBus.Unsubscribe<LaunchSequenceCompletedGameplayEvent>(TransitionToIdleState);
        }

        void TransitionToIdleState()
        {
            _statemachine.TransitionTo(typeof(PlayerSwingingIdleState));
        }

        void CalculateAimProperties()
        {
            _hangingAimPosition = _player.bodyParts.head.transform.position + _settings.hangingRopeAimOffsetFromHead;
            _aimHeight = _tilemapParameters.tilemapHeight * _tilemapParameters.GridCellSize.y * _settings.hangingRopeAimHeightNormalized;
            _launchAimPosition = _player.bodyParts.head.transform.position + _settings.forwardRopeAimOffsetFromHead;
        }
    }
}
