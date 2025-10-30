
using DesignPatterns.ServiceLocatorPattern;
using Mechanics.CourseGeneration;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Mechanics.Grappling
{
    /// <summary>
    /// Handles rope creation, animation, and attachment/detachment to the character's hand.
    /// </summary>
    public class GrapplingRopeController : MonoBehaviour
    {
        Coroutine _currentCoroutine;

        RopeVisualsController _ropeVisualsController;

        Rigidbody2D _foreArmRigidbody;
        SpringJoint2D _springJoint2DAsRope;
        Rigidbody2D _ropeAttachmentEndRigidbody;

        int _wallLayerInBitMap;

        const string WALL_LAYER_NAME = "Wall";
        float _raycastDistance = 500;

        public void Initialize(
            GrapplingRopeDependencies ropeDependencies,
            RopeAnimationDependencies ropeAnimationDependencies,
            CommonGrapplingDependencies commonDependencies)
        {
            _wallLayerInBitMap = Utility.LayerNameToBitMap(WALL_LAYER_NAME);
            FetchDependencies(ropeDependencies, commonDependencies);
            _ropeVisualsController.Initialize(ropeAnimationDependencies, ropeDependencies.player.bodyParts.backForearm.transform);
        }

        void FetchDependencies(GrapplingRopeDependencies ropeDependencies, CommonGrapplingDependencies commonDependencies)
        {
            _ropeVisualsController = GetComponent<RopeVisualsController>();
            _foreArmRigidbody = ropeDependencies.player.bodyParts.backForearm.GetComponent<Rigidbody2D>();

            // Assumes second joint of each type is for rope (index 1)
            _springJoint2DAsRope = _foreArmRigidbody.GetComponent<SpringJoint2D>();
            _ropeAttachmentEndRigidbody = ropeDependencies.ropeAttachmentEndRigidbody;
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
            var raycastHit = Physics2D.Raycast(
                _foreArmRigidbody.transform.position,
                _foreArmRigidbody.transform.right,
                _raycastDistance,
                _wallLayerInBitMap
            );
            var tilemap = raycastHit.collider.GetComponent<Tilemap>();
            _springJoint2DAsRope.distance = raycastHit.distance;
            _ropeAttachmentEndRigidbody.transform.position = tilemap.GetCellCenterWorld(tilemap.WorldToCell(raycastHit.point));
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
