
using DG.Tweening;
using UnityEngine;

namespace UI
{
    [CreateAssetMenu(fileName = "BlinkingUISettings", menuName = "UI/Blinking UI Settings")]
    public class BlinkingUISettings : ScriptableObject
    {
        [SerializeField] float _eachBlinkDuration;
        [SerializeField] int _numberOfBlinks;
        [SerializeField] float _maxAlpha;
        [SerializeField] Ease _blinkingEase;

        public float eachBlinkDuration => _eachBlinkDuration;
        public int numberOfBlinks => _numberOfBlinks;
        public float maxAlpha => _maxAlpha;
        public Ease blinkingEase => _blinkingEase;
    }
}
