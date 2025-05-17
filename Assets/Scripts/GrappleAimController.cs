
using DesignPatterns.ServiceLocatorPattern;
using InputManagement;
using UnityEngine;

public class GrappleAimController : MonoBehaviour, IInitializeable
{
    [SerializeField] HingeJoint2D _backForeArmJoint;
    HingeJoint2D _backUpperArmJoint;

    TouchInputManager _touchInputManager;

    void Awake()
    {
        _backUpperArmJoint = GetComponent<HingeJoint2D>();
    }

    public void Initialize()
    {
        _touchInputManager = ServiceLocator.instance.Get<TouchInputManager>();
        _touchInputManager.currentTouchPositionInWorldObservable.AddListener(OnTouchPositionChanged);
        _touchInputManager.isTouchDown.AddListener(OnTouchToggled);
        Debug.Log($"GrappleAimController.Initialize");
    }

    private void OnTouchPositionChanged(Vector3 newPosition)
    {
        Debug.Log($"GrappleAimController.OnTouchPositionChanged: {newPosition}");
    }

    private void OnTouchToggled(bool isTouchDown)
    {
        Debug.Log($"GrappleAimController.OnTouchToggled: {isTouchDown}");
    }
}
