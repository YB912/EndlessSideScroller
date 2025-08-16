
using UnityEngine;

namespace Mechanics.CourseGeneration
{
    public class TilemapEntryTriggerController : TilemapTriggerController
    {
        protected override string nameInHierarchy => "EntryTrigger";

        protected override void OnPlayerEnteredTrigger()
        {
            _gamePlayEventBus.Publish<PlayerEnteredATilemapGameplayEvent>();
        }

        protected override void SetupTrigger()
        {
            transform.localPosition = new Vector3(
                0,
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
