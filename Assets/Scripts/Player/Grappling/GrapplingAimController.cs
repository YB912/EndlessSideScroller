
using DesignPatterns.EventBusPattern;
using DesignPatterns.ServiceLocatorPattern;
using DG.Tweening;
using InputManagement;
using System.Collections;
using UnityEngine;
using UnityEngine.U2D.IK;

namespace Mechanics.Grappling
{
    public class GrapplingAimController : MonoBehaviour
    {
        IKManager2D _IKManager;
        Transform _IKTargetTransform;
        Transform _effectorTransform;

        float _aimMovementDuration;
        float _minimumAimDistance;

        TouchInputManager _touchInputManager;
        HingeJoint2D _forearmJoint;

        Vector3 _currentAimPosition;
        Tween _currentTween;
        float _forearmJointLimitRange;

        internal void Initialize(GrapplingAimDependencies aimDependencies, CommonGrapplingDependencies commonDependencies)
        {
            FetchDependencies(aimDependencies, commonDependencies);
            AddObservableListeners();
            _forearmJointLimitRange = _forearmJoint.limits.max - _forearmJoint.limits.min;
            DisableIK();
        }

        void FetchDependencies(GrapplingAimDependencies aimDependencies, CommonGrapplingDependencies commonDependencies)
        {
            _IKManager = aimDependencies.IKManager;
            _IKTargetTransform = aimDependencies.IKTargetTransform;
            _effectorTransform = commonDependencies.effectorTransform;
            _aimMovementDuration = aimDependencies.aimMovementDuration;
            _minimumAimDistance = aimDependencies.minimumAimDistance;
            _touchInputManager = ServiceLocator.instance.Get<TouchInputManager>();
            _forearmJoint = transform.GetChild(0).GetComponent<HingeJoint2D>();
        }

        void AddObservableListeners()
        {
            _touchInputManager.currentTouchPositionInWorldObservable.AddListener(OnTouchPositionChanged);
            _touchInputManager.isTouchDown.AddListener(OnTouchToggled);
            ServiceLocator.instance.Get<GrapplingEventBus>().Subscribe<GrapplerAttachedToSurfaceEvent>(DisableIK);
        }

        void OnTouchToggled(bool isTouchDown)
        {
            if (isTouchDown)
            {
                StartCoroutine(WaitForTouchPositionAndAimCoroutine());
            }
            else
            {
                DisableIK();
            }
        }

        IEnumerator WaitForTouchPositionAndAimCoroutine()
        {
            yield return null;
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
            ServiceLocator.instance.Get<GrapplingEventBus>().Publish<GrapplerAimedEvent>();
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
