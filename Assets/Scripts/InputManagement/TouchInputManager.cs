
using DesignPatterns.ServiceLocatorPattern;
using DesignPatterns.ObserverPattern;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputManagement
{
    public class TouchInputManager
    {
        public readonly Observable<Vector3> currentTouchPositionInWorldObservable;
        public readonly Observable<bool> isTouchDown;

        InputManager _inputManager;
        Camera _mainCamera;

        internal TouchInputManager()
        {
            currentTouchPositionInWorldObservable = new Observable<Vector3>(Vector3.zero);
            isTouchDown = new Observable<bool>(false);

            FetchDependencies();
            SubscribeToEvents();
        }

        private void FetchDependencies()
        {
            _inputManager = ServiceLocator.instance.Get<InputManager>();
            _mainCamera = Camera.main;
        }

        private void SubscribeToEvents()
        {
            _inputManager.inputSystemActions.Player.TouchStart.started += OnTouchStarted;
            _inputManager.inputSystemActions.Player.TouchEnd.performed += OnTouchEnded;
        }

        private void OnTouchStarted(InputAction.CallbackContext context)
        {
            isTouchDown.value = true;
            var posContext = _inputManager.inputSystemActions.Player.TouchPosition.ReadValue<Vector2>();
            currentTouchPositionInWorldObservable.value = _mainCamera.ViewportToWorldPoint(posContext);
        }

        private void OnTouchEnded(InputAction.CallbackContext context)
        {
            isTouchDown.value = false;
        }
    }
}