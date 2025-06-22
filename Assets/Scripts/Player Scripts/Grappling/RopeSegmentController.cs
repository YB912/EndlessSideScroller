
using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace Mechanics.Grappling
{
    public class RopeSegmentController : MonoBehaviour
    {
        public static int SEGMENT_COUNT;

        static Color _transparentColor = new Color(1, 1, 1, 0);

        Rigidbody2D _rigidBody;
        BoxCollider2D _collider;
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
            SEGMENT_COUNT++;
        }

        void OnDisable()
        {
            _hingeJoint.connectedBody = null;
            _distanceJoint.connectedBody = null;
            SEGMENT_COUNT--;
        }

        void FetchDependencies()
        {
            _rigidBody = GetComponent<Rigidbody2D>();
            _hingeJoint = GetComponent<HingeJoint2D>();
            _distanceJoint = GetComponent<DistanceJoint2D>();
            _collider = GetComponent<BoxCollider2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void SetAsAttachmentSegment()
        {
            _rigidBody.bodyType = RigidbodyType2D.Kinematic;
        }

        public void FadeIn(float fadeDuration)
        {
            _spriteRenderer.DOFade(1f, fadeDuration);
        }
    }
}
