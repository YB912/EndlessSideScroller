
using DesignPatterns.ServiceLocatorPattern;
using DesignPatterns.ObserverPattern;
using UnityEngine;
using UnityEngine.InputSystem;
using DesignPatterns.EventBusPattern;

namespace InputManagement
{
    public class TouchInputManager
    {
        public readonly Observable<Vector3> currentTouchPositionInWorldObservable;

        InputManager _inputManager;
        InputEventBus _inputEventBus;
        Camera _mainCamera;

        float _cameraZ;

        internal TouchInputManager()
        {
            currentTouchPositionInWorldObservable = new Observable<Vector3>(Vector3.zero);

            FetchDependencies();
            SubscribeToEvents();
        }

        private void FetchDependencies()
        {
            _inputManager = ServiceLocator.instance.Get<InputManager>();
            _inputEventBus = ServiceLocator.instance.Get<InputEventBus>();
            _mainCamera = Camera.main;
            _cameraZ = _mainCamera.transform.position.z;
        }

        private void SubscribeToEvents()
        {
            _inputManager.inputSystemActions.Player.TouchStart.started += OnTouchStarted;
            _inputManager.inputSystemActions.Player.TouchPosition.performed += OnTouchPositionPerformed;
            _inputManager.inputSystemActions.Player.TouchEnd.performed += OnTouchEnded;
        }

        private void OnTouchStarted(InputAction.CallbackContext context)
        {
            _inputEventBus.Publish<TouchStartedEvent>();
        }

        private void OnTouchPositionPerformed(InputAction.CallbackContext context)
        {
            var posContext = context.ReadValue<Vector2>();
            var newWorldPos = _mainCamera.ScreenToWorldPoint(posContext);
            newWorldPos.z = _cameraZ;
            currentTouchPositionInWorldObservable.value = newWorldPos;
        }

        private void OnTouchEnded(InputAction.CallbackContext context)
        {
            _inputEventBus.Publish<TouchEndedEvent>();
        }
    }
}