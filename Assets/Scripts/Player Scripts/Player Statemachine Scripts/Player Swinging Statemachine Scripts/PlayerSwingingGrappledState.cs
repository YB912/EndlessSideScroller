
using DesignPatterns.EventBusPattern;
using DesignPatterns.StatePattern;

namespace Player.StateMachines
{
    /// <summary>
    /// State when the player is attached to a grapple point.
    /// Handles rope behavior and forces during grappling.
    /// </summary>
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
            _player.swingingForceController.ApplyAttachmentForce();
        }

        public override void OnExit()
        {
            base.OnExit();
            _player.grapplingManager.ropeController.EndGrappling();
            _player.swingingForceController.CancelAttachmentForce();
            _player.swingingForceController.ApplyDetatchmentForce();
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
