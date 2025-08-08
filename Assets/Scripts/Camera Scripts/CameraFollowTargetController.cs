
using DesignPatterns.EventBusPattern;
using DesignPatterns.ServiceLocatorPattern;
using DG.Tweening;
using Unity.Cinemachine;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

namespace Mechanics.Camera
{
    /// <summary>
    /// Follows the player horizontally as proxy, locked on a fixed height.
    /// Cinemachine camera follows the GameObject this is attached to.
    /// </summary>
    public class CameraFollowTargetController : MonoBehaviour
    {
        [SerializeField] CameraFollowTargetSettings _settings;

        Transform _playerAbdomenTransform;
        float _fixedY;
        bool _isFollowingThePlyer;
        float _cameraAspect;
        float _orthographicSize;
        float _screenWidth;
        float _lookahead;

        LoadingEventBus _loadingEventBus;
        GameCycleEventBus _gameCycleEventBus;
        GameplayEventBus _gamePlayEventBus;

        bool isAheadOfPlayer
        {
            get
            {
                if (_playerAbdomenTransform != null)
                {
                    return transform.position.x > _playerAbdomenTransform.position.x + _lookahead;
                }
                return false;
            }
        }

        public void Initialize(float fixedY)
        {
            _fixedY = fixedY;
            FetchDependencies();
            SubscribeToEvents();
        }

        private void LateUpdate()
        {
            if (_isFollowingThePlyer && isAheadOfPlayer == false)
            {
                SetPositionToPlayer();
            }
        }

        void FetchDependencies()
        {
            _loadingEventBus = ServiceLocator.instance.Get<LoadingEventBus>();
            _gameCycleEventBus = ServiceLocator.instance.Get<GameCycleEventBus>();
            _gamePlayEventBus = ServiceLocator.instance.Get<GameplayEventBus>();
        }

        void SubscribeToEvents()
        {
            _loadingEventBus.Subscribe<PlayerInitializedEvent>(SetPlayerTransform);
            _loadingEventBus.Subscribe<CinemachineCameraInitializedEvent>(OnCinemachineCameraInitialized);

            _gameCycleEventBus.Subscribe<EnteredPlayStateGameCycleEvent>(() =>
            {
                StopFollowingPlayer();
                LookAheadOfPlayer();
            });
            _gamePlayEventBus.Subscribe<LaunchSequenceCompletedGameplayEvent>(BeginFollowingPlayer);
            _gameCycleEventBus.Subscribe<EnteredMainMenuStateGameCycleEvent>(() =>
            {
                StopFollowingPlayer();
                SetPositionToOutsidePlayState();
            });
            _gamePlayEventBus.Subscribe<PlayerDiedGameplayEvent>(StopFollowingPlayer);
            _gameCycleEventBus.Subscribe<RagdollResetGameCycleEvent>(SetPositionToOutsidePlayState);
        }

        void OnCinemachineCameraInitialized()
        {
            _cameraAspect = UnityEngine.Camera.main.aspect;
            _orthographicSize = ServiceLocator.instance.Get<CinemachineCamera>().Lens.OrthographicSize;
            _loadingEventBus.Unsubscribe<CinemachineCameraInitializedEvent>(OnCinemachineCameraInitialized);
            _screenWidth = 2f * _orthographicSize * _cameraAspect;
            _lookahead = _screenWidth * _settings.normalizedPlayStateLookaheadAmount;
        }

        void BeginFollowingPlayer()
        {
            _isFollowingThePlyer = true;
        }

        void StopFollowingPlayer()
        {
            _isFollowingThePlyer = false;
        }

        void SetPositionToPlayer()
        {
            if (_playerAbdomenTransform == null) return;
            transform.position = new Vector3(_playerAbdomenTransform.position.x + _lookahead, _fixedY, 0);
        }

        void LookAheadOfPlayer()
        {
            TweenPositionToDistance(_settings.playerNormalizedLaunchLookAheadScreenX);
        }

        void SetPositionToOutsidePlayState()
        {
            SetPositionToDistance(_settings.playerNormalizedScreenXOutsidePlayState);
        }

        void SetPositionToDistance(float playerNormalizedX)
        {
            transform.position = PositionWhenPlayerIsVisibleAt(playerNormalizedX);
        }

        void TweenPositionToDistance(float playerNormalizedX)
        {
            var destination = PositionWhenPlayerIsVisibleAt(playerNormalizedX);
            transform.DOMove(destination, _settings.positionTweenDuration)
                .OnComplete(_gamePlayEventBus.Publish<CameraTargetReachedTweenPositionGameplayEvent>);
        }

        Vector3 PositionWhenPlayerIsVisibleAt(float playerNormalizedX)
        {
            if (_playerAbdomenTransform != null)
            {
                float halfWidth = _screenWidth / 2f;
                float playerX = _playerAbdomenTransform.position.x;

                float desiredPlayerWorldX = playerX;
                float cameraX = desiredPlayerWorldX - (-halfWidth + _screenWidth * playerNormalizedX);

                return new Vector3(cameraX, _fixedY, transform.position.z);
            }
            Debug.LogError("CameraFollowTargetController.PositionWhenPlayerIsVisibleAt(): _playerAbdomenTransform is null.");
            return Vector3.zero;
        }

        void SetPlayerTransform()
        {
            _playerAbdomenTransform = ServiceLocator.instance.Get<Player.PlayerController>().bodyParts.abdomen.transform;
            _loadingEventBus.Unsubscribe<PlayerInitializedEvent>(SetPlayerTransform);
            SetPositionToPlayer();
            _loadingEventBus.Publish<CameraFollowTargetInitializedEvent>();
        }
    }
}
