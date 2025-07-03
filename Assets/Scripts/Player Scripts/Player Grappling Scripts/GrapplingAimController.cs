
using DesignPatterns.EventBusPattern;
using DesignPatterns.ServiceLocatorPattern;
using DG.Tweening;
using InputManagement;
using System.Collections;
using UnityEngine;
using UnityEngine.U2D.IK;

namespace Mechanics.Grappling
{
    /// <summary>
    /// Controls the aiming mechanics for grappling,
    /// managing IK targets, tweening aim movement, and event notifications.
    /// </summary>
    public class GrapplingAimController : MonoBehaviour
    {
        IKManager2D _IKManager;
        Transform _IKTargetTransform;
        Transform _effectorTransform;

        float _aimMovementDuration;
        float _minimumAimDistance;

        TouchInputManager _touchInputManager;
        GrapplingEventBus _grapplingEventBus;
        HingeJoint2D _forearmJoint;

        Vector3 _currentAimPosition;
        Tween _currentTween;
        float _forearmJointLimitRange;

        internal void Initialize(GrapplingAimDependencies aimDependencies, CommonGrapplingDependencies commonDependencies)
        {
            FetchDependencies(aimDependencies, commonDependencies);
            SetupEventHandlers();
            _forearmJointLimitRange = _forearmJoint.limits.max - _forearmJoint.limits.min;
            DisableIK();
        }

        public void StartAiming()
        {
            StartCoroutine(WaitForTouchPositionAndAimCoroutine());
        }

        public void EndAiming()
        {
            DisableIK();
        }

        void FetchDependencies(GrapplingAimDependencies aimDependencies, CommonGrapplingDependencies commonDependencies)
        {
            _IKManager = aimDependencies.IKManager;
            _IKTargetTransform = aimDependencies.IKTargetTransform;
            _aimMovementDuration = aimDependencies.aimMovementDuration;
            _minimumAimDistance = aimDependencies.minimumAimDistance;
            _effectorTransform = commonDependencies.effectorTransform;
            _touchInputManager = ServiceLocator.instance.Get<TouchInputManager>();
            _grapplingEventBus = ServiceLocator.instance.Get<GrapplingEventBus>();
            _forearmJoint = transform.GetChild(0).GetComponent<HingeJoint2D>();
        }

        void SetupEventHandlers()
        {
            _touchInputManager.currentTouchPositionInWorldObservable.AddListener(OnTouchPositionChanged);
        }

        IEnumerator WaitForTouchPositionAndAimCoroutine()
        {
            yield return null; // Wait one frame to ensure input updates
            Aim();
        }

        void OnTouchPositionChanged(Vector3 newPosition)
        {
            _currentAimPosition = newPosition;
        }

        void Aim()
        {
            if (IsAimPositionFarEnough())
            {
                EnableIK();
                _currentTween = _IKTargetTransform.DOMove(_currentAimPosition, _aimMovementDuration).OnComplete(OnAimingFinished);
            }
        }

        void OnAimingFinished()
        {
            _grapplingEventBus.Publish<GrapplerAimedEvent>();
        }

        void EnableIK()
        {
            CancelCurrentTween();
            _IKTargetTransform.position = _effectorTransform.position;
            _IKManager.enabled = true;
        }

        void DisableIK()
        {
            CancelCurrentTween();
            _IKManager.enabled = false;
            SyncBoneAngleLimitsWithIK();
        }

        void SyncBoneAngleLimitsWithIK()
        {
            float currentAngle = _forearmJoint.jointAngle;

            JointAngleLimits2D newLimits = new JointAngleLimits2D
            {
                min = currentAngle - _forearmJointLimitRange,
                max = currentAngle
            };

            _forearmJoint.limits = newLimits;
        }

        void CancelCurrentTween()
        {
            _currentTween?.Kill();
            _currentTween = null;
        }

        bool IsAimPositionFarEnough()
        {
            return Vector3.Distance(transform.position, _currentAimPosition) > _minimumAimDistance;
        }
    }
}
