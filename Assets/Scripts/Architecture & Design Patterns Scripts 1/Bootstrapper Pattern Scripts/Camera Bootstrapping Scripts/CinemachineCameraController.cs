
using DesignPatterns.EventBusPattern;
using DesignPatterns.ServiceLocatorPattern;
using Mechanics.CourseGeneration;
using Unity.Cinemachine;
using UnityEngine;

namespace Mechanics.Camera
{
    public class CinemachineCameraController : MonoBehaviour
    {
        CinemachineCamera _virtualCamera;
        CameraFollowTargetController _followTarget;
        TilemapParameters _tilemapParameters;

        LoadingEventBus _loadingEventBus;

        public void Initialize(CameraFollowTargetController followTarget)
        {
            _followTarget = followTarget;
            FetchDependencies();
            AdjustCameraLens();
            _loadingEventBus.Subscribe<CameraFollowTargetInitializedEvent>(OnFollowTargetInitialized);
            _loadingEventBus.Publish<CinemachineCameraInitializedEvent>();
            // Need to enable the wider dead zone only when play state begins and disable it otherwise
        }

        void FetchDependencies()
        {
            _virtualCamera = GetComponent<CinemachineCamera>();
            _loadingEventBus = ServiceLocator.instance.Get<LoadingEventBus>();
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
    }
}
