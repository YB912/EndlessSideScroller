

using DesignPatterns.EventBusPattern;

namespace DesignPatterns.StatePattern
{
    public class PlayerSwingingIdleState : State
    {
        InputEventBus _inputEventBus;

        public PlayerSwingingIdleState(IStateMachine statemachine, InputEventBus inputEventBus) 
            : base(statemachine)
        {
            _inputEventBus = inputEventBus;
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
