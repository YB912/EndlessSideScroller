
using DesignPatterns.StatePattern;
using DesignPatterns.EventBusPattern;

namespace Mechanics.GameManagement
{
    public class GameCyclePlayState : State
    {
        UIEventBus _UIEventBus;
        GameCycleEventBus _gameCycleEventBus;

        public GameCyclePlayState(IStateMachine statemachine, UIEventBus UIEventBus, GameCycleEventBus gameCycleEventBus) : base(statemachine)
        {
            _UIEventBus = UIEventBus;
            _gameCycleEventBus = gameCycleEventBus;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _gameCycleEventBus.Publish<EnteredPlayStateGameCycleEvent>();
        }

        public override void OnExit()
        {
            base.OnExit();
            _gameCycleEventBus.Publish<ExitedPlayStateGameCycleEvent>();
        }

        protected override void SubscribeToTransitionEvents()
        {
            _UIEventBus.Subscribe<UpgradeShopStateButtonClickedUIEvent>(TransitionToUpgradeShopState);
            _UIEventBus.Subscribe<MainMenuStateButtonClickedUIEvent>(TransitionToMainMenuState);
        }

        protected override void UnsubscribeFromTransitionEvents()
        {
            _UIEventBus.Unsubscribe<UpgradeShopStateButtonClickedUIEvent>(TransitionToUpgradeShopState);
            _UIEventBus.Unsubscribe<MainMenuStateButtonClickedUIEvent>(TransitionToMainMenuState);
        }

        void TransitionToUpgradeShopState()
        {
            _statemachine.TransitionTo(typeof(GameCycleUpgradeShopState));
        }

        void TransitionToMainMenuState()
        {
            _statemachine.TransitionTo(typeof(GameCycleMainMenuState));
        }
    }
}
