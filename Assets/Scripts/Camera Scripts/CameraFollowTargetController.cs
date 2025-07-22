
using DesignPatterns.EventBusPattern;
using DesignPatterns.ServiceLocatorPattern;
using Unity.Cinemachine;
using UnityEngine;

namespace Mechanics.Camera
{
    /// <summary>
    /// Follows the player horizontally as proxy, locked on a fixed height.
    /// Cinemachine camera follows the GameObject this is attached to.
    /// </summary>
    public class CameraFollowTargetController : MonoBehaviour
    {
        [SerializeField, Range(0, 1)] float _playerNormalizedScreenXOutsidePlayState;

        Transform _playerAbdomenTransform;
        float _fixedY;
        bool _isFollowingThePlyer;
        float _cameraAspect;
        float _orthographicSize;

        LoadingEventBus _loadingEventBus;
        GameCycleEventBus _gameCycleEventBus;

        public void Initialize(float fixedY)
        {
            _fixedY = fixedY;
            FetchDependencies();
            SubscribeToEvents();
        }

        private void LateUpdate()
        {
            if (_isFollowingThePlyer)
            {
                SetPositionToPlayer();
            }
        }

        void FetchDependencies()
        {
            _loadingEventBus = ServiceLocator.instance.Get<LoadingEventBus>();
            _gameCycleEventBus = ServiceLocator.instance.Get<GameCycleEventBus>();
        }

        void SubscribeToEvents()
        {
            _loadingEventBus.Subscribe<PlayerInitializedEvent>(SetPlayerTransform);
            _loadingEventBus.Subscribe<CinemachineCameraInitializedEvent>(OnCinemachineCameraInitialized);
            _gameCycleEventBus.Subscribe<EnteredPlayStateGameCycleEvent>(BeginFollowingThePlayer);
            _gameCycleEventBus.Subscribe<EnteredMainMenuStateGameCycleEvent>(StopFollowingThePlayer);
        }

        void OnCinemachineCameraInitialized()
        {
            _cameraAspect = UnityEngine.Camera.main.aspect;
            _orthographicSize = ServiceLocator.instance.Get<CinemachineCamera>().Lens.OrthographicSize;
            _loadingEventBus.Unsubscribe<CinemachineCameraInitializedEvent>(OnCinemachineCameraInitialized);
        }

        void BeginFollowingThePlayer()
        {
            _isFollowingThePlyer = true;
        }

        void StopFollowingThePlayer()
        {
            _isFollowingThePlyer = false;
            SetPositionToDistance();
        }

        void SetPositionToPlayer()
        {
            if (_playerAbdomenTransform == null) return;
            transform.position = new Vector3(_playerAbdomenTransform.position.x, _fixedY, 0);
        }

        void SetPositionToDistance()
        {
            if (_playerAbdomenTransform != null)
            {
                float screenWidth = 2f * _orthographicSize * _cameraAspect;

                float halfWidth = screenWidth / 2f;
                float playerX = _playerAbdomenTransform.position.x;

                float desiredPlayerWorldX = playerX;
                float cameraX = desiredPlayerWorldX - (-halfWidth + screenWidth * _playerNormalizedScreenXOutsidePlayState);

                transform.position = new Vector3(cameraX, _fixedY, transform.position.z);
            }
        }

        void SetPlayerTransform()
        {
            _playerAbdomenTransform = ServiceLocator.instance.Get<PlayerController>().bodyParts.abdomen.transform;
            _loadingEventBus.Unsubscribe<PlayerInitializedEvent>(SetPlayerTransform);
            SetPositionToPlayer();
            _loadingEventBus.Publish<CameraFollowTargetInitializedEvent>();
        }
    }
}
