
using UnityEngine;

namespace Player.Health
{
    [CreateAssetMenu(fileName = "BodypartDamageSettings", menuName = "Health/Bodypart Damage Settings")]
    public class BodypartDamageSettings : ScriptableObject
    {
        [SerializeField, Range(0f, 1f)] float _impactSensitivity = 1f;

        public float impactSensitivity => _impactSensitivity;
    }
}
