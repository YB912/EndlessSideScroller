
using UnityEngine;

namespace Player.Health
{
    [CreateAssetMenu(fileName = "PlayerHealthSettings", menuName = "Health/Player Health Settings")]
    public class PlayerHealthSettings : ScriptableObject
    {
        [SerializeField] float _playerMaxHealth;
        [SerializeField, Range(0f, 1f)] float _normalizedRegenerationPerSecond;
        [SerializeField] float _timeBeforeRegenerationStarts;

        public float playerMaxHealth => _playerMaxHealth;
        public float normalizedRegenerationPerSecond => _normalizedRegenerationPerSecond;
        public float timeBeforeRegenerationStarts => _timeBeforeRegenerationStarts;
    }
}
