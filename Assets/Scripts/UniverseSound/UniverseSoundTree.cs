using System.Collections.Generic;
using UnityEngine;
using Utilities.Settings;

// we dont have audio space, so we dont need to worry about different audio sources positions

namespace UniverseSound
{
    public class UniverseSoundTree : MonoBehaviour
    {
        // nodes - all clips with properties which developer configures in editor
        // hash table is used to get better performance on loops
        public static UniverseSoundTree Instance;
        [SerializeField] private AudioSource source;
        [SerializeField] public List<UniverseSoundNode> nodes;
        private readonly UniverseSoundHash _hashTable = new ();
        
        public void Awake() => Instance = this;

        void Start()
        {
            // initializing hashset
            foreach(var node in nodes)
                _hashTable.AddToSet(node);
        }
        
        public void PlaySoundByName(string nodeName, string actionName, object sender)
        {
            var node = _hashTable.GetNodeByName(nodeName);
            if (node == null)
            {
                Debug.LogError($"DIDNT FIND SOUND NODE {nodeName}! Error from {actionName}, {sender}");
                return;
            }
            source.clip = node.SoundClip;
            float volumeFromMenu = SoundSettingsController.Instance.GetSoundSetting(node.GetSoundType).CurrentVolume *
                                   SoundSettingsController.Instance.GetSoundSetting(UniverseSoundNode.SoundType.Master).CurrentVolume;
            source.volume = volumeFromMenu + node.Boost;
            Debug.Log(volumeFromMenu);
            source.pitch = node.Pitch;
            source.PlayDelayed(node.Delay);
        }
    }
}