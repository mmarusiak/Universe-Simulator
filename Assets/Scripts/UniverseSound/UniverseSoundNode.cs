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
            Music,
            UIButtonsEffects
        }
        
        [SerializeField] public string name;
        [SerializeField] private AudioClip soundClip; 
        [SerializeField] private int delay;
        [Range(-100, 100)] [SerializeField] private int boost;
        [Range(-300, 300)] [SerializeField] private int pitch;
        [SerializeField] private SoundType soundType;
        
        public string Name => name;
        public AudioClip SoundClip => soundClip;
        // ReSharper disable once PossibleLossOfFraction
        public float Delay => delay;
        // ReSharper disable once PossibleLossOfFraction
        public float Pitch => pitch/100;
        // ReSharper disable once PossibleLossOfFraction
        public float Boost => boost/100;
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