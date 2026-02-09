
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.GameOverMenu
{
    public interface IGameOverMenuUIView : IFadingUIViewWithButtons
    {
        public void UpdateTotalScore(int totalScore);
    }

    public class GameOverMenuUIView : FadingUIViewWithButtons, IInitializeable, IGameOverMenuUIView
    {
        [SerializeField] TextMeshProUGUI _gameOverTitleText;
        [SerializeField] TextMeshProUGUI _scoreText;
        [SerializeField] Button _restartButton;
        [SerializeField] Button _quitButton;

        IGameOverUIPresenter _presenter;

        public override void Initialize()
        {
            _presenter = IGameOverUIPresenter.Create(this);
        }

        public void UpdateTotalScore(int totalScore)
        {
            _scoreText.text = "Distance: " + totalScore.ToString();
        }

        protected override void AddButtonListeners()
        {
            _restartButton.onClick.AddListener(_presenter.RestartButtonClicked);
            _quitButton.onClick.AddListener(_presenter.QuitButtonClicked);
        }

        protected override void RemoveButtonListeners()
        {
            _restartButton.onClick.RemoveAllListeners();
            _quitButton.onClick.RemoveAllListeners();
        }
    }
}
