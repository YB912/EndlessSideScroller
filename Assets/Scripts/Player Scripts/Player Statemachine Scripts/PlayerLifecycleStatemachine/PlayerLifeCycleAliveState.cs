
using DesignPatterns.EventBusPattern;
using DesignPatterns.StatePattern;

namespace Player.StateMachines
{
    public class PlayerLifeCycleAliveState : State
    {
        GameplayEventBus _gamePlayEventBus;
        public PlayerLifeCycleAliveState(IStateMachine statemachine, GameplayEventBus gameplayEventBus) : base(statemachine)
        {
            _gamePlayEventBus = gameplayEventBus;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            // Reset the player
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
