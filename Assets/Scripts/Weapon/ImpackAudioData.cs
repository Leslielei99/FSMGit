using System.ComponentModel.Design;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Scripts.Weapon
{
    [CreateAssetMenu(menuName = "FPS/Impack Audio Data")]
    public class ImpackAudioData : ScriptableObject
    {
        public List<ImpackTagsWithAudio> ImpackTagsWithAduios;
    }
    [System.Serializable]
    public class ImpackTagsWithAudio
    {
        public string Tag;
        public List<AudioClip> ImpackAudioClip;
    }
}

