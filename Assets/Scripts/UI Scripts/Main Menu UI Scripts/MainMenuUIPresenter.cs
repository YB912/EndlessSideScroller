
using DesignPatterns.EventBusPattern;
using DesignPatterns.ServiceLocatorPattern;
using DG.Tweening;

namespace UI.MainMenu
{
    public interface IMainMenuUIPresenter
    {
        public void PlayButtonClicked();
        public static IMainMenuUIPresenter Create(IFadingUIViewWithButtons view)
        {
            return new MainMenuUIPresenter(view);
        }
    }

    public class MainMenuUIPresenter : IMainMenuUIPresenter
    {
        IFadingUIViewWithButtons _view;
        UIEventBus _UIEventBus;
        GameCycleEventBus _gameCycleEventBus;

        public MainMenuUIPresenter(IFadingUIViewWithButtons view)
        {
            _UIEventBus = ServiceLocator.instance.Get<UIEventBus>();
            _gameCycleEventBus = ServiceLocator.instance.Get<GameCycleEventBus>();
            _gameCycleEventBus.Subscribe<EnteredMainMenuStateGameCycleEvent>(OnEnteredMainMenuState);
            _view = view;
        }

        public void PlayButtonClicked()
        {
            _view.FadePanelOut().OnComplete(() => _UIEventBus.Publish<PlayStateButtonClickedUIEvent>());
        }

        void OnEnteredMainMenuState()
        {
            _view.FadePanelIn();
        }
    }
}
