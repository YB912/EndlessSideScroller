
using DesignPatterns.EventBusPattern;
using DesignPatterns.ServiceLocatorPattern;
using DG.Tweening;

namespace UI.MainMenu
{
    internal class MainMenuUIPresenter
    {
        MainMenuUIView _view;
        UIEventBus _UIEventBus;
        GameCycleEventBus _gameCycleEventBus;

        internal MainMenuUIPresenter(MainMenuUIView view)
        {
            _UIEventBus = ServiceLocator.instance.Get<UIEventBus>();
            _gameCycleEventBus = ServiceLocator.instance.Get<GameCycleEventBus>();
            _gameCycleEventBus.Subscribe<EnteredMainMenuStateGameCycleEvent>(OnEnteredMainMenuState);
            _view = view;
        }

        internal void PlayButtonClicked()
        {
            _view.DisableButtons();
            _view.SlidePanelOut().OnComplete(() => _UIEventBus.Publish<UpgradeShopStateButtonClickedUIEvent>());
        }

        internal void SettingsButtonClicked()
        {
            // _view.DisableButtons();
            // Hide this menu here
        }

        void OnEnteredMainMenuState()
        {
            _view.SlidePanelIn().OnComplete(() => _view.EnableButtons());
        }
    }
}
