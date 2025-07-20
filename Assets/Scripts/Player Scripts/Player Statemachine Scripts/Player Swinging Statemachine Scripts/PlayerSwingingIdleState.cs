

using DesignPatterns.EventBusPattern;
using DesignPatterns.StatePattern;

namespace Mechanics.Grappling
{
    /// <summary>
    /// State when the player is idle (not grappling or aiming).
    /// Listens for input to transition to aiming state.
    /// </summary>
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
