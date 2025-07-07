
using DesignPatterns.EventBusPattern;
using DesignPatterns.ServiceLocatorPattern;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class MainCameraBootstrapper : MonoBehaviour, IBootstrapper
{
    [SerializeField] private GameObject _mainCameraPrefab;
    [SerializeField] private GameObject _cinemachineCameraPrefab;

    GameObject _cinemachineCamera;

    LoadingEventBus _loadingEventBus;

    public IEnumerator BootstrapCoroutine()
    {
        FetchDependencies();
        _loadingEventBus.Subscribe<PlayerInitializedEvent>(OnPlayerInitialized);

        var mainCamera = Instantiate(_mainCameraPrefab);
        mainCamera.GetComponent<IInitializeable>().Initialize();

        _cinemachineCamera = Instantiate(_cinemachineCameraPrefab, mainCamera.transform);
        _cinemachineCamera.GetComponent<IInitializeable>().Initialize();

        yield break;
    }

    void FetchDependencies()
    {
        _loadingEventBus = ServiceLocator.instance.Get<LoadingEventBus>();
    }

    void OnPlayerInitialized()
    {
        _cinemachineCamera.GetComponent<CinemachineCamera>().Follow = ServiceLocator.instance.Get<PlayerController>().bodyParts.head.transform;
    }
}
