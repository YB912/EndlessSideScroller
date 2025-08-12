
using DesignPatterns.EventBusPattern;
using DesignPatterns.ServiceLocatorPattern;
using DesignPatterns.StatePattern;

namespace Mechanics.GameManagement
{
    public class GameCycleStatemachine : Statemachine
    {
        LoadingEventBus _loadingEventBus;
        UIEventBus _UIEventBus;
        GameCycleEventBus _gameCycleEventbus;

        public GameCycleStatemachine() : base()
        {
            _loadingEventBus = ServiceLocator.instance.Get<LoadingEventBus>();
            _UIEventBus = ServiceLocator.instance.Get<UIEventBus>();
            _gameCycleEventbus = ServiceLocator.instance.Get<GameCycleEventBus>();
            SetupStates();
            _loadingEventBus.Subscribe<MainSceneBootstrappedLoadingEvent>(OnMainSceneBootstrapped);
        }

        public override void Reset()
        {
            TransitionTo(typeof(GameCycleMainMenuState));
        }

        protected override void SetupStates()
        {
            var mainMenuState = new GameCycleMainMenuState(this, _UIEventBus, _gameCycleEventbus);
            var upgradeShopState = new GameCycleUpgradeShopState(this, _UIEventBus, _gameCycleEventbus);
            var playState = new GameCyclePlayState(this, _UIEventBus, _gameCycleEventbus);

            _states.Add(typeof(GameCycleMainMenuState), mainMenuState);
            _states.Add(typeof(GameCycleUpgradeShopState) ,upgradeShopState);
            _states.Add(typeof(GameCyclePlayState), playState);
        }

        void OnMainSceneBootstrapped()
        {
            TransitionTo(typeof(GameCycleMainMenuState));
            Resume();
        }
    }
}
