
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Mechanics.Grappling
{
    /// <summary>
    /// Handles rope creation, animation, and attachment/detachment to the character's hand.
    /// </summary>
    public class GrapplingRopeController : MonoBehaviour
    {
        List<RopeSegmentController> _currentRope;
        Coroutine _currentCoroutine;

        RopeCreator _ropeCreator;
        RopeCreationAnimationController _animationController;

        GameObject _foreArm;
        HingeJoint2D _hingeJointToRope;
        DistanceJoint2D _distanceJointToRope;

        public void Initialize(
            GrapplingRopeDependencies ropeDependencies,
            RopeAnimationDependencies ropeAnimationDependencies,
            CommonGrapplingDependencies commonDependencies)
        {
            FetchDependencies(ropeDependencies, commonDependencies);
            _ropeCreator.Initialize(ropeDependencies, commonDependencies);
            _animationController.Initialize(ropeAnimationDependencies);
        }

        public void StartGrappling()
        {
            if (_currentCoroutine != null) { StopCoroutine(_currentCoroutine); }
            var newRope = _ropeCreator.CreateRope();
            _currentRope = newRope;
            AttachRopeEndToHand();
            _animationController.AnimateRopeCreation(_currentRope);
        }

        public void EndGrappling()
        {
            if (_currentCoroutine != null) { StopCoroutine(_currentCoroutine); }
            DetatchRopeEndFromHand();
        }

        public void StartGrapplingWithDelay(float delay)
        {
            if (_currentCoroutine != null) { StopCoroutine(_currentCoroutine); }
            StartCoroutine(GrappleWithDelayCoroutine(delay));
        }

        public void StartGrapplingWithDelay(float delay, Action onComplete)
        {
            StartGrapplingWithDelay(delay);
            onComplete();
        }

        public void EndGrapplingWithDelay(float delay)
        {
            if (_currentCoroutine != null) { StopCoroutine(_currentCoroutine); }
            _currentCoroutine = StartCoroutine(EndGrapplingWithDelayCoroutine(delay));
        }

        public void EndGrapplingWithDelay(float delay, Action onComplete)
        {
            if (_currentCoroutine != null) { StopCoroutine(_currentCoroutine); }
            _currentCoroutine = StartCoroutine(EndGrapplingWithDelayCoroutine(delay, onComplete));
        }

        void FetchDependencies(GrapplingRopeDependencies ropeDependencies, CommonGrapplingDependencies commonDependencies)
        {
            _ropeCreator = GetComponent<RopeCreator>();
            _animationController = GetComponent<RopeCreationAnimationController>();
            _foreArm = ropeDependencies.forearmRigidbody.gameObject;

            // Assumes second joint of each type is for rope (index 1)
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

        IEnumerator GrappleWithDelayCoroutine(float delay)
        {
            yield return new WaitForSeconds(delay);
            StartGrappling();
        }

        IEnumerator EndGrapplingWithDelayCoroutine(float delay)
        {
            yield return new WaitForSeconds(delay);
            EndGrappling();
        }

        IEnumerator EndGrapplingWithDelayCoroutine(float delay, Action onComplete)
        {
            yield return new WaitForSeconds(delay);
            EndGrappling();
            onComplete?.Invoke();
        }
    }
}
