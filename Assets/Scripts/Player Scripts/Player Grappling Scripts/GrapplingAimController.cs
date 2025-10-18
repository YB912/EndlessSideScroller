
using DesignPatterns.EventBusPattern;
using DesignPatterns.ServiceLocatorPattern;
using DG.Tweening;
using System.Collections;
using UI.GameplayInput;
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

        ITouchPositionProvider _touchPositionProvider;
        GrapplingEventBus _grapplingEventBus;
        HingeJoint2D _forearmJoint;

        Vector3 _currentAimPosition;
        Tween _currentTween;
        float _forearmJointLimitRange;

        public void Initialize(GrapplingAimDependencies aimDependencies, CommonGrapplingDependencies commonDependencies)
        {
            FetchDependencies(aimDependencies, commonDependencies);
            SubscribeToInput();
            _forearmJointLimitRange = _forearmJoint.limits.max - _forearmJoint.limits.min;
            DisableIK();
        }

        public void StartAiming()
        {
            StartCoroutine(WaitForTouchPositionAndAimCoroutine());
        }

        public void AimTowards(Vector3 position)
        {
            EnableIK();
            _currentTween = _IKTargetTransform.DOMove(position, _aimMovementDuration).OnComplete(OnAimingFinished);
        }

        public void AimTowardsWithDelay(Vector3 position, float delay)
        {
            StartCoroutine(AimTowardsWithDelayCoroutine(position, delay));
        }

        void FetchDependencies(GrapplingAimDependencies aimDependencies, CommonGrapplingDependencies commonDependencies)
        {
            _IKManager = aimDependencies.IKManager;
            _IKTargetTransform = aimDependencies.IKTargetTransform;
            _aimMovementDuration = aimDependencies.aimMovementDuration;
            _minimumAimDistance = aimDependencies.minimumAimDistance;
            _effectorTransform = commonDependencies.effectorTransform;
            _touchPositionProvider = ServiceLocator.instance.Get<ITouchPositionProvider>();
            _grapplingEventBus = ServiceLocator.instance.Get<GrapplingEventBus>();
            _forearmJoint = transform.GetChild(0).GetComponent<HingeJoint2D>();
        }

        void SubscribeToInput()
        {
            _touchPositionProvider.currentTouchPositionInWorldObservable.AddListener(OnTouchPositionChanged);
        }

        IEnumerator WaitForTouchPositionAndAimCoroutine()
        {
            yield return null; // Wait one frame to ensure input updates
            Aim();
        }

        IEnumerator AimTowardsWithDelayCoroutine(Vector3 position, float delay)
        {
            yield return new WaitForSeconds(delay);
            AimTowards(position);
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

        void EndAiming()
        {
            DisableIK();
        }

        void OnAimingFinished()
        {
            _grapplingEventBus.Publish<GrapplerAimedEvent>();
            EndAiming();
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
