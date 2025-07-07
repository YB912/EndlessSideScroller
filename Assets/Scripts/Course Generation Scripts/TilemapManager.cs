
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using DesignPatterns.EventBusPattern;
using DesignPatterns.ServiceLocatorPattern;

namespace Mechanics.CourseGeneration
{
    /// <summary>
    /// Manages a sequence of tilemaps for an infinite scrolling or procedural level.
    /// </summary>
    public class TilemapManager : MonoBehaviour, IInitializeable
    {
        [SerializeField] TilemapParameters _tilemapParameters;
        Queue<TilemapController> _tilemaps = new();

        GameplayEventBus _gameplayEventBus;

        float _tilemapDistanceOnX;

        const string TILEMAP_NAME = "Tilemap";

        public TilemapParameters tilemapParameters { get => _tilemapParameters; }

        public void Initialize()
        {
            FetchDependencies();
            _tilemaps = new Queue<TilemapController>(GetComponentsInChildren<TilemapController>().ToList());
            _tilemapDistanceOnX = _tilemapParameters.GridCellSize.x * _tilemapParameters.tilemapWidth;
            _gameplayEventBus.Subscribe<PlayerPassedATilemapEvent>(RevolveTilemaps);
            CreateTilemaps();
            ServiceLocator.instance.Register(this);
        }

        void FetchDependencies()
        {
            _gameplayEventBus = ServiceLocator.instance.Get<GameplayEventBus>();
        }

        void CreateTilemaps()
        {
            for (var i = 0; i < _tilemapParameters.numberOfTilemaps; i++)
            {
                var position = TilemapPositionOfIndex(i);
                var tilemap = Instantiate(_tilemapParameters.TilemapPrefab, position, Quaternion.identity, transform).GetComponent<TilemapController>();
                tilemap.Initialize(_tilemapParameters, _gameplayEventBus);
                tilemap.name = TILEMAP_NAME + (i + 1).ToString();
                _tilemaps.Enqueue(tilemap);
            }
        }

        void RevolveTilemaps()
        {
            var tilemapBehind = _tilemaps.Dequeue();
            var newPosition = tilemapBehind.transform.position;
            newPosition.x += _tilemapParameters.numberOfTilemaps * _tilemapDistanceOnX;
            tilemapBehind.transform.position = newPosition;
            tilemapBehind.Generate();
            _tilemaps.Enqueue(tilemapBehind);
        }

        Vector3 TilemapPositionOfIndex(int index)
        {
            var x = _tilemapDistanceOnX * index;
            return new Vector3(x, 0, 0);
        }
    }
}