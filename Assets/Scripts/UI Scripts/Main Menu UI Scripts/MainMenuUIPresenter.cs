
using DesignPatterns.EventBusPattern;
using DesignPatterns.ServiceLocatorPattern;
using DG.Tweening;

namespace UI.MainMenu
{
    public interface IMainMenuUIPresenter
    {
        public void PlayButtonClicked();
        public void SettingsButtonClicked();
        public static IMainMenuUIPresenter Create(IUIViewWithButtons view)
        {
            return new MainMenuUIPresenter(view);
        }
    }

    public class MainMenuUIPresenter : IMainMenuUIPresenter
    {
        IUIViewWithButtons _view;
        UIEventBus _UIEventBus;
        GameCycleEventBus _gameCycleEventBus;

        public MainMenuUIPresenter(IUIViewWithButtons view)
        {
            _UIEventBus = ServiceLocator.instance.Get<UIEventBus>();
            _gameCycleEventBus = ServiceLocator.instance.Get<GameCycleEventBus>();
            _gameCycleEventBus.Subscribe<EnteredMainMenuStateGameCycleEvent>(OnEnteredMainMenuState);
            _view = view;
        }

        public void PlayButtonClicked()
        {
            _view.DisableButtonsInteractability();
            _view.SlidePanelOut().OnComplete(() => _UIEventBus.Publish<UpgradeShopStateButtonClickedUIEvent>());
        }

        public void SettingsButtonClicked()
        {
            // _view.DisableButtons();
            // Hide this menu here
        }

        void OnEnteredMainMenuState()
        {
            _view.SlidePanelIn().OnComplete(() => _view.EnableButtonsInteractability());
        }
    }
}
