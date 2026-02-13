
using UnityEngine;
using System.Collections.Generic;

namespace Audio
{
    internal class SFXManager
    {
        private SFXList sfxList;
        private Dictionary<SoundEffectType, SoundEffect> SFXDictionary = new Dictionary<SoundEffectType, SoundEffect>();

        public SFXManager(SFXList sfxList, GameObject audioSourcesParent)
        {
            this.sfxList = sfxList;

            foreach (var soundEffect in sfxList.list)
            {
                SFXDictionary[soundEffect.type] = soundEffect;
            }

            foreach (var soundEffect in sfxList.list)
            {
                soundEffect.InitializeAudioSource(audioSourcesParent);
            }
        }

        public void PlayRandomPitch(SoundEffectType type)
        {
            GetSoundEffect(type).PlayRandomPitch();
        }

        public void PlayDefaultPitch(SoundEffectType type)
        {
            GetSoundEffect(type).PlayDefaultPitch();
        }

        private SoundEffect GetSoundEffect(SoundEffectType type)
        {
            if (SFXDictionary.ContainsKey(type))
            {
                return SFXDictionary[type];
            }
            else
            {
                Debug.LogWarning($"Sound effect '{type}' not found.");
                return new SoundEffect { type = SoundEffectType.NONE };
            }
        }
    }
}
