
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

        protected override void SubscribeToTransitionEvents()
        {
            _gamePlayEventBus.Subscribe<PlayerHitADeathTrigger>(TransitionToDeadState);
        }

        protected override void UnsubscribeFromTransitionEvents()
        {
            _gamePlayEventBus.Unsubscribe<PlayerHitADeathTrigger>(TransitionToDeadState);
        }

        void TransitionToDeadState()
        {
            _statemachine.TransitionTo(typeof(PlayerLifeCycleDeadState));
        }
    }
}
