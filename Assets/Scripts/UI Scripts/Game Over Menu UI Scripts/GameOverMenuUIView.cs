
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.GameOverMenu
{
    public class GameOverMenuUIView : UIViewWithButtons, IInitializeable
    {
        [SerializeField] TextMeshProUGUI _gameOverTitleText;
        [SerializeField] Button _restartButton;
        [SerializeField] Button _quitButton;

        IGameOverUIPresenter _presenter;

        public override void Initialize()
        {
            _presenter = IGameOverUIPresenter.Create(this);
        }

        public override void EnableButtonsInteractability()
        {
            _restartButton.interactable = true;
            _quitButton.interactable = true;
        }

        public override void DisableButtonsInteractability()
        {
            _restartButton.interactable = false;
            _quitButton.interactable = false;
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
