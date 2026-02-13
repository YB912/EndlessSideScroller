
using UnityEngine;

namespace Mechanics.CourseGeneration
{
    public class TilemapCeilingDeathTriggerController : TilemapTriggerController
    {
        protected override string nameInHierarchy => "CeilingDeathTrigger";

        string PLAYER_BODYPARTS_LAYER_NAME = "PlayerBodyParts";

        protected override void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer(PLAYER_BODYPARTS_LAYER_NAME))
            {
                OnPlayerEnteredTrigger();
            }
        }

        protected override void OnPlayerEnteredTrigger()
        {
            _gamePlayEventBus.Publish<PlayerHitADeathTriggerGameplayEvent>();
            _gamePlayEventBus.Publish<PlayerHitCeilingLaserGameplayEvent>();
        }

        protected override void SetupTrigger()
        {
            transform.localPosition = new Vector3(
                _tilemapParameters.tilemapWidth / 2 * _tilemapParameters.GridCellSize.x,
                (_tilemapParameters.tilemapHeight * _tilemapParameters.GridCellSize.y) + _tilemapParameters.ceilingDeathTriggerYOffsetInTiles,
                0
            );
            _trigger.size = new Vector2(
                _tilemapParameters.GridCellSize.x * _tilemapParameters.tilemapWidth,
                _tilemapParameters.GridCellSize.y / 2
            );
        }
    }
}
