
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using DesignPatterns.EventBusPattern;
using DesignPatterns.ServiceLocatorPattern;
using System;

namespace Mechanics.CourseGeneration
{
    /// <summary>
    /// Manages a sequence of tilemaps for an infinite scrolling or procedural level.
    /// </summary>
    public class CourseGenerationManager : MonoBehaviour, IInitializeable
    {
        [SerializeField] GenerationParameters _parameters;
        TilemapParameters _tilemapParameters;

        Queue<TilemapController> _tilemapControllers = new();

        GameplayEventBus _gameplayEventBus;

        float _tilemapDistanceOnX;

        const string TILEMAP_NAME = "Tilemap";

        public GenerationParameters parameters => _parameters;

        public Vector3 playerSpawnPoint => PlayerSpawnPoint();
        public float cameraHeight => CameraHeightRelativeToCourse();

        public void Initialize()
        {
            FetchDependencies();
            _tilemapControllers = new Queue<TilemapController>(GetComponentsInChildren<TilemapController>().ToList());
            _tilemapDistanceOnX = _tilemapParameters.GridCellSize.x * _tilemapParameters.tilemapWidth;
            _gameplayEventBus.Subscribe<PlayerPassedATilemapEvent>(RevolveTilemaps);
            CreateTilemaps();
            ServiceLocator.instance.Register(this);
        }

        void FetchDependencies()
        {
            _tilemapParameters = _parameters.tilemapParameters;
            _gameplayEventBus = ServiceLocator.instance.Get<GameplayEventBus>();
        }

        void CreateTilemaps()
        {
            for (var i = 0; i < _tilemapParameters.numberOfTilemaps; i++)
            {
                var position = TilemapPositionOfIndex(i);
                var tilemapController = Instantiate(_tilemapParameters.TilemapPrefab, position, Quaternion.identity, transform).
                    GetComponent<TilemapController>();
                tilemapController.Initialize(_parameters, _gameplayEventBus);
                tilemapController.name = TILEMAP_NAME + (i + 1).ToString();
                _tilemapControllers.Enqueue(tilemapController);
            }
        }

        void RevolveTilemaps()
        {
            var tilemapControllerBehind = _tilemapControllers.Dequeue();
            var newPosition = tilemapControllerBehind.transform.position;
            newPosition.x += _tilemapParameters.numberOfTilemaps * _tilemapDistanceOnX;
            tilemapControllerBehind.transform.position = newPosition;
            tilemapControllerBehind.Generate();
            _tilemapControllers.Enqueue(tilemapControllerBehind);
        }

        Vector3 TilemapPositionOfIndex(int index)
        {
            var x = _tilemapDistanceOnX * index;
            return new Vector3(x, 0, 0);
        }

        Vector3 PlayerSpawnPoint()
        {
            var tilemapController = _tilemapControllers.Peek();
            if (tilemapController == null) return Vector3.zero;
            var tile = new Vector3Int(_tilemapParameters.playerSpawnTile.x, _tilemapParameters.playerSpawnTile.y, 0);
            return tilemapController.tilemap.CellToWorld(tile);
        }

        float CameraHeightRelativeToCourse()
        {
            var tilemapController = _tilemapControllers.Peek();
            if (tilemapController == null)
            {
                throw new NullReferenceException
                    ("CourseGenerationManager.CameraHeightRelativeToCourse(): Tried getting camera height while _tilemapControllers is empty");
            }
            var originWorldHeight = tilemapController.tilemap.CellToWorld(Vector3Int.zero).y;
            var highestCellWorldHeight = tilemapController.tilemap.CellToWorld(new Vector3Int(0, _tilemapParameters.tilemapHeight - 1, 0)).y;
            return (originWorldHeight + highestCellWorldHeight) / 2;
        }
    }
}