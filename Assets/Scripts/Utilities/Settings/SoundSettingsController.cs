using System;
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
            if(_settings[(int)settings.SettingSoundType] != null) Debug.LogWarning("Added multiple settings type to: " + settings.SettingSoundType.ToString());
                _settings[(int)settings.SettingSoundType] = settings;
        }

        public SoundSettings GetSoundSetting(UniverseSoundNode.SoundType settingType) => _settings[(int) settingType];
    }
}