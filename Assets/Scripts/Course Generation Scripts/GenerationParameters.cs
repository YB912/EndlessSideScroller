
using UnityEngine;

namespace Mechanics.CourseGeneration
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "GenerationParameters", menuName = "Course Generation/GenerationParameters")]
    public class GenerationParameters : ScriptableObject
    {
        [SerializeField] CourseParameters _courseParameters;
        [SerializeField] TilemapParameters _tilemapParameters;
        [SerializeField] TilePaletteReferences _tilePaletteReferences;

        public CourseParameters courseParameters => _courseParameters;
        public TilemapParameters tilemapParameters => _tilemapParameters;
        public TilePaletteReferences tilePaletteReferences => _tilePaletteReferences;
    }
}
