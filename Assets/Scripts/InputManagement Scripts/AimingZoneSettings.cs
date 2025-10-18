
using UnityEngine;

namespace InputManagement
{
    [CreateAssetMenu(fileName = "AimingZoneSettings", menuName = "Aiming Zone Settings")]
    public class AimingZoneSettings : ScriptableObject
    {
        [SerializeField, Range(0.1f, 0.6f)] float _aimingZoneXAnchorMinNormalized;
        [SerializeField, Range(0.3f, 0.7f)] float _aimingZoneXAnchorMaxNormalized;

        public float aimingZoneAnchorMinNormalized => _aimingZoneXAnchorMinNormalized;
        public float aimingZoneAnchorMaxNormalized => _aimingZoneXAnchorMaxNormalized;
    }
}
