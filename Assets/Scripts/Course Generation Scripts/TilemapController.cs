
using DesignPatterns.EventBusPattern;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Mechanics.CourseGeneration
{
    /// <summary>
    /// Controls an individual tilemap segment in the course generation system.
    /// </summary>
    public class TilemapController : MonoBehaviour
    {
        [SerializeField] GameObject _revolvingTriggerPrefab;

        Tilemap _tilemap;
        TileSetter _tileSetter;

        GenerationParameters _parameters;
        TilemapParameters _tilemapParameters;

        public Tilemap tilemap => _tilemap;

        public void Initialize(GenerationParameters parameters, GameplayEventBus gameplayEventBus)
        {
            _parameters = parameters;
            _tilemapParameters = _parameters.tilemapParameters;
            FetchDependencies();
            SetupRevolvingTrigger(gameplayEventBus);
            Generate();
            VisualizeTilemapArea();
        }

        void FetchDependencies()
        {
            _tilemap = GetComponent<Tilemap>();
            _tileSetter = new TileSetter(_tilemap, _parameters);
        }

        public void Generate()
        {
            _tileSetter.SetTiles();
        }

        void SetupRevolvingTrigger(GameplayEventBus gameplayEventBus)
        {
            var trigger = Instantiate(_revolvingTriggerPrefab, transform);
            trigger.GetComponent<RevolvingTriggerController>().Initialize(gameplayEventBus, _tilemapParameters);
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

        private class TileSetter
        {
            TilemapParameters _tilemapParameters;
            TilePaletteReferences _paletteReferences;
            CourseParameters _courseParameters;
            Tilemap _attachedTilemap;
            public TileSetter(Tilemap attachedTilemap, GenerationParameters parameters)
            {
                _attachedTilemap = attachedTilemap;
                _tilemapParameters = parameters.tilemapParameters;
                _paletteReferences = parameters.tilePaletteReferences;
                _courseParameters = parameters.courseParameters;
            }

            public void SetTiles()
            {
                ClearAllTiles();
                GenerateCeiling();
            }

            public void ClearAllTiles()
            {
                _attachedTilemap.ClearAllTiles();
            }

            void GenerateCeiling()
            {
                var origin = new Vector2Int(0, _tilemapParameters.tilemapHeight - 1);
                FillBox(origin, _tilemapParameters.tilemapWidth, 1, _paletteReferences.whiteTile);
            }

            void FillBox(Vector2Int origin, int width, int height, TileBase fillTile)
            {
                var area = new BoundsInt(origin.x, origin.y, 0, width, height, 1);

                TileBase[] tileArray = new TileBase[width * height];
                for (int i = 0; i < tileArray.Length; i++)
                    tileArray[i] = fillTile;

                _attachedTilemap.SetTilesBlock(area, tileArray);
            }
        }
    }
}
