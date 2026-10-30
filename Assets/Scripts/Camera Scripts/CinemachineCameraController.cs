
using DesignPatterns.EventBusPattern;
using DesignPatterns.ServiceLocatorPattern;
using Mechanics.CourseGeneration;
using Unity.Cinemachine;
using UnityEngine;

namespace Mechanics.Camera
{
    public class CinemachineCameraController : MonoBehaviour
    {
        [SerializeField] CameraShakeSettings _cameraShakeSettings;
        CinemachineCamera _virtualCamera;
        CameraFollowTargetController _followTarget;
        CameraShakeController _shakeController;
        TilemapParameters _tilemapParameters;
        CinemachineBasicMultiChannelPerlin _cinemachinePerlin;

        LoadingEventBus _loadingEventBus;
        GameCycleEventBus _gameCycleEventBus;
        GameplayEventBus _gameplayEventBus;

        public void Initialize(CameraFollowTargetController followTarget)
        {
            _followTarget = followTarget;
            FetchDependencies();
            AdjustCameraLens();
            _shakeController = new CameraShakeController(_cameraShakeSettings, _cinemachinePerlin, _gameCycleEventBus, _gameplayEventBus, this);
            _loadingEventBus.Subscribe<CameraFollowTargetInitializedLoadingEvent>(OnFollowTargetInitialized);
            _loadingEventBus.Publish<CinemachineCameraInitializedLoadingEvent>();
            // Need to enable the wider dead zone only when play state begins and disable it otherwise
        }

        void FetchDependencies()
        {
            _virtualCamera = GetComponent<CinemachineCamera>();
            _cinemachinePerlin = GetComponent<CinemachineBasicMultiChannelPerlin>();
            _loadingEventBus = ServiceLocator.instance.Get<LoadingEventBus>();
            _gameCycleEventBus = ServiceLocator.instance.Get<GameCycleEventBus>();
            _gameplayEventBus = ServiceLocator.instance.Get<GameplayEventBus>();
            _tilemapParameters = ServiceLocator.instance.Get<CourseGenerationManager>().parameters.tilemapParameters;
        }

        void OnFollowTargetInitialized()
        {
            SetCameraPosition();
            SetupCameraFollow();
        }

        void AdjustCameraLens()
        {
            _virtualCamera.Lens.OrthographicSize = _tilemapParameters.tilemapHeight * _tilemapParameters.GridCellSize.y / 2;
        }

        void SetupCameraFollow()
        {
            _virtualCamera.Follow = _followTarget.transform;
        }

        void SetCameraPosition()
        {
            _virtualCamera.ForceCameraPosition(_followTarget.transform.position, Quaternion.identity);
        }

        private void OnDestroy()
        {
            _loadingEventBus.Unsubscribe<CameraFollowTargetInitializedLoadingEvent>(OnFollowTargetInitialized);
            _shakeController.Dispose();
        }
    }
}
