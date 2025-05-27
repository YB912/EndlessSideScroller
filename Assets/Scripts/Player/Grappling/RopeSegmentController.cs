
using UnityEngine;
using DesignPatterns.ServiceLocatorPattern;
using DesignPatterns.EventBusPattern;

namespace Mechanics.Grappling
{
    public class RopeSegmentController : MonoBehaviour
    {
        Rigidbody2D _rigidBody;
        BoxCollider2D _collider;
        HingeJoint2D _joint;
        RopeSegmentSurfaceDetector _surfaceDetector;

        public Rigidbody2D rigidBody => _rigidBody;
        public HingeJoint2D joint => _joint;

        void Awake()
        {
            FetchDependencies();
            _surfaceDetector.hasHitAGrappleableSurfaceObservable.AddListener(OnHitSurfaceChanged);
        }

        void FetchDependencies()
        {
            _rigidBody = GetComponent<Rigidbody2D>();
            _joint = GetComponent<HingeJoint2D>();
            _collider = GetComponent<BoxCollider2D>();
            _surfaceDetector = GetComponentInChildren<RopeSegmentSurfaceDetector>();
        }

        void OnHitSurfaceChanged(bool hasHitAGrappleableSurface)
        {
            if (hasHitAGrappleableSurface)
            {
                _rigidBody.bodyType = RigidbodyType2D.Static;
                _collider.isTrigger = true;
                ServiceLocator.instance.Get<GrapplingEventBus>().Publish<GrapplerAttachedToSurfaceEvent>();
            }
        }
    }
}
