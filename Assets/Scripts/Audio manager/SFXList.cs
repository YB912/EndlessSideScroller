
using UnityEngine;
using System.Collections.Generic;

namespace Audio
{
    [CreateAssetMenu(fileName = "SFX List", menuName = "Audio Manager/SFX List")]
    internal class SFXList : ScriptableObject
    {
        public List<SoundEffect> list;
    }
}