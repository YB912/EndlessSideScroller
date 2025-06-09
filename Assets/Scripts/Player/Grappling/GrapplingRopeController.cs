
using DesignPatterns.EventBusPattern;
using DesignPatterns.ObjectPool;
using DesignPatterns.ServiceLocatorPattern;
using InputManagement;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Mechanics.Grappling
{
    public class GrapplingRopeController : MonoBehaviour
    {
        GameObject _ropeSegmentPrefab;
        Rigidbody2D _forearmRigidbody;
        int _segmentCountLimit;

        List<RopeSegmentController> _ropeSegments = new();
        Transform _ropeSegmentsHolder;
        Transform _handTransform;

        ObjectPoolManager _objectPool;
        GrapplingEventBus _grapplingEventBus;
        TouchInputManager _touchInputManager;
        RopeSegmentController _currentSegment;
        int _wallLayerInBitMap;
        float _targetRopeLength;
        float _segmentLength;
        int _targetSegementsCount;

        const string WALL_LAYER_NAME = "Wall";
        float _raycastDistance = 500;

        internal void Initialize(GrapplingRopeDependencies ropeDependencies, CommonGrapplingDependencies commonDependencies)
        {
            FetchDependencies(ropeDependencies, commonDependencies);
            _grapplingEventBus.Subscribe<GrapplerAimedEvent>(OnGrapplerAimed);
            _touchInputManager.isTouchDown.AddListener(OnTouchToggled);
            _wallLayerInBitMap = Utility.LayerNameToBitMap(WALL_LAYER_NAME);
            _segmentLength = _ropeSegmentPrefab.GetComponent<SpriteRenderer>().bounds.size.y;
        }

        void FetchDependencies(GrapplingRopeDependencies ropeDependencies, CommonGrapplingDependencies commonDependencies)
        {
            _ropeSegmentPrefab = ropeDependencies.ropeSegmentPrefab;
            _forearmRigidbody = ropeDependencies.forearmRigidbody;
            _segmentCountLimit = ropeDependencies.segmentCountLimit;
            _handTransform = commonDependencies.effectorTransform;
            _ropeSegmentsHolder = new GameObject("RopeSegmentsHolder").transform;
            _grapplingEventBus = ServiceLocator.instance.Get<GrapplingEventBus>();
            _touchInputManager = ServiceLocator.instance.Get<TouchInputManager>();
            _objectPool = ServiceLocator.instance.Get<ObjectPoolManager>();
        }

        void OnGrapplerAimed()
        {
            CreateRope();
        }

        void OnTouchToggled(bool isTouchDown)
        {
            if (isTouchDown == false)
            {

            }
        }

        void CreateRope()
        {
            CalculateTargetRopeLength();
            CalculateTargetSegmentsCount();
            for (var i = 0; i < _targetSegementsCount; i++)
            {
                CreateRopeSegment();
                if (i == 0)
                {
                    SetupFirstSegmentJoints();
                }
                else
                {
                    SetupNonFirstSegmentJoints();
                }
                _ropeSegments.Add(_currentSegment);
            }
            _grapplingEventBus.Publish<GrapplerAttachedToSurfaceEvent>();
            _currentSegment.SetAsAttachmentSegment();
        }

        void CalculateTargetRopeLength()
        {
            _targetRopeLength = Physics2D.Raycast(_handTransform.position, _forearmRigidbody.transform.right, _raycastDistance, _wallLayerInBitMap).distance;
        }

        void CalculateTargetSegmentsCount()
        {
            _targetSegementsCount = Mathf.CeilToInt(_targetRopeLength / _segmentLength);
            _targetSegementsCount = Mathf.Min(_targetSegementsCount, _segmentCountLimit);
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawRay(_handTransform.position, _forearmRigidbody.transform.right * _raycastDistance);
        }

        void CreateRopeSegment()
        {
            var position = GetSegmentPosition();
            var rotation = GetSegmentRotation();
            var mass = GetSegmentMass();
            var segment = _objectPool.TakeFromPool(_ropeSegmentPrefab, position, rotation, _ropeSegmentsHolder).
                GetComponent<RopeSegmentController>();
            segment.rigidBody.mass = mass;
            _currentSegment = segment;
        }

        Vector2 GetSegmentPosition()
        {
            if (_ropeSegments.Any())
            {
                return _ropeSegments.Last().hingeJoint.connectedAnchor;
            }
            return _handTransform.position;
        }

        Quaternion GetSegmentRotation()
        {
            return transform.rotation * Quaternion.Euler(0, 0, -90);
        }

        float GetSegmentMass()
        {
            return 5; // To be cleaned further
        }

        void SetupFirstSegmentJoints()
        {
            var hingeJointToHand = _currentSegment.AddComponent<HingeJoint2D>();
            hingeJointToHand.connectedBody = _forearmRigidbody;

            var distanceJointToHand = _currentSegment.AddComponent<DistanceJoint2D>();
            distanceJointToHand.connectedBody = _forearmRigidbody;
            distanceJointToHand.distance = 0;
            distanceJointToHand.autoConfigureConnectedAnchor = true;
            distanceJointToHand.autoConfigureDistance = false;
            distanceJointToHand.maxDistanceOnly = true;
        }

        void SetupNonFirstSegmentJoints()
        {
            var previousSegment = _ropeSegments.Last();
            previousSegment.hingeJoint.connectedBody = _currentSegment.rigidBody;
            previousSegment.distanceJoint.connectedBody = _currentSegment.rigidBody;
        }
    }
}