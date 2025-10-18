
using DesignPatterns.EventBusPattern;
using DesignPatterns.ServiceLocatorPattern;
using DG.Tweening;

namespace UI.UpgradeShop
{
    public interface IUpgradeShipMenuUIPresenter
    {
        public void DeployButtonClicked();
        public static IUpgradeShipMenuUIPresenter Create(IFadingUIViewWithButtons view)
        {
            return new UpgradeShopMenuUIPresenter(view);
        }
    }

    public class UpgradeShopMenuUIPresenter : IUpgradeShipMenuUIPresenter
    {
        IFadingUIViewWithButtons _view;
        UIEventBus _UIEventBus;
        GameCycleEventBus _gameCycleEventBus;

        public UpgradeShopMenuUIPresenter(IFadingUIViewWithButtons view)
        {
            _view = view;
            _UIEventBus = ServiceLocator.instance.Get<UIEventBus>(); 
            _gameCycleEventBus = ServiceLocator.instance.Get<GameCycleEventBus>();
            _gameCycleEventBus.Subscribe<EnteredUpgradeShopStateGameCycleEvent>(OnEnteredUpgradeShopState);
        }

        public void DeployButtonClicked()
        {
            _view.FadePanelOut().OnComplete(() => _UIEventBus.Publish<PlayStateButtonClickedUIEvent>());
        }

        void OnEnteredUpgradeShopState()
        {
            _view.FadePanelIn();
        }
    }
}
