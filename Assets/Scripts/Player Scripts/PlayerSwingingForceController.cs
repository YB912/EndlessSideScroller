
using DesignPatterns.ServiceLocatorPattern;
using DG.Tweening;
using UnityEngine;

public class PlayerSwingingForceController : MonoBehaviour, IInitializeable
{
    [SerializeField] float _maxForwardAttachmentForce;
    [SerializeField] float _maxOutwardAttachmentForce;
    [SerializeField] float _detachmentForce;
    [SerializeField] float _attachmentForceDuration;
    [SerializeField] AnimationCurve _attachmentForceCurve;

    Transform _forearmTransform;
    Rigidbody2D _abdomenRigidbody;
    Rigidbody2D _backUpperLegRigidbody;
    Rigidbody2D _frontUpperLegRigidbody;

    Vector2 _inwardDirection;
    Vector2 _outwardDirection;
    Vector2 _forwardDirection;
    Vector2 _currentForwardForce;
    Vector2 _currentOutwardForce;

    Tween _attachmentForceTween;

    void FixedUpdate()
    {
        if (_currentForwardForce != Vector2.zero)
        {
            AddForwardForces();
        }
        if (_currentOutwardForce != Vector2.zero)
        {
            AddOutwardForces();
        }
    }

    public void Initialize()
    {
        var bodyParts = ServiceLocator.instance.Get<PlayerController>().bodyParts;
        _forearmTransform = bodyParts.backForearm.transform;
        _abdomenRigidbody = bodyParts.abdomen.GetComponent<Rigidbody2D>();
        _backUpperLegRigidbody = bodyParts.backUpperLeg.GetComponent<Rigidbody2D>();
        _frontUpperLegRigidbody = bodyParts.frontUpperLeg.GetComponent<Rigidbody2D>();
    }

    public void ApplyAttachmentForce()
    {
        CancelAttachmentForce();
        var curveTime = 0f;
        _attachmentForceTween = DOTween.To(() => curveTime, t => curveTime = t, 1, _attachmentForceDuration)
            .SetEase(Ease.Linear)
            .OnUpdate(() => UpdateForceVectors(curveTime));
    }

    public void CancelAttachmentForce()
    {
        _currentForwardForce = Vector2.zero;
        _currentOutwardForce = Vector2.zero;
        _attachmentForceTween?.Kill();
    }

    public void ApplyDetatchmentForce()
    {
        UpdateDirectionVectors();
        _abdomenRigidbody.AddForce(_forwardDirection * _detachmentForce, ForceMode2D.Impulse);
    }

    void UpdateDirectionVectors()
    {
        _inwardDirection = _forearmTransform.right.normalized;
        _outwardDirection = -_inwardDirection;
        _forwardDirection = new Vector2(_inwardDirection.y, -_inwardDirection.x);
    }

    void UpdateForceVectors(float curveTime)
    {
        UpdateDirectionVectors();
        _currentForwardForce = _forwardDirection * _maxForwardAttachmentForce * _attachmentForceCurve.Evaluate(curveTime);
        _currentOutwardForce = _outwardDirection * _maxOutwardAttachmentForce * _attachmentForceCurve.Evaluate(curveTime);
    }

    void AddForwardForces()
    {
        _abdomenRigidbody.AddForce(_currentForwardForce, ForceMode2D.Impulse);
        _backUpperLegRigidbody.AddForce(_currentForwardForce, ForceMode2D.Impulse);
        _frontUpperLegRigidbody.AddForce(_currentForwardForce, ForceMode2D.Impulse);
    }

    void AddOutwardForces()
    {
        _abdomenRigidbody.AddForce(_currentOutwardForce, ForceMode2D.Impulse);
        _backUpperLegRigidbody.AddForce(_currentOutwardForce, ForceMode2D.Impulse);
        _frontUpperLegRigidbody.AddForce(_currentOutwardForce, ForceMode2D.Impulse);
    }

    void OnDrawGizmos()
    {
        if (_forearmTransform != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawRay(_abdomenRigidbody.transform.position, _currentForwardForce * 0.5f);
            Gizmos.color = Color.magenta;
            Gizmos.DrawRay(_abdomenRigidbody.transform.position, _currentOutwardForce * 0.5f);
            Gizmos.color = Color.red;
            Gizmos.DrawRay(_abdomenRigidbody.transform.position, _forwardDirection * _detachmentForce * 0.5f);
        }
    }
}
