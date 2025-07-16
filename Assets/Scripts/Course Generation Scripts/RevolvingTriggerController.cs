
using DesignPatterns.EventBusPattern;
using UnityEngine;

namespace Mechanics.CourseGeneration
{
    /// <summary>
    /// Controls the triggering of the tilemaps revolving via an event
    /// </summary>
    public class RevolvingTriggerController : MonoBehaviour
    {
        GameplayEventBus _gameplayEventBus;
        TilemapParameters _tilemapParameters;

        BoxCollider2D _trigger;

        const string PLAYER_ABDOMEN_TAG = "PlayerAbdomen";

        public void Initialize(GameplayEventBus gameplayEventBus, TilemapParameters tilemapParameters)
        {
            _gameplayEventBus = gameplayEventBus;
            _tilemapParameters = tilemapParameters;
            _trigger = GetComponent<BoxCollider2D>();
            SetupTrigger();
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(PLAYER_ABDOMEN_TAG))
            {
                _gameplayEventBus.Publish<PlayerPassedATilemapEvent>();
            }
        }

        void SetupTrigger()
        {
            transform.localPosition = new Vector3(
                _tilemapParameters.tilemapWidth * _tilemapParameters.GridCellSize.x * _tilemapParameters.revolvingTriggerOffsetPercentage / 100,
                _tilemapParameters.tilemapHeight / 2 * _tilemapParameters.GridCellSize.y,
                0
            );
            _trigger.size = new Vector2(
                _tilemapParameters.GridCellSize.x,
                _tilemapParameters.GridCellSize.y * _tilemapParameters.tilemapHeight
            );
            _trigger.isTrigger = true;
        }
    }
}
