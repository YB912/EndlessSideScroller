
using UnityEngine;
using System.Collections.Generic;

namespace Audio
{
    internal class MusicManager
    {
        private MusicList musicList;
        private Dictionary<MusicType, Music> MusicDictionary = new Dictionary<MusicType, Music>();

        public static MusicType currectlyPlaying;

        public MusicManager(MusicList musicList, GameObject audioSourcesParent)
        {
            this.musicList = musicList;

            foreach (var music in musicList.list)
            {
                MusicDictionary[music.type] = music;
            }

            foreach (var music in musicList.list)
            {
                music.InitializeAudioSource(audioSourcesParent);
            }
        }

        public void PlaySuddenly(MusicType type, bool isLooped = true)
        {
            if (currectlyPlaying == type) { return; }
            GetMusic(type).Play(isLooped);
            currectlyPlaying = type;
        }

        private Music GetMusic(MusicType type)
        {
            if (MusicDictionary.ContainsKey(type))
            {
                return MusicDictionary[type];
            }
            else
            {
                Debug.LogWarning($"Music '{type}' not found.");
                return new Music { type = MusicType.NONE };
            }
        }
    }
}
