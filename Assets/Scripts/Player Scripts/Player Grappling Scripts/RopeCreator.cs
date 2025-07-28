using DesignPatterns.EventBusPattern;
using DesignPatterns.ObjectPool;
using DesignPatterns.ServiceLocatorPattern;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Mechanics.Grappling
{
    /// <summary>
    /// Creates rope segments between the character and a wall using raycasting and pooling.
    /// </summary>
    public class RopeCreator : MonoBehaviour
    {
        List<RopeSegmentController> _currentRope = new();
        RopeSegmentController _currentSegment;

        GameObject _ropeSegmentPrefab;

        Transform _ropeSegmentsHolder;
        Transform _handTransform;
        Rigidbody2D _forearmRigidbody;
        Rigidbody2D _abdomenRigidbody;
        int _segmentCountLimit;
        float _targetRopeLength;
        float _segmentLength;
        int _targetSegementsCount;
        int _wallLayerInBitMap;

        ObjectPoolManager _objectPool;
        GrapplingEventBus _grapplingEventBus;

        const string WALL_LAYER_NAME = "Wall";
        float _raycastDistance = 500;

        public void Initialize(GrapplingRopeDependencies ropeDependencies, CommonGrapplingDependencies commonDependencies)
        {
            FetchDependencies(ropeDependencies, commonDependencies);
            _wallLayerInBitMap = Utility.LayerNameToBitMap(WALL_LAYER_NAME);
            _segmentLength = _ropeSegmentPrefab.GetComponent<SpriteRenderer>().bounds.size.y;
        }

        void FetchDependencies(GrapplingRopeDependencies ropeDependencies, CommonGrapplingDependencies commonDependencies)
        {
            _ropeSegmentPrefab = ropeDependencies.ropeSegmentPrefab;
            _forearmRigidbody = ropeDependencies.forearmRigidbody;
            _abdomenRigidbody = ropeDependencies.abdomenRigidbody;
            _segmentCountLimit = ropeDependencies.segmentCountLimit;
            _handTransform = commonDependencies.effectorTransform;
            _ropeSegmentsHolder = new GameObject("RopeSegmentsHolder").transform;
            _grapplingEventBus = ServiceLocator.instance.Get<GrapplingEventBus>();
            _objectPool = ServiceLocator.instance.Get<ObjectPoolManager>();
        }

        /// <summary>
        /// Creates a rope by raycasting toward a wall and placing segments accordingly.
        /// </summary>
        public List<RopeSegmentController> CreateRope()
        {
            _currentRope.Clear();
            CalculateTargetRopeLength();
            CalculateTargetSegmentsCount();

            for (var i = 0; i < _targetSegementsCount; i++)
            {
                CreateRopeSegment();

                if (i != 0)
                {
                    SetupNonFirstSegmentJoints();
                }

                _currentRope.Add(_currentSegment);
            }

            _grapplingEventBus.Publish<GrapplerAttachedToSurfaceEvent>();
            _currentSegment.SetAsAttachmentSegment();
            return _currentRope;
        }

        void CalculateTargetRopeLength()
        {
            _targetRopeLength = Physics2D.Raycast(
                _handTransform.position,
                _forearmRigidbody.transform.right,
                _raycastDistance,
                _wallLayerInBitMap
            ).distance;
        }

        void CalculateTargetSegmentsCount()
        {
            _targetSegementsCount = Mathf.CeilToInt(_targetRopeLength / _segmentLength);
            _targetSegementsCount = Mathf.Min(_targetSegementsCount, _segmentCountLimit);
        }

        void CreateRopeSegment()
        {
            var position = GetSegmentPosition();
            var rotation = GetSegmentRotation();
            var mass = GetSegmentMass();

            var segment = _objectPool
                .TakeFromPool(_ropeSegmentPrefab, position, rotation, _ropeSegmentsHolder)
                .GetComponent<RopeSegmentController>();

            segment.rigidBody.mass = mass;
            var velocityScale = 1f - ((float)(_currentRope.Count + 1) / _targetSegementsCount);
            segment.rigidBody.linearVelocity = _abdomenRigidbody.linearVelocity * velocityScale;
            _currentSegment = segment;
        }

        Vector2 GetSegmentPosition()
        {
            return _currentRope.Any()
                ? _currentRope.Last().hingeJoint.connectedAnchor
                : _handTransform.position;
        }

        Quaternion GetSegmentRotation()
        {
            return transform.rotation * Quaternion.Euler(0, 0, -90);
        }

        float GetSegmentMass()
        {
            return 5; // Currently hardcoded, can be parameterized if needed
        }

        void SetupNonFirstSegmentJoints()
        {
            var previousSegment = _currentRope.Last();
            previousSegment.hingeJoint.connectedBody = _currentSegment.rigidBody;
            previousSegment.distanceJoint.connectedBody = _currentSegment.rigidBody;
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawRay(_handTransform.position, _forearmRigidbody.transform.right * _raycastDistance);
        }
    }
}
