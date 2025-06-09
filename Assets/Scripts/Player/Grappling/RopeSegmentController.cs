
using UnityEngine;
using DesignPatterns.ServiceLocatorPattern;
using DesignPatterns.EventBusPattern;

namespace Mechanics.Grappling
{
    public class RopeSegmentController : MonoBehaviour
    {
        public static int SEGMENT_COUNT;

        Rigidbody2D _rigidBody;
        BoxCollider2D _collider;
        HingeJoint2D _hingeJoint;
        DistanceJoint2D _distanceJoint;

        public Rigidbody2D rigidBody => _rigidBody;
        public new HingeJoint2D hingeJoint => _hingeJoint;
        public DistanceJoint2D distanceJoint => _distanceJoint;

        void Awake()
        {
            FetchDependencies();
        }

        private void OnEnable()
        {
            SEGMENT_COUNT++;
        }

        private void OnDisable()
        {
            SEGMENT_COUNT--;
        }

        void FetchDependencies()
        {
            _rigidBody = GetComponent<Rigidbody2D>();
            _hingeJoint = GetComponent<HingeJoint2D>();
            _distanceJoint = GetComponent<DistanceJoint2D>();
            _collider = GetComponent<BoxCollider2D>();
        }

        public void SetAsAttachmentSegment()
        {
            _rigidBody.bodyType = RigidbodyType2D.Static;
            _collider.enabled = false;
        }
    }
}
