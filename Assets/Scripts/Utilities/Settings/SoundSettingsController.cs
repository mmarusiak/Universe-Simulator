using System;
using System.Collections.Generic;
using UnityEngine;
using UniverseSound;

namespace Utilities.Settings
{
    public class SoundSettingsController : MonoBehaviour
    {
        public static SoundSettingsController Instance;

        private readonly SoundSettings[] _settings = new SoundSettings[Enum.GetNames(typeof(UniverseSoundNode.SoundType)).Length];
        private SoundSettings _master;

        void Awake() => Instance = this;

        public void AddNewSound(SoundSettings settings)
        {
            _settings[(int)settings.SettingSoundType] = settings;
            Debug.Log("added");
        }

        public SoundSettings GetSoundSetting(UniverseSoundNode.SoundType settingType) => _settings[(int) settingType];
    }
}