
using UnityEngine;
using System.Collections.Generic;

namespace Audio
{
    [CreateAssetMenu(fileName = "Music List", menuName = "Audio Manager/Music List")]
    internal class MusicList : ScriptableObject
    {
        public List<Music> list;
    }
}
