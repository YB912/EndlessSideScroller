
using UnityEngine;

namespace Audio
{
    internal abstract class Sound
    {
        public AudioClip clip;
        [Range(0, 1)] public float volume = 1f;

        protected AudioSource source;

        public virtual void InitializeAudioSource(GameObject parent)
        {
            if (source == null)
            {
                source = parent.AddComponent<AudioSource>();
            }
            source.clip = clip;
            source.volume = volume;
        }
    }
}
