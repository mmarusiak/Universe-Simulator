using System;
using UnityEngine;
using UniverseSound;

namespace Utilities.Settings
{
    [Serializable]
    public class SoundSettings
    {
        [SerializeField] private string settingName;
        [SerializeField] private UniverseSoundNode.SoundType settingSoundType;
        [SerializeField] private int currentVolume = 100;

        public UniverseSoundNode.SoundType SettingSoundType => settingSoundType;
        public int SetVolume(int volume) => currentVolume = volume;
        // ReSharper disable once PossibleLossOfFraction
        public float CurrentVolume => currentVolume/100;
    }
}