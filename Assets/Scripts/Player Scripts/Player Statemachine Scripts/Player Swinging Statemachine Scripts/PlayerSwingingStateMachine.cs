
using DesignPatterns.EventBusPattern;
using DesignPatterns.ServiceLocatorPattern;
using DesignPatterns.StatePattern;

namespace Player.StateMachines
{
    /// <summary>
    /// Manages the player's swinging states and transitions.
    /// Initializes states and sets the starting state.
    /// </summary>
    public class PlayerSwingingStateMachine : StateMachine
    {
        PlayerController _player;

        InputEventBus _inputEventBus;
        GrapplingEventBus _grapplingEventBus;
        GameplayEventBus _gameplayEventBus;

        public PlayerSwingingStateMachine(PlayerController player) : base()
        {
            _player = player;
            FetchDependencies();
            SetupStates();
            TransitionTo(typeof(PlayerSwingingMainMenuState));
            //TransitionTo(typeof(PlayerSwingingIdleState));
        }

        protected override void SetupStates()
        {
            var mainMenuState = new PlayerSwingingMainMenuState(this, _player, _grapplingEventBus, _gameplayEventBus);
            var idleState = new PlayerSwingingIdleState(this, _inputEventBus);
            var aimingState = new PlayerSwingingAimingState(this, _inputEventBus, _grapplingEventBus, _player);
            var grappledState = new PlayerSwingingGrappledState(this, _inputEventBus, _player);

            _states.Add(typeof(PlayerSwingingMainMenuState), mainMenuState);
            _states.Add(typeof(PlayerSwingingIdleState), idleState);
            _states.Add(typeof(PlayerSwingingAimingState), aimingState);
            _states.Add(typeof(PlayerSwingingGrappledState), grappledState);
        }

        private void FetchDependencies()
        {
            _inputEventBus = ServiceLocator.instance.Get<InputEventBus>();
            _grapplingEventBus = ServiceLocator.instance.Get<GrapplingEventBus>();
            _gameplayEventBus = ServiceLocator.instance.Get<GameplayEventBus>();
        }
    }
}
