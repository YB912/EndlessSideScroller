

using DesignPatterns.EventBusPattern;
using DesignPatterns.ServiceLocatorPattern;

namespace DesignPatterns.StatePattern
{
    public class PlayerSwingingIdleState : State
    {
        InputEventBus _inputEventBus;

        public PlayerSwingingIdleState(IStateMachine statemachine, PlayerController player) : base(statemachine, player)
        {
            _inputEventBus = ServiceLocator.instance.Get<InputEventBus>();
        }

        protected override void SubscribeToTransitionEvents()
        {
            _inputEventBus.Subscribe<TouchStartedEvent>(TransitionToAimingState);
        }

        protected override void UnsubscribeFromTransitionEvents()
        {
            _inputEventBus.Unsubscribe<TouchStartedEvent>(TransitionToAimingState);
        }

        void TransitionToAimingState()
        {
            _statemachine.TransitionTo(typeof(PlayerSwingingAimingState));
        }
    }
}
