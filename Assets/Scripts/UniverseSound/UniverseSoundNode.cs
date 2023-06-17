using System;
using UnityEngine;

namespace UniverseSound
{
    [Serializable]
    public class UniverseSoundNode
    {
        public enum SoundType
        {
            Master,
            MenuEffects,
            GameEffects,
            GameMusic,
            MenuMusic
        }
        
        [SerializeField] public string name;
        [SerializeField] private AudioClip soundClip; 
        [SerializeField] private int delay;
        [Range(-100, 100)] [SerializeField] private int boost;
        [Range(-300, 300)] [SerializeField] private int pitch;
        [SerializeField] private SoundType soundType;
        
        public string Name => name;
        public AudioClip SoundClip => soundClip;
        public float Delay => delay;
        public float Pitch => (float) pitch/100;
        public float Boost => (float) boost/100;
        public SoundType GetSoundType => soundType;

        public UniverseSoundNode(AudioClip soundClip, SoundType soundType, int delay = 0, int pitch = 0, int boost = 0)
        {
            this.soundClip = soundClip;
            this.delay = delay;
            this.pitch = pitch;
            this.boost = boost;
            this.soundType = soundType;
        }
    }
}