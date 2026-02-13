
using DesignPatterns.EventBusPattern;
using DesignPatterns.ServiceLocatorPattern;
using DesignPatterns.StatePattern;
using Mechanics.CourseGeneration;

namespace Player.StateMachines
{
    /// <summary>
    /// Manages the player's swinging states and transitions.
    /// Initializes states and sets the starting state.
    /// </summary>
    public class PlayerSwingingStatemachine : Statemachine
    {
        PlayerController _player;

        InputEventBus _inputEventBus;
        GrapplingEventBus _grapplingEventBus;
        GameplayEventBus _gameplayEventBus;
        TilemapParameters _tilemapParameters; 

        public PlayerSwingingStatemachine(PlayerController player) : base()
        {
            _player = player;
            FetchDependencies();
            SetupStates();
            TransitionTo(typeof(PlayerSwingingMainMenuState));
            _gameplayEventBus.Subscribe<PlayerDiedGameplayEvent>(OnPlayerDied);
        }

        protected override void SetupStates()
        {
            var mainMenuState = new PlayerSwingingMainMenuState(this, _player, _grapplingEventBus, _gameplayEventBus, _tilemapParameters);
            var idleState = new PlayerSwingingIdleState(this, _inputEventBus);
            var aimingState = new PlayerSwingingAimingState(this, _inputEventBus, _grapplingEventBus, _player);
            var grappledState = new PlayerSwingingGrappledState(this, _inputEventBus, _grapplingEventBus, _player);

            _states.TryAdd(typeof(PlayerSwingingMainMenuState), mainMenuState);
            _states.TryAdd(typeof(PlayerSwingingIdleState), idleState);
            _states.TryAdd(typeof(PlayerSwingingAimingState), aimingState);
            _states.TryAdd(typeof(PlayerSwingingGrappledState), grappledState);
        }

        void OnPlayerDied()
        {
            Pause();
            TransitionTo(typeof(PlayerSwingingMainMenuState));
        }

        void FetchDependencies()
        {
            _inputEventBus = ServiceLocator.instance.Get<InputEventBus>();
            _grapplingEventBus = ServiceLocator.instance.Get<GrapplingEventBus>();
            _gameplayEventBus = ServiceLocator.instance.Get<GameplayEventBus>();
            _tilemapParameters = ServiceLocator.instance.Get<CourseGenerationManager>().parameters.tilemapParameters;
        }
    }
}
