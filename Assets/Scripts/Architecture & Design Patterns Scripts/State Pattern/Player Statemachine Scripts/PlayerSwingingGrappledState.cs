
using DesignPatterns.EventBusPattern;

namespace DesignPatterns.StatePattern
{
    public class PlayerSwingingGrappledState : State
    {
        InputEventBus _inputEventBus;
        PlayerController _player;

        public PlayerSwingingGrappledState(IStateMachine statemachine, InputEventBus inputEventBus, PlayerController player) : base(statemachine)
        {
            _inputEventBus = inputEventBus;
            _player = player;
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
