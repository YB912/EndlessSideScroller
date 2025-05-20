
using DesignPatterns.ServiceLocatorPattern;
using DG.Tweening;
using InputManagement;
using System.Collections;
using UnityEngine;
using UnityEngine.U2D.IK;

public class GrappleAimController : MonoBehaviour, IInitializeable
{
    [SerializeField] IKManager2D _IKManager;
    [SerializeField] Transform _IKTargetTransform;
    [SerializeField] Transform _effectorTransform;
    HingeJoint2D _forearmJoint;

    [SerializeField] float _aimMovementDuration;

    TouchInputManager _touchInputManager;

    Vector3 _currentAimPosition;
    Tween _currentTween;
    float _forearmJointLimitRange;

    public void Initialize()
    {
        _touchInputManager = ServiceLocator.instance.Get<TouchInputManager>();
        _touchInputManager.currentTouchPositionInWorldObservable.AddListener(OnTouchPositionChanged);
        _touchInputManager.isTouchDown.AddListener(OnTouchToggled);
        _forearmJoint = transform.GetChild(0).GetComponent<HingeJoint2D>();
        _forearmJointLimitRange = _forearmJoint.limits.max - _forearmJoint.limits.min;
        DisableIK();
    }

    private void OnTouchToggled(bool isTouchDown)
    {
        if (isTouchDown)
        {
            StartCoroutine(WaitForTouchPositionAndAimCoroutine());
        }
        else
        {
            OnTouchEnded();
        }
    }

    private IEnumerator WaitForTouchPositionAndAimCoroutine()
    {
        yield return null;
        Aim();
    }

    private void OnTouchPositionChanged(Vector3 newPosition)
    {
        _currentAimPosition = newPosition;
    }

    private void OnTouchEnded()
    {
        CancelCurrentTween();
        DisableIK();
    }

    private void Aim()
    {
        CancelCurrentTween();
        EnableIK();
        _currentTween = _IKTargetTransform.DOMove(_currentAimPosition, _aimMovementDuration);
    }

    private void EnableIK()
    {
        _IKTargetTransform.position = _effectorTransform.position;
        _IKManager.enabled = true;
    }

    private void DisableIK()
    {
        _IKManager.enabled = false;
        SyncBonesRotationWithIK();
        _IKTargetTransform.position = _effectorTransform.position;
    }

    private void SyncBonesRotationWithIK()
    {
        float currentAngle = _forearmJoint.jointAngle;

        JointAngleLimits2D newLimits = new JointAngleLimits2D
        {
            min = currentAngle - _forearmJointLimitRange,
            max = currentAngle
        };

        _forearmJoint.limits = newLimits;
    }

    private void CancelCurrentTween()
    {
        _currentTween?.Kill();
        _currentTween = null;
    }
}
