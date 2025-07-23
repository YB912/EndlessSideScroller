

using DesignPatterns.EventBusPattern;
using DesignPatterns.ObserverPattern;
using DesignPatterns.ServiceLocatorPattern;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputManagement
{
    /// <summary>
    /// Manages touch input by subscribing to the input system via InputManager.
    /// </summary>
    public class TouchInputManager
    {
        public readonly Observable<Vector3> currentTouchPositionInWorldObservable;

        InputManager _inputManager;  
        InputEventBus _inputEventBus;
        Camera _mainCamera;
        float _cameraZ;

        public TouchInputManager()
        {
            currentTouchPositionInWorldObservable = new Observable<Vector3>(Vector3.zero);
            FetchDependencies();
            SubscribeToEvents();
        }

        void FetchDependencies()
        {
            _inputManager = ServiceLocator.instance.Get<InputManager>();
            _inputEventBus = ServiceLocator.instance.Get<InputEventBus>();
            _mainCamera = Camera.main;
            _cameraZ = _mainCamera.transform.position.z;
        }

        void SubscribeToEvents()
        {
            _inputManager.inputSystemActions.Player.TouchStart.started += OnTouchStarted;
            _inputManager.inputSystemActions.Player.TouchPosition.performed += OnTouchPositionPerformed;
            _inputManager.inputSystemActions.Player.TouchEnd.performed += OnTouchEnded;
        }

        void OnTouchStarted(InputAction.CallbackContext context)
        {
            _inputEventBus.Publish<TouchStartedEvent>();
        }

        void OnTouchPositionPerformed(InputAction.CallbackContext context)
        {
            var screenPos = context.ReadValue<Vector2>();
            var newWorldPos = _mainCamera.ScreenToWorldPoint(screenPos);
            newWorldPos.z = _cameraZ;
            currentTouchPositionInWorldObservable.value = newWorldPos;
        }

        void OnTouchEnded(InputAction.CallbackContext context)
        {
            _inputEventBus.Publish<TouchEndedEvent>();
        }
    }
}