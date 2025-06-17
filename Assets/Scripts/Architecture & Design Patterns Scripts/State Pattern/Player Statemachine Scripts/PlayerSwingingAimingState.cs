
using DesignPatterns.EventBusPattern;
using DesignPatterns.ServiceLocatorPattern;

namespace DesignPatterns.StatePattern
{
    public class PlayerSwingingAimingState : State
    {
        InputEventBus _inputEventBus;
        GrapplingEventBus _grarapplingEventBus;
        PlayerController _player;

        public PlayerSwingingAimingState(IStateMachine statemachine, InputEventBus inputEventBus, GrapplingEventBus grapplingEventBus, PlayerController player) : base(statemachine)
        {
            _inputEventBus = inputEventBus;
            _grarapplingEventBus = grapplingEventBus;
            _player = player;
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
