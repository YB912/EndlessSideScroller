
using UnityEngine;
using System.Collections.Generic;

namespace Audio
{
    [System.Serializable]
    internal class Music : Sound
    {
        public const float DEFAULT_BUILDUP_DURATION = 3f;
        public const float DEFAULT_FADEOUT_DURATION = 3f;

        public MusicType type;

        public static List<AudioSource> musicSourcePool = new List<AudioSource>();

        public override void InitializeAudioSource(GameObject parent)
        {
            base.InitializeAudioSource(parent);
            if (musicSourcePool.Contains(source) == false)
            {
                musicSourcePool.Add(source);
            }
        }

        public void Play(bool isLooped = true)
        {
            StopAllSuddenly();
            source.loop = isLooped;
            source.volume = volume;
            source.clip = clip;
            source.Play();
        }

        public static void StopAllSuddenly()
        {
            foreach (var source in musicSourcePool)
            {
                if (source.isPlaying) { source.Stop(); }
            }
        }
    }

    public enum MusicType
    {
        NONE,
        BACKGROUND_MUSIC
    }
}
