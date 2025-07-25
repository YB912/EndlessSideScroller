
using DG.Tweening;
using UnityEngine;

namespace UI
{
    [CreateAssetMenu(fileName = "SlidingWindowSettings", menuName = "UI/Sliding Window Settings")]
    public class SlidingWindowSettings : ScriptableObject
    {
        [SerializeField] Vector2 _offScreenAnchorOffsetMin;
        [SerializeField] Vector2 _offScreenAnchorOffsetMax;
        [SerializeField] float _slidingDuration;
        [SerializeField] Ease _slidingEase;

        public Vector2 offScreenAnchorOffsetMin => _offScreenAnchorOffsetMin;
        public Vector2 offScreenAnchorOffsetMax => _offScreenAnchorOffsetMax;
        public float slidingDuration => _slidingDuration;
        public Ease slidingEase => _slidingEase;
    }
}
