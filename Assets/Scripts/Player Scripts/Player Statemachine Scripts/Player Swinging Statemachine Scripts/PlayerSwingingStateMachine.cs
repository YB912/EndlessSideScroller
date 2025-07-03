

using DesignPatterns.EventBusPattern;
using DesignPatterns.ServiceLocatorPattern;

namespace DesignPatterns.StatePattern
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

        internal PlayerSwingingStateMachine(PlayerController player) : base()
        {
            _player = player;
            FetchDependencies();
            SetupStates();
            TransitionTo(typeof(PlayerSwingingIdleState));
        }

        protected override void SetupStates()
        {
            var idleState = new PlayerSwingingIdleState(this, _inputEventBus);
            var aimingState = new PlayerSwingingAimingState(this, _inputEventBus, _grapplingEventBus, _player);
            var grappledState = new PlayerSwingingGrappledState(this, _inputEventBus, _player);

            _states.Add(typeof(PlayerSwingingIdleState), idleState);
            _states.Add(typeof(PlayerSwingingAimingState), aimingState);
            _states.Add(typeof(PlayerSwingingGrappledState), grappledState);
        }

        private void FetchDependencies()
        {
            _inputEventBus = ServiceLocator.instance.Get<InputEventBus>();
            _grapplingEventBus = ServiceLocator.instance.Get<GrapplingEventBus>();
        }

        /// <summary>
        /// Encapsulates dependencies used by swinging states.
        /// </summary>
        public class SwingingStateDependencies
        {
            InputEventBus _inputEventBus;
            GrapplingEventBus _grapplingEventBus;
            PlayerController _player;

            public InputEventBus inputEventBus => _inputEventBus;
            public GrapplingEventBus grapplingEventBus => _grapplingEventBus;
            public PlayerController player => _player;

            internal SwingingStateDependencies(PlayerController player)
            {
                _inputEventBus = ServiceLocator.instance.Get<InputEventBus>();
                _grapplingEventBus = ServiceLocator.instance.Get<GrapplingEventBus>();
                _player = player;
            }
        }
    }
}
