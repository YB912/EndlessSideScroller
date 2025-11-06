
using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

namespace Mechanics.Grappling
{
    /// <summary>
    /// Handles rope creation, animation, and attachment/detachment to the character's hand.
    /// </summary>
    public class GrapplingRopeController : MonoBehaviour
    {
        Coroutine _currentCoroutine;

        GrapplingAimController _aimController;

        RopeVisualsController _ropeVisualsController;

        Rigidbody2D _foreArmRigidbody;
        SpringJoint2D _springJoint2DAsRope;
        Rigidbody2D _ropeAttachmentEndRigidbody;
        Rigidbody2D _abdomenRigidbody;

        float _pullFactor;
        float _pullFrequency;
        float _dampingRatio;
        float _pullDuration;
        float _maxSpeedMagnitudeForPull;

        Tween _pullTween;

        public void Initialize(
            GrapplingRopeDependencies ropeDependencies,
            RopeAnimationDependencies ropeAnimationDependencies,
            CommonGrapplingDependencies commonDependencies,
            GrapplingAimController aimController)
        {
            _aimController = aimController;
            FetchDependencies(ropeDependencies, commonDependencies);
            SetupRope();
            _ropeVisualsController.Initialize(ropeAnimationDependencies, ropeDependencies.player.bodyParts.backForearm.transform);
        }

        void FetchDependencies(GrapplingRopeDependencies ropeDependencies, CommonGrapplingDependencies commonDependencies)
        {
            _ropeVisualsController = GetComponent<RopeVisualsController>();
            _foreArmRigidbody = ropeDependencies.player.bodyParts.backForearm.GetComponent<Rigidbody2D>();
            _abdomenRigidbody = ropeDependencies.player.bodyParts.abdomen.GetComponent<Rigidbody2D>();

            // Assumes second joint of each type is for rope (index 1)
            _springJoint2DAsRope = _foreArmRigidbody.GetComponent<SpringJoint2D>();
            _ropeAttachmentEndRigidbody = ropeDependencies.ropeAttachmentEndRigidbody;

            _pullFactor = ropeDependencies.pullFactor;
            _pullFrequency = ropeDependencies.pullFrequency;
            _dampingRatio = ropeDependencies.dampingRatio;  
            _pullDuration = ropeDependencies.pullDuration;
            _maxSpeedMagnitudeForPull = ropeDependencies.maxSpeedMagnitudeForPull;
        }

        void SetupRope()
        {
            _springJoint2DAsRope.frequency = _pullFrequency;
            _springJoint2DAsRope.dampingRatio = _dampingRatio;
        }

        public void StartGrappling()
        {
            if (_currentCoroutine != null) { StopCoroutine(_currentCoroutine); }
            AttachRopeEndToHand();
            _ropeVisualsController.AnimateRopeCreation(_ropeAttachmentEndRigidbody.transform.position);
        }

        public void EndGrappling()
        {
            if (_currentCoroutine != null) { StopCoroutine(_currentCoroutine); }
            DetatchRopeEndFromHand();
            _ropeVisualsController.AnimateRopeFade();
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

        void AttachRopeEndToHand()
        {
            AttachRope();
            EnableHandJointsToRope();
        }

        void AttachRope()
        {
            var handPosition = _foreArmRigidbody.transform.TransformPoint(_springJoint2DAsRope.anchor);
            var originalDistance = Vector3.Distance(handPosition, _aimController.currentAimedTilePosition);
            _springJoint2DAsRope.distance = originalDistance;
            _ropeAttachmentEndRigidbody.transform.position = _aimController.currentAimedTilePosition;
            PullRope(originalDistance);
        }

        void DetatchRopeEndFromHand()
        {
            DisableHandJointsToRope();
            _springJoint2DAsRope.connectedBody = null;
        }

        void EnableHandJointsToRope()
        {
            _springJoint2DAsRope.connectedBody = _ropeAttachmentEndRigidbody;
            _springJoint2DAsRope.enabled = true;
        }

        void DisableHandJointsToRope()
        {
            _springJoint2DAsRope.enabled = false;
        }

        void PullRope(float originalDistance)
        {
            var velocity = _abdomenRigidbody.linearVelocity.magnitude;
            var speedFactor = Mathf.Min(velocity / _maxSpeedMagnitudeForPull, 1);
            _pullTween?.Kill();
            _pullTween = DOTween.To(() => _springJoint2DAsRope.distance,
                x => _springJoint2DAsRope.distance = x,
                originalDistance * _pullFactor * speedFactor,
                _pullDuration);
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
            _currentCoroutine = null;
        }
    }
}
