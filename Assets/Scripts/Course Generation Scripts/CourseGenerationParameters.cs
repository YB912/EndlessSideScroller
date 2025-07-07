using UnityEngine;

namespace Mechanics.CourseGeneration
{
    /// <summary>
    /// Holds configuration parameters for procedural course generation,
    /// </summary>
    [System.Serializable]
    [CreateAssetMenu(fileName = "CourseGenerationParameters", menuName = "Course Generation/Course Generation Parameters")]
    public class CourseGenerationParameters : ScriptableObject
    {
        [Header("Number Of Elements Per Tilemap")]
        [SerializeField] int _minCeilingGapNumberPerMap;
        [SerializeField] int _maxCeilingGapNumberPerMap;
        [SerializeField] int _minPlatformNumberPerMap;
        [SerializeField] int _maxPlatformNumberPerMap;
        [SerializeField] int _minWallNumberPerMap;
        [SerializeField] int _maxWallNumberPerMap;

        [Header("Ceiling Gap Dimensions")]
        [SerializeField] int _minCeilingGapLength;
        [SerializeField] int _maxCeilingGapLength;

        [Header("Platform Dimensions")]
        [SerializeField] int _minPlatformLength;
        [SerializeField] int _maxPlatformLength;
        [SerializeField] int _minPlatformHeight;
        [SerializeField] int _maxPlatformHeight;

        [Header("Wall Dimensions")]
        [SerializeField] int _minWallLength;
        [SerializeField] int _maxWallLength;
        [SerializeField] int _minWallHeight;
        [SerializeField] int _maxWallHeight;

        [Header("Distance Of Elements")]
        [SerializeField] int _minVerticalDistance;
        [SerializeField] int _minHorizontalDistance;

        public int minCeilingGapNumberPerMap => _minCeilingGapNumberPerMap;
        public int maxCeilingGapNumberPerMap => _maxCeilingGapNumberPerMap;
        public int minPlatformNumberPerMap => _minPlatformNumberPerMap;
        public int maxPlatformNumberPerMap => _maxPlatformNumberPerMap;
        public int minWallNumberPerMap => _minWallNumberPerMap;
        public int maxWallNumberPerMap => _maxWallNumberPerMap;

        public int minCeilingGapLength => _minCeilingGapLength;
        public int maxCeilingGapLength => _maxCeilingGapLength;

        public int minPlatformLength => _minPlatformLength;
        public int maxPlatformLength => _maxPlatformLength;
        public int minPlatformHeight => _minPlatformHeight;
        public int maxPlatformHeight => _maxPlatformHeight;

        public int minWallLength => _minWallLength;
        public int maxWallLength => _maxWallLength;
        public int minWallHeight => _minWallHeight;
        public int maxWallHeight => _maxWallHeight;

        public int minVerticalDistance => _minVerticalDistance;
        public int minHorizontalDistance => _minHorizontalDistance;
    }
}
