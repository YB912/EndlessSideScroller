
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MainMenu
{
    internal class MainMenuUIView : UIView, IInitializeable
    {
        [SerializeField] TextMeshProUGUI _mainMenuTitleText;
        [SerializeField] Button _mainMenuPlayButton;
        [SerializeField] Button _mainMenuSettingsButton;

        MainMenuUIPresenter _presenter;

        public void Initialize()
        {
            _presenter = new MainMenuUIPresenter(this);
        }

        public override Tween SlidePanelIn()
        {
            AddButtonListeners();
            return base.SlidePanelIn();
        }

        public override Tween SlidePanelOut()
        {
            RemoveButtonListeners();
            return base.SlidePanelOut();
        }

        internal void EnableButtons()
        {
            _mainMenuPlayButton.interactable = true;
            _mainMenuSettingsButton.interactable = true;
        }

        internal void DisableButtons()
        {
            _mainMenuPlayButton.interactable = false;
            _mainMenuSettingsButton.interactable = false;
        }

        void AddButtonListeners()
        {
            _mainMenuPlayButton.onClick.AddListener(_presenter.PlayButtonClicked);
            _mainMenuSettingsButton.onClick.AddListener(_presenter.SettingsButtonClicked);
        }

        void RemoveButtonListeners()
        {
            _mainMenuPlayButton.onClick.RemoveAllListeners();
            _mainMenuSettingsButton.onClick.RemoveAllListeners();
        }
    }
}
