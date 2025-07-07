
using DesignPatterns.ServiceLocatorPattern;
using Mechanics.CourseGeneration;
using Unity.Cinemachine;
using UnityEngine;

public class CinemachineCameraController : MonoBehaviour, IInitializeable
{
    CinemachineCamera _virtualCamera;
    TilemapParameters _tilemapParameters;

    public void Initialize()
    {
        FetchDependencies();
        AdjustCameraLens();
    }

    void FetchDependencies()
    {
        _virtualCamera = GetComponent<CinemachineCamera>();
        _tilemapParameters = ServiceLocator.instance.Get<TilemapManager>().tilemapParameters;
    }

    void AdjustCameraLens()
    {
        _virtualCamera.Lens.OrthographicSize = _tilemapParameters.tilemapHeight * _tilemapParameters.GridCellSize.y / 2;
    }
}
