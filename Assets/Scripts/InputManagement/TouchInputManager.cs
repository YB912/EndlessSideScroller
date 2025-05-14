
using DesignPatterns.ServiceLocatorPattern;
using DesignPatterns.ObserverPattern;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputManagement
{
    public class TouchInputManager
    {
        public readonly Observable<Vector3> currentTouchPositionInWorldObservable;

        InputManager _inputManager;
        Camera _mainCamera;

        internal TouchInputManager()
        {
            currentTouchPositionInWorldObservable = new Observable<Vector3>(Vector3.zero);

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
            _inputManager.inputSystemActions.Player.Touch.started += OnTouchStarted;
        }

        private void OnTouchStarted(InputAction.CallbackContext context)
        {
            currentTouchPositionInWorldObservable.value = _mainCamera.ViewportToWorldPoint(context.ReadValue<Vector2>());
        }
    }
}