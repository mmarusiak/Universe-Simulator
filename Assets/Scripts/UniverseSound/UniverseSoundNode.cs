using System;
using System.Collections.Generic;
using UnityEngine;

namespace UniverseSound
{
    [Serializable]
    public class UniverseSoundNode
    {
        [SerializeField] public string name;
        [SerializeField] private AudioClip _soundClip;
        [SerializeField] private float _delay, _pitch, _boost;

        public string Name => name;
        public AudioClip SoundClip => _soundClip;
        public float Delay => _delay;
        public float Pitch => _pitch;
        public float Boost => _boost;

        public UniverseSoundNode(AudioClip soundClip, float delay = 0, float pitch = 0, float boost = 0)
        {
            _soundClip = soundClip;
            _delay = delay;
            _pitch = pitch;
            _boost = boost;
        }
    }
}