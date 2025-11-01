
using DesignPatterns.EventBusPattern;
using DesignPatterns.ObserverPattern;
using DesignPatterns.ServiceLocatorPattern;
using InputManagement;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI.GameplayInputAndHUD
{
    public interface IInputAndHUDPresenter
    {
        public void StartAimingTouch();
        public void EndAimingTouch();
        public void BackgroundTouch();
        public static IInputAndHUDPresenter Create(IInputAndHUDView view)
        {
            return new InputAndHUDPresenter(view);
        }
    }

    public interface ITouchPositionProvider
    {
        public Observable<Vector3> currentTouchPositionInWorldObservable {  get; }
        public Observable<Vector2> currentTouchPositionInScreenObservable { get; }
    }

    public class InputAndHUDPresenter : IInputAndHUDPresenter, ITouchPositionProvider
    {

        private Observable<Vector3> _currentTouchPositionInWorldObservable;
        private Observable<Vector2> _currentTouchPositionInScreenObservable;

        InputManager _inputManager;

        IInputAndHUDView _view;
        InputEventBus _inputEventBus;
        GameCycleEventBus _gameCycleEventBus;
        GameplayEventBus _gameplayEventBus;
        LoadingEventBus _loadingEventBus;

        PlayerStopDeathController _playerStopDeathController;

        Camera _mainCamera;
        float _cameraZ;

        public Observable<Vector3> currentTouchPositionInWorldObservable => _currentTouchPositionInWorldObservable;
        public Observable<Vector2> currentTouchPositionInScreenObservable => _currentTouchPositionInScreenObservable;

        public InputAndHUDPresenter(IInputAndHUDView view)
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
            var serviceLocator = ServiceLocator.instance;
            _inputManager = serviceLocator.Get<InputManager>();
            _inputEventBus = serviceLocator.Get<InputEventBus>();
            _gameCycleEventBus = serviceLocator.Get<GameCycleEventBus>();
            _gameplayEventBus = serviceLocator.Get<GameplayEventBus>();
            _loadingEventBus = serviceLocator.Get<LoadingEventBus>();
            _mainCamera = Camera.main;
            _cameraZ = _mainCamera.transform.position.z;
        }

        void SubscribeToEvents()
        {
            _inputManager.inputSystemActions.Player.TouchPosition.performed += OnAimingTouchPositionPerformed;
            _gameCycleEventBus.Subscribe<EnteredPlayStateGameCycleEvent>(OnEnteredPlayState);
            _gameplayEventBus.Subscribe<PlayerDiedGameplayEvent>(OnPlayerDied);
            _loadingEventBus.Subscribe<PlayerInitializedLoadingEvent>(OnPlayerLoaded);
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

        void OnCountDownNumberChanged(int number)
        {
            if (number <= 0) return;
            _view.DisplayStopDeathCountdownNumber(number);
        }

        void OnPlayerLoaded()
        {
            _playerStopDeathController = ServiceLocator.instance.Get<PlayerStopDeathController>();
            _playerStopDeathController.countdownNumberToDisplay.AddListener(OnCountDownNumberChanged);
        }
    }
}
