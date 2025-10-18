
using DesignPatterns.EventBusPattern;
using DesignPatterns.ObserverPattern;
using DesignPatterns.ServiceLocatorPattern;
using InputManagement;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI.GameplayInput
{
    public interface IInputUIPresenter
    {
        public void StartAimingTouch();
        public void EndAimingTouch();
        public void BackgroundTouch();
        public static IInputUIPresenter Create(IInputUIView view)
        {
            return new InputUIPresenter(view);
        }
    }

    public interface ITouchPositionProvider
    {
        public Observable<Vector3> currentTouchPositionInWorldObservable {  get; }
        public Observable<Vector2> currentTouchPositionInScreenObservable { get; }
    }

    public class InputUIPresenter : IInputUIPresenter, ITouchPositionProvider
    {

        private Observable<Vector3> _currentTouchPositionInWorldObservable;
        private Observable<Vector2> _currentTouchPositionInScreenObservable;

        InputManager _inputManager;

        IInputUIView _view;
        InputEventBus _inputEventBus;
        GameCycleEventBus _gameCycleEventBus;
        GameplayEventBus _gameplayEventBus;

        Camera _mainCamera;
        float _cameraZ;

        public Observable<Vector3> currentTouchPositionInWorldObservable => _currentTouchPositionInWorldObservable;
        public Observable<Vector2> currentTouchPositionInScreenObservable => _currentTouchPositionInScreenObservable;

        public InputUIPresenter(IInputUIView view)
        {
            _currentTouchPositionInWorldObservable = new Observable<Vector3>(Vector3.zero);
            _currentTouchPositionInScreenObservable = new Observable<Vector2>(Vector2.zero);
            FetchDependencies();
            SubscribeToEvents();
            _view = view;
        }

        public void StartAimingTouch()
        {
            _inputEventBus.Publish<AimingTouchStartedInputEvent>();
        }

        public void EndAimingTouch()
        {
            _inputEventBus.Publish<AimingTouchEndedInputEvent>();
        }

        public void BackgroundTouch()
        {
            _view.Blink();
        }

        void FetchDependencies()
        {
            _inputManager = ServiceLocator.instance.Get<InputManager>();
            _inputEventBus = ServiceLocator.instance.Get<InputEventBus>();
            _gameCycleEventBus = ServiceLocator.instance.Get<GameCycleEventBus>();
            _gameplayEventBus = ServiceLocator.instance.Get<GameplayEventBus>();
            _mainCamera = Camera.main;
            _cameraZ = _mainCamera.transform.position.z;
        }

        void SubscribeToEvents()
        {
            _inputManager.inputSystemActions.Player.TouchPosition.performed += OnAimingTouchPositionPerformed;
            _gameCycleEventBus.Subscribe<EnteredPlayStateGameCycleEvent>(OnEnteredPlayState);
            _gameplayEventBus.Subscribe<PlayerDiedGameplayEvent>(OnPlayerDied);
        }

        void OnAimingTouchPositionPerformed(InputAction.CallbackContext context)
        {
            var screenPos = context.ReadValue<Vector2>();
            var newWorldPos = _mainCamera.ScreenToWorldPoint(screenPos);
            newWorldPos.z = _cameraZ;
            _currentTouchPositionInWorldObservable.value = newWorldPos;
            _currentTouchPositionInScreenObservable.value = screenPos;
        }

        void OnEnteredPlayState()
        {
            _view.FadePanelIn();
        }

        void OnPlayerDied()
        {
            _view.FadePanelOut();
        }
    }
}
