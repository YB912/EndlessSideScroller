
using UnityEngine;

namespace Player.Health
{
    [CreateAssetMenu(fileName = "PlayerHealthSettings", menuName = "Health/Player Health Settings")]
    public class PlayerHealthSettings : ScriptableObject
    {
        [SerializeField] float _playerMaxHealth;

        public float playerMaxHealth => _playerMaxHealth;
    }
}
