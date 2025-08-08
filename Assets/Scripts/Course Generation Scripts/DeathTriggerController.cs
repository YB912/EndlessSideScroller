
using UnityEngine;

namespace Mechanics.CourseGeneration
{
    public class DeathTriggerController : TilemapTriggerController
    {
        protected override string nameInHierarchy => "DeathTrigger";

        protected override void OnPlayerEnteredTrigger()
        {
            _gamePlayEventBus.Publish<PlayerHitADeathTriggerGameplayEvent>();
        }

        protected override void SetupTrigger()
        {
            transform.localPosition = new Vector3(
                _tilemapParameters.tilemapWidth / 2 * _tilemapParameters.GridCellSize.x,
                _tilemapParameters.deathTriggerYOffsetInTiles * _tilemapParameters.GridCellSize.y,
                0
            );
            _trigger.size = new Vector2(
                _tilemapParameters.GridCellSize.x * _tilemapParameters.tilemapWidth,
                _tilemapParameters.GridCellSize.y
            );
        }
    }
}
