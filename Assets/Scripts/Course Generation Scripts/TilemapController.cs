
using DesignPatterns.EventBusPattern;
using UnityEngine;

namespace Mechanics.CourseGeneration
{
    /// <summary>
    /// Controls an individual tilemap segment in the course generation system.
    /// </summary>
    public class TilemapController : MonoBehaviour
    {
        BoxCollider2D _revolvingTrigger;

        TilemapParameters _parameters;

        GameplayEventBus _gameplayEventBus;

        const string PLAYER_ABDOMEN_TAG = "PlayerAbdomen";

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(PLAYER_ABDOMEN_TAG))
            {
                _gameplayEventBus.Publish<PlayerPassedATilemapEvent>();
            }
        }

        public void Initialize(TilemapParameters parameters, GameplayEventBus gameplayEventBus)
        {
            _parameters = parameters;
            _gameplayEventBus = gameplayEventBus;
            FetchDependencies();
            SetupRevolvingTrigger();
            VisualizeTilemapArea();
        }

        public void Generate()
        {

        }

        void FetchDependencies()
        {
            _revolvingTrigger = GetComponent<BoxCollider2D>();
        }

        void SetupRevolvingTrigger()
        {
            _revolvingTrigger.size = new Vector2(
                _parameters.GridCellSize.x,
                _parameters.GridCellSize.y * _parameters.tilemapHeight * 2
            );

            _revolvingTrigger.offset = new Vector2(
                _parameters.tilemapWidth * _parameters.GridCellSize.x * _parameters.revolvingTriggerOffsetPercentage / 100,
                0
            );
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

            var width = _parameters.GridCellSize.x * _parameters.tilemapWidth;
            var height = _parameters.GridCellSize.y * _parameters.tilemapHeight;

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
