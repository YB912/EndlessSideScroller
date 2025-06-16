
using DesignPatterns.EventBusPattern;
using DesignPatterns.ServiceLocatorPattern;

namespace DesignPatterns.StatePattern
{
    public class PlayerSwingingGrappledState : State
    {
        InputEventBus _inputEventBus;
        public PlayerSwingingGrappledState(IStateMachine statemachine, PlayerController player) : base(statemachine, player)
        {
            _inputEventBus = ServiceLocator.instance.Get<InputEventBus>();
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _player.grapplingManager.ropeController.StartGrappling();
        }

        public override void OnExit()
        {
            base.OnExit();
            _player.grapplingManager.ropeController.EndGrappling();
        }

        protected override void SubscribeToTransitionEvents()
        {
            _inputEventBus.Subscribe<TouchEndedEvent>(TransitionToIdleState);
        }

        protected override void UnsubscribeFromTransitionEvents()
        {
            _inputEventBus.Unsubscribe<TouchEndedEvent>(TransitionToIdleState);
        }

        void TransitionToIdleState()
        {
            _statemachine.TransitionTo(typeof(PlayerSwingingIdleState));
        }
    }
}
