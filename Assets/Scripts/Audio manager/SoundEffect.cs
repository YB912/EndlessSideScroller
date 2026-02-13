
using UnityEngine;

namespace Audio
{
    [System.Serializable]
    internal class SoundEffect : Sound
    {
        public SoundEffectType type;
        public float defaultPitch = 1f;
        public float minimumRandomPitch;
        public float maximumRandomPitch;

        public override void InitializeAudioSource(GameObject parent)
        {
            base.InitializeAudioSource(parent);              
            if (source == null)
            {
                source.pitch = defaultPitch;
            }
        }

        public void PlayDefaultPitch()
        {
            ResetPitch();
            source.PlayOneShot(clip, volume);
        }

        public void PlayRandomPitch()
        {
            SetRandomPitch();
            source.PlayOneShot(clip, volume);
        }

        private void SetRandomPitch()
        {
            source.pitch = Random.Range(minimumRandomPitch, maximumRandomPitch);
        }

        private void ResetPitch()
        {
            source.pitch = defaultPitch;
        }
    }

    public enum SoundEffectType
    {
        NONE,
        HOOK_WHOOSH,
        HOOK_ATTACH,
        DAMAGE,
        LASER_HIT,
        DEATH,
        GAME_START,
    }
}
