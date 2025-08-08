
using DesignPatterns.EventBusPattern;
using DesignPatterns.ServiceLocatorPattern;
using DG.Tweening;

namespace UI.GameOverMenu
{
    public interface IGameOverUIPresenter
    {
        public void RestartButtonClicked();
        public void QuitButtonClicked();
        public static IGameOverUIPresenter Create(IUIViewWithButtons view)
        {
            return new GameOverMenuUIPresenter(view);
        }
    }

    public class GameOverMenuUIPresenter : IGameOverUIPresenter
    {
        IUIViewWithButtons _view;
        UIEventBus _UIEventBus;
        GameplayEventBus _gameplayEventBus;

        public GameOverMenuUIPresenter(IUIViewWithButtons view)
        {
            _UIEventBus = ServiceLocator.instance.Get<UIEventBus>();
            _gameplayEventBus = ServiceLocator.instance.Get<GameplayEventBus>();
            _view = view;
            _gameplayEventBus.Subscribe<PlayerDiedGameplayEvent>(OnPlayerDied);
        }

        public void RestartButtonClicked()
        {
            _view.DisableButtonsInteractability();
            _view.SlidePanelOut().OnComplete(() => _UIEventBus.Publish<UpgradeShopStateButtonClickedUIEvent>());
        }

        public void QuitButtonClicked()
        {
            _view.DisableButtonsInteractability();
            _view.SlidePanelOut().OnComplete(() => _UIEventBus.Publish<MainMenuStateButtonClickedUIEvent>());
        }

        void OnPlayerDied()
        {
            _view.SlidePanelIn();
        }
    }
}
