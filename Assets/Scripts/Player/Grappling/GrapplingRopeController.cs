
using DesignPatterns.EventBusPattern;
using DesignPatterns.ObjectPool;
using DesignPatterns.ServiceLocatorPattern;
using InputManagement;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.HableCurve;

namespace Mechanics.Grappling
{
    public class GrapplingRopeController : MonoBehaviour
    {
        GameObject _ropeSegmentPrefab;
        Rigidbody2D _forearmRigidbody;
        float _segmentCreationInterval;

        List<RopeSegmentController> _ropeSegments = new();
        Transform _ropeSegmentsHolder;
        Transform _handTransform;

        GrapplingEventBus _grapplingEventBus;
        TouchInputManager _touchInputManager;
        bool _grapplerAttached;

        ObjectPoolManager _objectPool;

        internal void Initialize(GrapplingRopeDependencies ropeDependencies, CommonGrapplingDependencies commonDependencies)
        {
            FetchDependencies(ropeDependencies, commonDependencies);
            _grapplingEventBus.Subscribe<GrapplerAimedEvent>(OnGrapplerAimed);
            _grapplingEventBus.Subscribe<GrapplerAttachedToSurfaceEvent>(OnGrapplerAttachedToSurface);
            _touchInputManager.isTouchDown.AddListener(OnTouchToggled);
        }

        void FetchDependencies(GrapplingRopeDependencies ropeDependencies, CommonGrapplingDependencies commonDependencies)
        {
            _ropeSegmentPrefab = ropeDependencies.ropeSegmentPrefab;
            _forearmRigidbody = ropeDependencies.forearmRigidbody;
            _segmentCreationInterval = ropeDependencies.segmentCreationInterval;
            _handTransform = commonDependencies.effectorTransform;
            _ropeSegmentsHolder = new GameObject("RopeSegmentsHolder").transform;
            _grapplingEventBus = ServiceLocator.instance.Get<GrapplingEventBus>();
            _touchInputManager = ServiceLocator.instance.Get<TouchInputManager>();
            _objectPool = ServiceLocator.instance.Get<ObjectPoolManager>();
        }

        void OnGrapplerAimed()
        {
            StartCoroutine(CreateRope());
        }

        void OnGrapplerAttachedToSurface()
        {
            _grapplerAttached = true;
        }

        void OnTouchToggled(bool isTouchDown)
        {
            if (isTouchDown == false)
            {

            }
        }

        IEnumerator CreateRope()
        {
            while (_grapplerAttached == false)
            {
                var segment = CreateRopeSegment();
                if (IsFirstSegmentCreated())
                {
                    _ropeSegments.Last().joint.connectedBody = segment.rigidBody;
                }
                else
                {
                    SetupFirstSegment(segment);
                }
                _ropeSegments.Add(segment);
                yield return new WaitForSeconds(_segmentCreationInterval);
            }
        }

        RopeSegmentController CreateRopeSegment()
        {
            var position = GetSegmentPosition();
            var rotation = GetSegmentRotation();
            var mass = GetSegmentMass();
            var segment = _objectPool.TakeFromPool(_ropeSegmentPrefab, position, rotation, _ropeSegmentsHolder).
                GetComponent<RopeSegmentController>();
            segment.rigidBody.mass = mass;
            return segment;
        }

        Vector2 GetSegmentPosition()
        {
            if (_ropeSegments.Any())
            {
                return _ropeSegments.Last().joint.connectedAnchor;
            }
            return _handTransform.position;
        }

        Quaternion GetSegmentRotation()
        {
            return transform.rotation * Quaternion.Euler(0, 0, -90);
        }

        float GetSegmentMass()
        {
            if (IsFirstSegmentCreated())
            {
                return _ropeSegments.Last().rigidBody.mass + 0.1f;
            }
            return 2;
        }

        void SetupFirstSegment(RopeSegmentController segment)
        {
            var jointToHand = segment.AddComponent<HingeJoint2D>();
            jointToHand.connectedBody = _forearmRigidbody;
        }

        bool IsFirstSegmentCreated()
        {
            return _ropeSegments.Any();
        }
    }
}