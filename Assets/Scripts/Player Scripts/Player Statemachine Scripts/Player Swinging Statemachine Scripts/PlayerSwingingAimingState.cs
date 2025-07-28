
using DesignPatterns.EventBusPattern;
using DesignPatterns.StatePattern;

namespace Player.StateMachines
{
    public class PlayerSwingingAimingState : State
    {
        InputEventBus _inputEventBus;
        GrapplingEventBus _grapplingEventBus;
        PlayerController _player;

        public PlayerSwingingAimingState(IStateMachine statemachine, InputEventBus inputEventBus, GrapplingEventBus grapplingEventBus, PlayerController player)
            : base(statemachine)
        {
            _inputEventBus = inputEventBus;
            _grapplingEventBus = grapplingEventBus;
            _player = player;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _player.grapplingManager.aimController.StartAiming();
        }

        protected override void SubscribeToTransitionEvents()
        {
            _inputEventBus.Subscribe<TouchEndedEvent>(TransitionToIdleState);
            _grapplingEventBus.Subscribe<GrapplerAimedEvent>(TransitionToGrappledState);
        }

        protected override void UnsubscribeFromTransitionEvents()
        {
            _inputEventBus.Unsubscribe<TouchEndedEvent>(TransitionToIdleState);
            _grapplingEventBus.Unsubscribe<GrapplerAimedEvent>(TransitionToGrappledState);
        }

        void TransitionToIdleState()
        {
            _statemachine.TransitionTo(typeof(PlayerSwingingIdleState));
        }

        void TransitionToGrappledState()
        {
            _statemachine.TransitionTo(typeof(PlayerSwingingGrappledState));
        }
    }
}
