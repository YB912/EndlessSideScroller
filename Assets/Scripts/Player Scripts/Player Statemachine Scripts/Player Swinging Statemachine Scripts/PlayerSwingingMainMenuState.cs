
using DesignPatterns.EventBusPattern;
using DesignPatterns.StatePattern;
using UnityEngine;

namespace Player.StateMachines
{
    public class PlayerSwingingMainMenuState : State
    {
        GrapplingEventBus _grapplingEventBus;
        GameplayEventBus _gameplayEventBus;
        PlayerController _player;

        public PlayerSwingingMainMenuState(IStateMachine statemachine, PlayerController player, GrapplingEventBus grapplingEventbus, GameplayEventBus gameplayEventBus) 
            : base(statemachine)
        {
            _player = player;
            _grapplingEventBus = grapplingEventbus;
            _gameplayEventBus = gameplayEventBus;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            MainMenuAimOverride();
            _gameplayEventBus.Subscribe<CameraTargetReachedTweenPositionEvent>(LaunchAimOverride);
        }

        void MainMenuAimOverride()
        {
            _grapplingEventBus.Subscribe<GrapplerAimedEvent>(MainMenuGrapplingOverride);
            var aimPosition = _player.bodyParts.head.transform.position + _player.mainMenuGrapplingOverrideSettings.hangingRopeAimOffsetFromHead;
            var aimDelay = _player.mainMenuGrapplingOverrideSettings.hangingRopeAimDelay;
            _player.grapplingManager.aimController.AimTowardsWithDelay(aimPosition, aimDelay);
        }

        void MainMenuGrapplingOverride()
        {
            _player.grapplingManager.ropeController.StartGrappling();
            _grapplingEventBus.Unsubscribe<GrapplerAimedEvent>(MainMenuGrapplingOverride);
        }

        void LaunchAimOverride()
        {
            _player.grapplingManager.ropeController.EndGrappling();
            _grapplingEventBus.Subscribe<GrapplerAimedEvent>(LaunchGrapplingOverride);
            var aimPosition = _player.bodyParts.head.transform.position + _player.mainMenuGrapplingOverrideSettings.forwardRopeAimOffsetFromHead;
            _player.grapplingManager.aimController.AimTowards(aimPosition);
        }

        void LaunchGrapplingOverride()
        {
            _player.grapplingManager.ropeController.StartGrappling();
            var releaseDelay = _player.mainMenuGrapplingOverrideSettings.forwardRopeReleaseDelay;
            _player.grapplingManager.ropeController.EndGrapplingAfterDelay(releaseDelay, 
                () => _gameplayEventBus.Publish<LaunchSequenceCompletedEvent>());
            _grapplingEventBus.Unsubscribe<GrapplerAimedEvent>(MainMenuGrapplingOverride);
        }

        protected override void SubscribeToTransitionEvents()
        {
            _gameplayEventBus.Subscribe<LaunchSequenceCompletedEvent>(TransitionToIdleState);
        }

        protected override void UnsubscribeFromTransitionEvents()
        {
            _gameplayEventBus.Unsubscribe<LaunchSequenceCompletedEvent>(TransitionToIdleState);
        }

        void TransitionToIdleState()
        {
            _statemachine.TransitionTo(typeof(PlayerSwingingIdleState));
        }
    }
}
