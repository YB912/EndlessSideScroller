
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

        float _cameraZ;

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
            isTouchDown.value = true;
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
            isTouchDown.value = false;
        }
    }
}