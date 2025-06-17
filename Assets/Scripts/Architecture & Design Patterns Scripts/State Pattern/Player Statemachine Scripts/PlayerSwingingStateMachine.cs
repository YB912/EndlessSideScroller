

using DesignPatterns.EventBusPattern;
using DesignPatterns.ServiceLocatorPattern;

namespace DesignPatterns.StatePattern
{
    public class PlayerSwingingStateMachine : StateMachine
    {
        PlayerController _player;

        internal PlayerSwingingStateMachine(PlayerController player) : base()
        {
            _player = player;
            SetupStates();
        }

        protected override void SetupStates()
        {
            var inputEventBus = ServiceLocator.instance.Get<InputEventBus>();
            var grapplingEventBus = ServiceLocator.instance.Get<GrapplingEventBus>();

            var idleState = new PlayerSwingingIdleState(this, inputEventBus);
            var aimingState = new PlayerSwingingAimingState(this, inputEventBus, grapplingEventBus, _player);
            var grappledState = new PlayerSwingingGrappledState(this, inputEventBus, _player);
            _states.Add(typeof(PlayerSwingingIdleState), idleState);
            _states.Add(typeof(PlayerSwingingAimingState), aimingState);
            _states.Add(typeof(PlayerSwingingGrappledState), grappledState);

            TransitionTo(typeof(PlayerSwingingIdleState));
        }

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


