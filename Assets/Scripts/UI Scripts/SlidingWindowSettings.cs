
using DG.Tweening;
using UnityEngine;

namespace UI
{
    [CreateAssetMenu(fileName = "SlidingWindowSettings", menuName = "UI/Sliding Window Settings")]
    public class SlidingWindowSettings : ScriptableObject
    {
        [SerializeField] float _slidingDuration;
        [SerializeField] protected Vector2 _onScreenNormalizedPosition;
        [SerializeField] protected Vector2 _offScreenNormalizedPosition;
        [SerializeField] Ease _slidingEase;

        public float slidingDuration => _slidingDuration;
        public Vector2 onScreenNormalizedPosition => _onScreenNormalizedPosition;
        public Vector2 offScreenNormalizedPosition => _offScreenNormalizedPosition;
        public Ease slidingEase => _slidingEase;
    }
}
