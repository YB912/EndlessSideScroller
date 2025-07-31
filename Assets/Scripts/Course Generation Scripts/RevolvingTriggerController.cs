
using UnityEngine;

namespace Mechanics.CourseGeneration
{
    /// <summary>
    /// Controls the triggering of the tilemaps revolving via an event
    /// </summary>
    public class RevolvingTriggerController : TilemapTrigger
    {
        protected override string nameInHierarchy => "RevolvingTrigger";

        protected override void OnPlayerEnteredTrigger()
        {
            _gamePlayEventBus.Publish<PlayerPassedATilemapEvent>();
        }

        protected override void SetupTrigger()
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
