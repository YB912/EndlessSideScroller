
using DesignPatterns.EventBusPattern;
using DesignPatterns.ServiceLocatorPattern;
using DG.Tweening;
using System.Collections;
using UnityEngine;

public class AimVisualizer : MonoBehaviour
{
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] LineRenderer _lineRenderer;

    public AimVisualizer Initialize()
    {
        name = "AimVisualizer";
        _lineRenderer.positionCount = 2;
        ServiceLocator.instance.Get<InputEventBus>().Subscribe<AimingTouchEndedInputEvent>(DisableVisuals);
        DisableVisuals();
        return this;
    }

    public void VisualizeAim(Vector3 touchPosition, Vector3 targetPosition)
    {
        targetPosition.z = 0;
        _lineRenderer.positionCount = 2;
        _lineRenderer.SetPosition(0, touchPosition);
        _lineRenderer.SetPosition(1, targetPosition);
        _spriteRenderer.transform.position = targetPosition;
        EnableVisuals();

        //StartCoroutine(BreakAfterDelay());
    }

    void EnableVisuals()
    {
        _spriteRenderer.enabled = true;
        _lineRenderer.enabled = true;
    }

    void DisableVisuals()
    {
        _spriteRenderer.enabled = false;
        _lineRenderer.enabled = false;
    }

    IEnumerator BreakAfterDelay()
    {
        yield return new WaitForSeconds(2);
        Debug.Break();
    }
}
