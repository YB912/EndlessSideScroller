
using DesignPatterns.EventBusPattern;
using DesignPatterns.ServiceLocatorPattern;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class MainCameraBootstrapper : MonoBehaviour, ISceneBootstrappable
{
    [SerializeField] private GameObject _mainCameraPrefab;
    [SerializeField] private GameObject _cinemachineCameraPrefab;

    GameObject _cinemachineCamera;

    public IEnumerator BootstrapCoroutine()
    {
        ServiceLocator.instance.Get<LoadingEventBus>().Subscribe<PlayerInitializedEvent>(OnPlayerInitialized);
        var mainCamera = Instantiate(_mainCameraPrefab);
        _cinemachineCamera = Instantiate(_cinemachineCameraPrefab);
        yield break;
    }

    void OnPlayerInitialized()
    {
        _cinemachineCamera.GetComponent<CinemachineCamera>().Follow = ServiceLocator.instance.Get<PlayerController>().bodyParts.head.transform;
    }
}
