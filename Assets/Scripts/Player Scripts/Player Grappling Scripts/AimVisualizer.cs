
using DesignPatterns.EventBusPattern;
using DesignPatterns.ServiceLocatorPattern;
using DG.Tweening;
using System.Collections;
using UnityEngine;

public class AimVisualizer : MonoBehaviour
{
    [SerializeField] SpriteRenderer _reticleVisual;
    [SerializeField] LineRenderer _aimLineVisual;
    [SerializeField] SpriteRenderer _aimLineBeginningVisual;
    [SerializeField] float _aimLineWidth = 2f;

    public AimVisualizer Initialize()
    {
        name = "AimVisualizer";
        _aimLineVisual.positionCount = 2;
        ServiceLocator.instance.Get<InputEventBus>().Subscribe<AimingTouchEndedInputEvent>(DisableVisuals);
        DisableVisuals();
        return this;
    }

    public void VisualizeAim(Vector3 touchPosition, Vector3 targetPosition)
    {
        _reticleVisual.transform.position = targetPosition;

        targetPosition.z = 0;
        _aimLineVisual.positionCount = 2;
        _aimLineVisual.widthMultiplier = _aimLineWidth;
        _aimLineVisual.SetPosition(0, touchPosition);
        _aimLineVisual.SetPosition(1, targetPosition);

        _aimLineBeginningVisual.transform.position = touchPosition;

        EnableVisuals();
    }

    void EnableVisuals()
    {
        _reticleVisual.enabled = true;
        _aimLineVisual.enabled = true;
        _aimLineBeginningVisual.enabled = true;
    }

    void DisableVisuals()
    {
        _reticleVisual.enabled = false;
        _aimLineVisual.enabled = false;
        _aimLineBeginningVisual.enabled = false;
    }
}
