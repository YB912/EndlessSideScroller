
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
        [Tooltip("Depends on the tilemap width. Can also be < 0 && 100 < and put it outside of the tilemap.")]
        [SerializeField] float _revolvingTriggerOffsetPercentage;

        public Vector3 GridCellSize { get => _gridCellSize; }

        public GameObject TilemapPrefab { get => _tilemapPrefab; set => _tilemapPrefab = value; }
        public int numberOfTilemaps { get => _numberOfTilemaps; }
        public int tilemapWidth { get => _tilemapWidth; }
        public int tilemapHeight { get => _tilemapHeight; }
        public float revolvingTriggerOffsetPercentage { get => _revolvingTriggerOffsetPercentage; }
    }
}
