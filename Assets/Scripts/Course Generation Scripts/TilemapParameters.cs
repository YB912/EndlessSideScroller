
using UnityEngine;

namespace Mechanics.CourseGeneration
{
    /// <summary>
    /// Holds configuration parameters related to tilemap management
    /// </summary>
    [System.Serializable]
    [CreateAssetMenu(fileName = "TilemapParameters", menuName = "Course Generation/Tilemap Parameters")]
    public class TilemapParameters : ScriptableObject
    {
        [SerializeField] Vector3 _gridCellSize;

        [SerializeField] GameObject _tilemapPrefab;
        [SerializeField] int _numberOfTilemaps;
        [SerializeField] int _tilemapWidth;
        [SerializeField] int _tilemapHeight;
        [Tooltip("Can also be < 0 && 100 < and put it outside of the tilemap.")]
        [SerializeField] float _revolvingTriggerOffsetPercentage;
        [SerializeField] int _floorDeathTriggerYOffsetInTiles;
        [SerializeField] int _ceilingDeathTriggerYOffsetInTiles;

        [SerializeField] Vector2Int _playerSpawnTile;

        public Vector3 GridCellSize { get => _gridCellSize; }

        public GameObject TilemapPrefab => _tilemapPrefab;
        public int numberOfTilemaps => _numberOfTilemaps;
        public int tilemapWidth => _tilemapWidth;
        public int tilemapHeight => _tilemapHeight;
        public float revolvingTriggerOffsetPercentage => _revolvingTriggerOffsetPercentage;
        public float floorDeathTriggerYOffsetInTiles => _floorDeathTriggerYOffsetInTiles;
        public float ceilingDeathTriggerYOffsetInTiles => _ceilingDeathTriggerYOffsetInTiles;
        public Vector2Int playerSpawnTile => _playerSpawnTile;
    }
}
