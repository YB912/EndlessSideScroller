
using DG.Tweening;
using UnityEngine;

namespace UI.GameplayInputAndHUD
{
    public interface IBlinkingFadingUIViewWithButtons : IFadingUIViewWithButtons
    {
        public void Blink();
    }

    public abstract class BlinkingFadingUIViewWithButtons : FadingUIViewWithButtons, IBlinkingFadingUIViewWithButtons
    {
        [SerializeField] BlinkingUISettings _blinkingUISettings;
        [SerializeField] CanvasGroup[] _blinkingElements;

        public override void Initialize()
        {
            foreach (var element in _blinkingElements)
            {
                element.alpha = 1 / 255;
            }
        }

        public void Blink()
        {
            foreach (var element in _blinkingElements)
            {
                var sequence = DOTween.Sequence();
                for (var blink = 0; blink < _blinkingUISettings.numberOfBlinks; blink++)
                {
                    var tweenIn = element.DOFade(_blinkingUISettings.maxAlpha, _blinkingUISettings.eachBlinkDuration)
                        .SetEase(_blinkingUISettings.blinkingEase);
                    var tweenOut = element.DOFade(1/255, _blinkingUISettings.eachBlinkDuration)
                        .SetEase(_blinkingUISettings.blinkingEase);
                    sequence.Append(tweenIn);
                    sequence.AppendInterval(_blinkingUISettings.eachBlinkDuration);
                    sequence.Append(tweenOut);
                }
                sequence.Play();
            }
        }
    }
}
