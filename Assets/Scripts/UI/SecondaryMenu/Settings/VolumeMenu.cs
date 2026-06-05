using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MVC.App.UI.SecondaryMenu.Settings
{
    public class VolumeMenu : MonoBehaviour
    {
        [SerializeField] private Slider volumeSlider;
        [SerializeField] private TextMeshProUGUI volumeDisplay;

        [SerializeField] private Toggle volumeToggle;

        void Start()
        {
            volumeSlider.onValueChanged.AddListener(UpdateVolume);
            volumeToggle.onValueChanged.AddListener(MuteVolume);

            volumeSlider.value = UserSettings.Instance.Volume;
            volumeToggle.isOn = UserSettings.Instance.IsMute;
        }

        private void UpdateVolume(float _value)
        {
            volumeDisplay.text = (int)(_value * 100f) + " %";
            UserSettings.Instance.Volume = _value;
        }

        private void MuteVolume(bool _value)
        {
            UserSettings.Instance.IsMute = _value;
        }
    }
}

