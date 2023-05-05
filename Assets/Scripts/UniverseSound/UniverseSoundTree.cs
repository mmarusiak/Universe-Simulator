using System.Collections.Generic;
using UnityEngine;

// we dont have audio space, so we dont need to worry about different audio sources positions

namespace UniverseSound
{
    public class UniverseSoundTree : MonoBehaviour
    {
        public static UniverseSoundTree Instance;
        [SerializeField] private AudioSource source;
        [SerializeField] private List<UniverseSoundNode> nodes;
        
        
        public void Awake() => Instance = this;
        
        public void PlaySoundByName(string nodeName)
        {
            foreach (var node in nodes)
            {
                if (node.Name != nodeName)
                    continue;
                source.clip = node.SoundClip;
                source.pitch = node.Pitch;
                source.volume = node.Boost + 1;
                source.PlayDelayed(node.Delay);
                break;
            }
        }
    }
}