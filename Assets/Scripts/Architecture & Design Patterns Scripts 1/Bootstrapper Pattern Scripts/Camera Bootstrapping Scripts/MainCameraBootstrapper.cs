
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

        Transform _cameraModulesHolder;
        GameObject _mainCamera;
        CinemachineCamera _cinemachineCamera;
        CameraFollowTargetController _cameraFollowTarget;


        public IEnumerator BootstrapCoroutine()
        {
            _cameraModulesHolder = new GameObject("CameraModulesHolder").transform;
            CreateMainCamera();
            CreateCameraFollowTarget();
            CreateCinemachineCamera();
            yield break;
        }

        void CreateMainCamera()
        {
            _mainCamera = Instantiate(_mainCameraPrefab, _cameraModulesHolder);
            _mainCamera.name = "MainCamera";
            _mainCamera.GetComponent<IInitializeable>().Initialize();
        }

        void CreateCameraFollowTarget()
        {
            var courseGenerationManager = ServiceLocator.instance.Get<CourseGenerationManager>();
            _cameraFollowTarget = Instantiate(_cameraFollowTargetPrefab, _cameraModulesHolder).GetComponent<CameraFollowTargetController>();
            _cameraFollowTarget.name = "CameraFollowTarget";
            _cameraFollowTarget.Initialize(courseGenerationManager.cameraHeight);
        }

        void CreateCinemachineCamera()
        {
            _cinemachineCamera = Instantiate(_cinemachineCameraPrefab, _cameraModulesHolder).GetComponent<CinemachineCamera>();
            _cinemachineCamera.name = "CinemachineVirtualCamera";
            ServiceLocator.instance.Register(_cinemachineCamera);
            _cinemachineCamera.GetComponent<CinemachineCameraController>().Initialize(_cameraFollowTarget);
        }
    }
}
