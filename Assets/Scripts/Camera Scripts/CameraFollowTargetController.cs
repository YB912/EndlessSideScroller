
using DesignPatterns.EventBusPattern;
using DesignPatterns.ServiceLocatorPattern;
using UnityEngine;

namespace Mechanics.Camera
{
    /// <summary>
    /// Follows the player horizontally as proxy, locked on a fixed height.
    /// Cinemachine camera follows the GameObject this is attached to.
    /// </summary>
    public class CameraFollowTargetController : MonoBehaviour
    {
        Transform _playerAbdomenTransform;
        float _fixedY;

        LoadingEventBus _loadingEventBus;

        public void Initialize(float fixedY)
        {
            _fixedY = fixedY;
            _loadingEventBus = ServiceLocator.instance.Get<LoadingEventBus>();
            _loadingEventBus.Subscribe<PlayerInitializedEvent>(SetPlayerTransform);
        }

        private void LateUpdate()
        {
            UpdatePosition();
        }

        void UpdatePosition()
        {
            if (_playerAbdomenTransform != null)
            {
                transform.position = new Vector3(_playerAbdomenTransform.position.x, _fixedY, 0);
            }
        }

        void SetPlayerTransform()
        {
            _playerAbdomenTransform = ServiceLocator.instance.Get<PlayerController>().bodyParts.abdomen.transform;
            _loadingEventBus.Unsubscribe<PlayerInitializedEvent>(SetPlayerTransform);
            UpdatePosition();
            _loadingEventBus.Publish<CameraFollowTargetInitializedEvent>();
        }
    }
}
