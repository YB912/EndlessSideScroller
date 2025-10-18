
using DesignPatterns.EventBusPattern;
using DesignPatterns.ServiceLocatorPattern;
using DG.Tweening;
using Mechanics.Scoring;

namespace UI.GameOverMenu
{
    public interface IGameOverUIPresenter
    {
        public void RestartButtonClicked();
        public void QuitButtonClicked();
        public static IGameOverUIPresenter Create(IGameOverMenuUIView view)
        {
            return new GameOverMenuUIPresenter(view);
        }
    }

    public class GameOverMenuUIPresenter : IGameOverUIPresenter
    {
        IGameOverMenuUIView _view;
        UIEventBus _UIEventBus;
        GameplayEventBus _gameplayEventBus;
        ScoreManager _scoreManager;

        public GameOverMenuUIPresenter(IGameOverMenuUIView view)
        {
            FetchDependencies();
            _view = view;
            _gameplayEventBus.Subscribe<PlayerDiedGameplayEvent>(OnPlayerDied);
        }

        public void RestartButtonClicked()
        {
            _view.FadePanelOut().OnComplete(() => _UIEventBus.Publish<UpgradeShopStateButtonClickedUIEvent>());
        }

        public void QuitButtonClicked()
        {
            _view.FadePanelOut().OnComplete(() => _UIEventBus.Publish<MainMenuStateButtonClickedUIEvent>());
        }

        void FetchDependencies()
        {
            _UIEventBus = ServiceLocator.instance.Get<UIEventBus>();
            _gameplayEventBus = ServiceLocator.instance.Get<GameplayEventBus>();
            _scoreManager = ServiceLocator.instance.Get<ScoreManager>();
        }

        void OnPlayerDied()
        {
            _view.UpdateTotalScore(_scoreManager.totalScore);
            _view.FadePanelIn();
        }
    }
}
