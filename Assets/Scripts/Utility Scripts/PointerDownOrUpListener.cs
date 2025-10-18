
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class PointerDownOrUpListener : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public event Action onPressed;
    public event Action onReleased;

    public void OnPointerDown(PointerEventData eventData)
    {
        onPressed?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        onReleased?.Invoke();
    }
}
