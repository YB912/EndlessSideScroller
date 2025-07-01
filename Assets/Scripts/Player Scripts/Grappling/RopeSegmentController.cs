
using DG.Tweening;
using UnityEngine;

namespace Mechanics.Grappling
{
    /// <summary>
    /// Controls an individual rope segment's physics, visuals, and attachment logic.
    /// </summary>
    public class RopeSegmentController : MonoBehaviour
    {
        static Color _transparentColor = new Color(1, 1, 1, 0); // Initial transparent color for fade-in effect

        Rigidbody2D _rigidBody;
        HingeJoint2D _hingeJoint;
        DistanceJoint2D _distanceJoint;
        SpriteRenderer _spriteRenderer;

        public Rigidbody2D rigidBody => _rigidBody;
        public new HingeJoint2D hingeJoint => _hingeJoint;
        public DistanceJoint2D distanceJoint => _distanceJoint;

        void Awake()
        {
            FetchDependencies();
        }

        void OnEnable()
        {
            _spriteRenderer.color = _transparentColor;
        }

        void OnDisable()
        {
            _hingeJoint.connectedBody = null;
            _distanceJoint.connectedBody = null;
        }

        void FetchDependencies()
        {
            _rigidBody = GetComponent<Rigidbody2D>();
            _hingeJoint = GetComponent<HingeJoint2D>();
            _distanceJoint = GetComponent<DistanceJoint2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        /// <summary>
        /// Marks this segment as the final one and freezes it in place.
        /// </summary>
        public void SetAsAttachmentSegment()
        {
            _rigidBody.bodyType = RigidbodyType2D.Kinematic;
        }

        /// <summary>
        /// Fades in the segment over a given duration.
        /// </summary>
        public void FadeIn(float fadeDuration)
        {
            _spriteRenderer.DOFade(1f, fadeDuration);
        }
    }
}
