
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MainMenu
{
    public class MainMenuUIView : FadingUIViewWithButtons, IInitializeable
    {
        [SerializeField] TextMeshProUGUI _mainMenuTitleText;
        [SerializeField] Button _mainMenuPlayButton;
        [SerializeField] Button _mainMenuSettingsButton;

        IMainMenuUIPresenter _presenter;

        public override void Initialize()
        {
            _presenter = IMainMenuUIPresenter.Create(this);
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
