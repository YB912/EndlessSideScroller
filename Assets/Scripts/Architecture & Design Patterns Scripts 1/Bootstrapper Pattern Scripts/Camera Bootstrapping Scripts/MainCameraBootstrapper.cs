
using DesignPatterns.EventBusPattern;
using DesignPatterns.ServiceLocatorPattern;
using Mechanics.CourseGeneration;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

namespace Mechanics.Camera
{
    public class MainCameraBootstrapper : MonoBehaviour, IBootstrapper
    {
        [SerializeField] private GameObject _mainCameraPrefab;
        [SerializeField] private GameObject _cinemachineCameraPrefab;
        [SerializeField] private GameObject _cameraFollowTargetPrefab;

        CinemachineCamera _cinemachineCamera;

        CameraFollowTargetController _cameraFollowTarget;


        public IEnumerator BootstrapCoroutine()
        {
            CreateCameraFollowTarget();
            CreateCamera();
            yield break;
        }

        void CreateCamera()
        {
            var mainCamera = Instantiate(_mainCameraPrefab);
            mainCamera.name = "MainCamera";
            mainCamera.GetComponent<IInitializeable>().Initialize();

            _cinemachineCamera = Instantiate(_cinemachineCameraPrefab, mainCamera.transform).GetComponent<CinemachineCamera>();
            _cinemachineCamera.name = "CinemachineVirtualCamera";
            _cinemachineCamera.GetComponent<CinemachineCameraController>().Initialize(_cameraFollowTarget);
        }

        void CreateCameraFollowTarget()
        {
            var courseGenerationManager = ServiceLocator.instance.Get<CourseGenerationManager>();
            _cameraFollowTarget = Instantiate(_cameraFollowTargetPrefab).GetComponent<CameraFollowTargetController>();
            _cameraFollowTarget.name = "CameraFollowTarget";
            _cameraFollowTarget.Initialize(courseGenerationManager.cameraHeight);
        }
    }
}
