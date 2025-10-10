
using UnityEngine;
using DG.Tweening;
using TMPro;

namespace UI
{
    public interface IUIView
    {
        public void Initialize();
        public Tween FadePanelIn();
        public Tween FadePanelOut();
    }

    public abstract class UIView : MonoBehaviour, IUIView
    {
        [SerializeField] protected GameObject _rootPanel;
        [SerializeField] FadingWindowSettings _fadingWindowSettings;

        CanvasGroup _canvasGroup;

        Tween _currentTween;

        protected virtual void Awake()
        {
            FetchDependencies();
            Hide();
        }

        public abstract void Initialize();

        protected virtual void FetchDependencies()
        {
            _canvasGroup = _rootPanel.GetComponent<CanvasGroup>();
        }

        public virtual Tween FadePanelIn()
        {
            ActivateRootPanel();
            return FadePanel(_fadingWindowSettings.visibleStateAlpha);
        }

        public virtual Tween FadePanelOut()
        {
            return FadePanel(0f)
                .OnComplete(() => 
                {
                    DeactivateRootPanel();
                });
        }

        protected void ActivateRootPanel()
        {
            _rootPanel.SetActive(true);
        }

        protected void DeactivateRootPanel()
        {
            _rootPanel.SetActive(false);
        }

        void Hide()
        {
            _canvasGroup.alpha = 0f;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;

        }

        Tween FadePanel(float targetAlpha)
        {
            _currentTween?.Kill();

            _currentTween = _canvasGroup
                .DOFade(targetAlpha, _fadingWindowSettings.fadingDuration)
                .SetEase(_fadingWindowSettings.fadingEase)
                .OnStart(() =>
                {
                    _canvasGroup.interactable = targetAlpha > 0f;
                    _canvasGroup.blocksRaycasts = targetAlpha > 0f;
                });

            return _currentTween;
        }
    }
}
