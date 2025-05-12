
using DesignPatterns.ServiceLocatorPattern;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputManagement
{
    public class TouchInputManager
    {
        Vector3 _currentTouchPositionInWorld; // TO DO: This needs to become an observable

        InputManager _inputManager;
        Camera _mainCamera;

        public Vector3 currentTouchPositionInWorld => _currentTouchPositionInWorld;

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
            _inputManager.inputSystemActions.Player.Touch.started += UpdateCurrentTouchPositionInWorld;
        }

        private void UpdateCurrentTouchPositionInWorld(InputAction.CallbackContext context)
        {
            _currentTouchPositionInWorld = _mainCamera.ViewportToWorldPoint(context.ReadValue<Vector2>());
        }
    }
}