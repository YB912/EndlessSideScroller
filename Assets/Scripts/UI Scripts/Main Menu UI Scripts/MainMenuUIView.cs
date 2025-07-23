
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MainMenu
{
    public class MainMenuUIView : UIViewWithButtons, IInitializeable
    {
        [SerializeField] TextMeshProUGUI _mainMenuTitleText;
        [SerializeField] Button _mainMenuPlayButton;
        [SerializeField] Button _mainMenuSettingsButton;

        IMainMenuUIPresenter _presenter;

        public override void Initialize()
        {
            _presenter = IMainMenuUIPresenter.Create(this);
        }

        public override void EnableButtonsInteractability()
        {
            _mainMenuPlayButton.interactable = true;
            _mainMenuSettingsButton.interactable = true;
        }

        public override void DisableButtonsInteractability()
        {
            _mainMenuPlayButton.interactable = false;
            _mainMenuSettingsButton.interactable = false;
        }

        protected override void AddButtonListeners()
        {
            _mainMenuPlayButton.onClick.AddListener(_presenter.PlayButtonClicked);
            _mainMenuSettingsButton.onClick.AddListener(_presenter.SettingsButtonClicked);
        }

        protected override void RemoveButtonListeners()
        {
            _mainMenuPlayButton.onClick.RemoveAllListeners();
            _mainMenuSettingsButton.onClick.RemoveAllListeners();
        }
    }
}
