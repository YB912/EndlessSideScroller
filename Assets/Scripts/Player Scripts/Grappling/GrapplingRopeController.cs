
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Mechanics.Grappling
{
    public class GrapplingRopeController : MonoBehaviour
    {
        List<List<RopeSegmentController>> _ropes = new();
        List<RopeSegmentController> _currentRope;

        RopeCreator _ropeCreator;

        GameObject _foreArm;
        HingeJoint2D _hingeJointToRope;
        DistanceJoint2D _distanceJointToRope;

        public void StartGrappling()
        {
            var newRope = _ropeCreator.CreateRope();
            _currentRope = newRope;
            AttachRopeEndToHand();
            _ropes.Add(newRope);
        }

        public void EndGrappling()
        {
            DetatchRopeEndFromHand();
        }

        internal void Initialize(GrapplingRopeDependencies ropeDependencies, CommonGrapplingDependencies commonDependencies)
        {
            FetchDependencies(ropeDependencies, commonDependencies);
            _ropeCreator.Initialize(ropeDependencies, commonDependencies);
        }

        void FetchDependencies(GrapplingRopeDependencies ropeDependencies, CommonGrapplingDependencies commonDependencies)
        {
            _ropeCreator = GetComponent<RopeCreator>();
            _foreArm = ropeDependencies.forearmRigidbody.gameObject;
            _hingeJointToRope = _foreArm.GetComponents<HingeJoint2D>()[1];
            _distanceJointToRope = _foreArm.GetComponents<DistanceJoint2D>()[1];
        }

        void AttachRopeEndToHand()
        {
            var firstSegmentRigidbody = _currentRope.First().rigidBody;
            _hingeJointToRope.connectedBody = firstSegmentRigidbody;
            _distanceJointToRope.connectedBody = firstSegmentRigidbody;
            EnableHandJointsToRope();
        }

        void DetatchRopeEndFromHand()
        {
            DisableHandJointsToRope();
            _hingeJointToRope.connectedBody = null;
            _distanceJointToRope.connectedBody = null;
        }

        void EnableHandJointsToRope()
        {
            _hingeJointToRope.enabled = true;
            _distanceJointToRope.enabled = true;
        }

        void DisableHandJointsToRope()
        {
            _hingeJointToRope.enabled = false;
            _distanceJointToRope.enabled = false;
        }
    }
}