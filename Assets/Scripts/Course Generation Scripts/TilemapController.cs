
using DesignPatterns.EventBusPattern;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

namespace Mechanics.CourseGeneration
{
    /// <summary>
    /// Controls an individual tilemap segment in the course generation system.
    /// </summary>
    public partial class TilemapController : MonoBehaviour
    {
        [SerializeField] GameObject _entryTriggerPrefab;
        [SerializeField] GameObject _revolvingTriggerPrefab;
        [SerializeField] GameObject _floorDeathTriggerPrefab;
        [SerializeField] GameObject _ceilingDeathTriggerPrefab;

        Tilemap _tilemap;
        TileSetter _tileSetter;

        GenerationParameters _parameters;
        TilemapParameters _tilemapParameters;

        // 
        byte[,] _occupancyMatrix;
        List<Vector2Int> _emptyCells;

        public Tilemap tilemap => _tilemap;

        /// <summary>
        /// 0 = Empty, 1 = Solid Tile, 2 = Ceiling Gap
        /// </summary>
        public byte[,] occupancyMatrix => _occupancyMatrix;
        public List<Vector2Int> emptyCells => _emptyCells;

        public void Initialize(GenerationParameters parameters, GameplayEventBus gameplayEventBus)
        {
            _parameters = parameters;
            _tilemapParameters = _parameters.tilemapParameters;
            _occupancyMatrix = new byte[_parameters.tilemapParameters.tilemapWidth, _parameters.tilemapParameters.tilemapHeight];
            FetchDependencies();
            SetupTriggers(gameplayEventBus);
            //VisualizeTilemapArea();
        }

        void FetchDependencies()
        {
            _tilemap = GetComponent<Tilemap>();
            _tileSetter = new TileSetter(this, _parameters);
        }

        public void Generate(bool isStartingTilemap = false)
        {
            if (isStartingTilemap)
            {
                _tileSetter.SetCeilingTilesOnly();
            }
            else
            {
                _tileSetter.SetTiles();
            }
        }

        public void ResetTilemap(bool isStartingTilemap)
        {
            _tileSetter.ClearAllTiles();
            Generate(isStartingTilemap);
        }

        void SetupTriggers(GameplayEventBus gameplayEventBus)
        {
            SetupEntryTrigger(gameplayEventBus);
            SetupRevolvingTrigger(gameplayEventBus);
            SetupFloorDeathTrigger(gameplayEventBus);
            SetupCeilingDeathTrigger(gameplayEventBus);
        }

        void SetupEntryTrigger(GameplayEventBus gameplayEventBus)
        {
            var trigger = Instantiate(_entryTriggerPrefab, transform);
            trigger.GetComponent<TilemapEntryTriggerController>().Initialize(gameplayEventBus, _tilemapParameters);
        }

        void SetupRevolvingTrigger(GameplayEventBus gameplayEventBus)
        {
            var trigger = Instantiate(_revolvingTriggerPrefab, transform);
            trigger.GetComponent<TilemapRevolvingTriggerController>().Initialize(gameplayEventBus, _tilemapParameters);
        }

        void SetupFloorDeathTrigger(GameplayEventBus gameplayEventBus)
        {
            var trigger = Instantiate(_floorDeathTriggerPrefab, transform);
            trigger.GetComponent<TilemapFloorDeathTriggerController>().Initialize(gameplayEventBus, _tilemapParameters);
        }

        void SetupCeilingDeathTrigger(GameplayEventBus gameplayEventBus)
        {
            var trigger = Instantiate(_ceilingDeathTriggerPrefab, transform);
            trigger.GetComponent<TilemapCeilingDeathTriggerController>().Initialize(gameplayEventBus, _tilemapParameters);
        }

        void VisualizeTilemapArea()
        {
            var visual = new GameObject("TilemapVisualizer");
            visual.transform.SetParent(transform);
            visual.transform.localPosition = Vector3.zero;

            var renderer = visual.AddComponent<SpriteRenderer>();
            renderer.sprite = GenerateWhiteTexture();
            renderer.drawMode = SpriteDrawMode.Sliced;
            renderer.color = new Color(Random.value, Random.value, Random.value, 0.2f); 

            var width = _tilemapParameters.GridCellSize.x * _tilemapParameters.tilemapWidth;
            var height = _tilemapParameters.GridCellSize.y * _tilemapParameters.tilemapHeight;

            renderer.size = new Vector2(width, height);
            renderer.sortingOrder = -1000; 
        }

        Sprite GenerateWhiteTexture()
        {
            Texture2D tex = new Texture2D(1, 1);
            tex.SetPixel(0, 0, Color.white);
            tex.Apply();

            Rect rect = new Rect(0, 0, 1, 1);
            Vector2 pivot = new Vector2(0f, 0f);
            return Sprite.Create(tex, rect, pivot, 100f);
        }
    }
}
