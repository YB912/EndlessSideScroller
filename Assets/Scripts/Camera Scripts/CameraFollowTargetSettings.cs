
using UnityEngine;

namespace Mechanics.Camera
{
    [CreateAssetMenu(fileName = "CameraFollowTargetSettings", menuName = "Camera Follow Target Settings")]
    public class CameraFollowTargetSettings : ScriptableObject
    {
        [SerializeField, Range(0, 1)] float _playerNormalizedScreenXOutsidePlayState;
        [SerializeField] float _normalizedPlayStateLookaheadAmount;
        [SerializeField] float _playerNormalizedLaunchLookAheadScreenX;
        [SerializeField] float _positionTweenDuration;

        public float playerNormalizedScreenXOutsidePlayState => _playerNormalizedScreenXOutsidePlayState;
        public float normalizedPlayStateLookaheadAmount => _normalizedPlayStateLookaheadAmount;
        public float playerNormalizedLaunchLookAheadScreenX => _playerNormalizedLaunchLookAheadScreenX;
        public float positionTweenDuration => _positionTweenDuration;
    }
}
