
using DG.Tweening;
using UnityEngine;

namespace UI
{
    public interface IFadingUIView
    {
        public Tween FadePanelIn();
        public Tween FadePanelOut();
    }

    public abstract class FadingUIView : UIView, IFadingUIView
    {
        [SerializeField] FadingWindowSettings _fadingWindowSettings;

        Tween _currentTween;

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
