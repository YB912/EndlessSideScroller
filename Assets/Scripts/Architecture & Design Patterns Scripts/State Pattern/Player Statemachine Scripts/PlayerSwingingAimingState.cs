
using DesignPatterns.EventBusPattern;
using DesignPatterns.ServiceLocatorPattern;

namespace DesignPatterns.StatePattern
{
    public class PlayerSwingingAimingState : State
    {
        InputEventBus _inputEventBus;
        GrapplingEventBus _grarapplingEventBus;

        public PlayerSwingingAimingState(IStateMachine statemachine, PlayerController player) : base(statemachine, player)
        {
            _inputEventBus = ServiceLocator.instance.Get<InputEventBus>();
            _grarapplingEventBus = ServiceLocator.instance.Get<GrapplingEventBus>();
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _player.grapplingManager.aimController.StartAiming();
        }

        public override void OnExit()
        {
            base.OnExit();
            _player.grapplingManager.aimController.EndAiming();
        }

        protected override void SubscribeToTransitionEvents()
        {
            _inputEventBus.Subscribe<TouchEndedEvent>(TransitionToIdleState);
            _grarapplingEventBus.Subscribe<GrapplerAimedEvent>(TransitionToGrappledState);
        }

        protected override void UnsubscribeFromTransitionEvents()
        {
            _inputEventBus.Unsubscribe<TouchEndedEvent>(TransitionToIdleState);
            _grarapplingEventBus.Unsubscribe<GrapplerAimedEvent>(TransitionToGrappledState);
        }

        private void TransitionToIdleState()
        {
            _statemachine.TransitionTo(typeof(PlayerSwingingIdleState));
        }

        void TransitionToGrappledState()
        {
            _statemachine.TransitionTo(typeof(PlayerSwingingGrappledState));
        }
    }
}
