using System;
using UnityEngine;

namespace Utilities.Settings
{
    public class SoundSlider : MonoBehaviour
    {
        [SerializeField] private SoundSettings mySetting;
        
        public void OnSliderChanged(Single input) => mySetting.SetVolume((int) input);

        private void Start() => SoundSettingsController.Instance.AddNewSound(mySetting);
    }
}