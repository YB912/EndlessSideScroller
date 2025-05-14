
using DesignPatterns.ServiceLocatorPattern;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputManagement
{
    public class TouchInputManager
    {
        public readonly Observable<Vector3> currentTouchPositionInWorldObservable;

        InputManager _inputManager;
        Camera _mainCamera;

        public TouchInputManager()
        {

            FetchDependencies();
            SubscribeToEvents();
        }

        private void FetchDependencies()
        {
            _inputManager = ServiceLocator.global.Get<InputManager>();
            _mainCamera = Camera.main;
        }

        private void SubscribeToEvents()
        {
            _inputManager.inputSystemActions.Player.Touch.started += OnTouchStarted;
        }

        private void OnTouchStarted(InputAction.CallbackContext context)
        {
            currentTouchPositionInWorldObservable.value = _mainCamera.ViewportToWorldPoint(context.ReadValue<Vector2>());
        }
    }
}