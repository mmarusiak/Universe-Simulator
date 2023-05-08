using System.Collections.Generic;
using UnityEngine;
using UniverseSound;

namespace Utilities.Settings
{
    public class SoundSettingsController : MonoBehaviour
    {
        public static SoundSettingsController Instance;

        private readonly List<SoundSettings> _soundsSettings = new();
        private SoundSettings _master;

        void Awake() => Instance = this;

        public void AddNewSound(SoundSettings settings)
        {
            if (settings.SettingSoundType == UniverseSoundNode.SoundType.Master)
            {
                _master = settings;
                return;
            }
            _soundsSettings.Add(settings);
            Debug.Log("added");
        }

        public SoundSettings GetSoundSetting(UniverseSoundNode.SoundType settingType)
        {
            if (settingType == UniverseSoundNode.SoundType.Master)
                return _master;
            foreach (SoundSettings sound in _soundsSettings)
            {
                if (sound.SettingSoundType == settingType)
                    return sound;
            }

            return null;
        }
    }
}