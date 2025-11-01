
using DesignPatterns.EventBusPattern;
using DesignPatterns.StatePattern;

namespace Player.StateMachines
{
    public class PlayerLifeCycleAliveState : State
    {
        GameplayEventBus _gamePlayEventBus;

        PlayerStopDeathController _stopDeathController;

        public PlayerLifeCycleAliveState(IStateMachine statemachine, GameplayEventBus gameplayEventBus) : base(statemachine)
        {
            _gamePlayEventBus = gameplayEventBus;
            _stopDeathController = new PlayerStopDeathController(this);
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _stopDeathController.Activate();
        }

        public override void Update()
        {
            _stopDeathController?.Update();
        }

        public void PlayerStopped()
        {
            TransitionToDeadState();
        } 

        protected override void SubscribeToTransitionEvents()
        {
            _gamePlayEventBus.Subscribe<PlayerHitADeathTriggerGameplayEvent>(TransitionToDeadState);
        }

        protected override void UnsubscribeFromTransitionEvents()
        {
            _gamePlayEventBus.Unsubscribe<PlayerHitADeathTriggerGameplayEvent>(TransitionToDeadState);
        }

        void TransitionToDeadState()
        {
            _statemachine.TransitionTo(typeof(PlayerLifeCycleDeadState));
        }

    }
}
