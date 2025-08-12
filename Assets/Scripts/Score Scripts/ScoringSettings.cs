
using UnityEngine;

namespace Mechanics.Scoring
{
    [CreateAssetMenu(fileName ="ScoringSettings", menuName ="Scoring Settings")]
    public class ScoringSettings : ScriptableObject
    {
        [SerializeField] float _distanceScoreMultiplier;

        public float distanceScoreMultiplier => _distanceScoreMultiplier;
    }
}
