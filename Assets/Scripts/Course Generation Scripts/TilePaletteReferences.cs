
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Mechanics.CourseGeneration
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "TilePaletteReferences", menuName = "Course Generation/Tile Palette References")]
    public class TilePaletteReferences : ScriptableObject
    {
        [SerializeField] TileBase _solidBlock;
        [SerializeField] TileBase _ceilingLaser;

        public TileBase solidBlock => _solidBlock;
        public TileBase ceilingLaser => _ceilingLaser;
    }
}
