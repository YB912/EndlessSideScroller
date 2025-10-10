
using DG.Tweening;
using UnityEngine;

namespace UI
{
    [CreateAssetMenu(fileName = "FadingWindowSettings", menuName = "UI/Fading Window Settings")]
    public class FadingWindowSettings : ScriptableObject
    {
        [SerializeField] float _visibleStateAlpha;
        [SerializeField] float _fadingDuration;
        [SerializeField] Ease _fadingEase;

        public float visibleStateAlpha => _visibleStateAlpha;
        public float fadingDuration => _fadingDuration;
        public Ease fadingEase => _fadingEase;
    }
}
