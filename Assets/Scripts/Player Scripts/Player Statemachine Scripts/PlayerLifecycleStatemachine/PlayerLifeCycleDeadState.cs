
using DesignPatterns.EventBusPattern;
using DesignPatterns.StatePattern;

namespace Player.StateMachines
{
    public class PlayerLifeCycleDeadState : State
    {
        GameplayEventBus _gameplayEventBus;
        GameCycleEventBus _gameCycleEventBus;

        public PlayerLifeCycleDeadState(IStateMachine statemachine, GameplayEventBus gameplayEventBus, GameCycleEventBus gameCycleEventBus) : 
            base(statemachine)
        {
            _gameplayEventBus = gameplayEventBus;
            _gameCycleEventBus = gameCycleEventBus;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _gameplayEventBus.Publish<PlayerDiedGameplayEvent>();
        }

        protected override void SubscribeToTransitionEvents()
        {
            _gameCycleEventBus.Subscribe<EnteredUpgradeShopStateGameCycleEvent>(TransitionToAliveState);
            _gameCycleEventBus.Subscribe<EnteredMainMenuStateGameCycleEvent>(TransitionToAliveState);
            _gameCycleEventBus.Subscribe<ExitedPlayStateGameCycleEvent>(TransitionToAliveState);
        }

        protected override void UnsubscribeFromTransitionEvents()
        {
            _gameCycleEventBus.Unsubscribe<EnteredUpgradeShopStateGameCycleEvent>(TransitionToAliveState);
            _gameCycleEventBus.Unsubscribe<EnteredMainMenuStateGameCycleEvent>(TransitionToAliveState);
            _gameCycleEventBus.Unsubscribe<ExitedPlayStateGameCycleEvent>(TransitionToAliveState);
        }

        void TransitionToAliveState()
        {
            _statemachine.TransitionTo(typeof(PlayerLifeCycleAliveState));
        }
    }
}
