using System.Collections.Generic;
using UnityEngine;
using Utilities.Settings;

// we dont have spatial audio, so we dont need to worry about different audio sources positions
// actually we have only 1 main sound source, so that fixes our worries

namespace UniverseSound
{
    /// <summary>
    /// Universe Sound controller, here sounds are played.
    /// </summary>
    public class UniverseSoundTree : MonoBehaviour
    {
        public static UniverseSoundTree Instance;
        [SerializeField] private AudioSource source;
        /// All clips with properties which developer configures in editor
        [SerializeField] public List<UniverseSoundNode> nodes;
        // hash table is used to get better performance on loops
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
            // try to get node of that name
            var node = _hashTable.GetNodeByName(nodeName);
            // if not found - node with that name doesn't exist. we throw expected nodeName, actionName and sender which is object data container
            if (node == null)
            {
                Debug.LogError($"DIDNT FIND SOUND NODE [{nodeName}]! Error from [{actionName}, {sender}]");
                return;
            }
            // get correct sound clip
            source.clip = node.SoundClip;
            // get volume from menu settings
            float volumeFromMenu = SoundSettingsController.Instance.GetSoundSetting(node.GetSoundType).CurrentVolume *
                                   SoundSettingsController.Instance.GetSoundSetting(UniverseSoundNode.SoundType.Master).CurrentVolume;
            // adjusting volume
            source.volume = volumeFromMenu + node.Boost;
            // setting pitch and play!
            source.pitch = node.Pitch;
            source.PlayDelayed(node.Delay);
        }
    }
}