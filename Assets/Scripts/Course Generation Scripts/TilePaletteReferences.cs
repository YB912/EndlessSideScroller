
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Mechanics.CourseGeneration
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "TilePaletteReferences", menuName = "Course Generation/Tile Palette References")]
    public class TilePaletteReferences : ScriptableObject
    {
        [SerializeField] TileBase _whiteTile;

        public TileBase whiteTile => _whiteTile;
    }
}
