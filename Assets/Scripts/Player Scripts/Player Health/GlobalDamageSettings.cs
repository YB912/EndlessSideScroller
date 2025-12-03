
using UnityEngine;

namespace Player.Health
{
    [CreateAssetMenu(fileName = "GlobalDamageSettings", menuName = "Health/Global Damage Settings")]
    public class GlobalDamageSettings : ScriptableObject
    {
        [SerializeField] float _maxImpactDamage;
        [SerializeField] float _impactForceThreshold;
        [SerializeField] float _maxImpactForce;
        [SerializeField] AnimationCurve _impactForceToDamageCurve;

        public float maxImpactDamage => _maxImpactDamage;
        public float maxImpactForce => _maxImpactForce;
        public float impactForceThreshold => _impactForceThreshold;
        public AnimationCurve impactForceToDamageCurve => _impactForceToDamageCurve;
    }
}
