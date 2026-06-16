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
            // Connect slider and toggle
            volumeSlider.onValueChanged.AddListener(UpdateVolume);
            volumeToggle.onValueChanged.AddListener(MuteVolume);

            // Initialize values
            volumeSlider.value = UserSettings.Instance.Volume;
            volumeToggle.isOn = UserSettings.Instance.IsMute;
        }

        /// <summary>
        /// Change the volume of the application and update the volume display
        /// </summary>
        /// <param name="_value"></param>
        private void UpdateVolume(float _value)
        {
            volumeDisplay.text = (int)(_value * 100f) + " %";
            UserSettings.Instance.Volume = _value;

            // Need to link that to the sound manager, to update its volume based on the UserSettings.Instance.Volume
        }

        /// <summary>
        /// Mute/unmute the volume of the application
        /// </summary>
        /// <param name="_value"></param>
        private void MuteVolume(bool _value)
        {
            UserSettings.Instance.IsMute = _value;

            // Need to link that to the sound manager, to mute/unmute the volume based on the UserSettings.Instance.IsMute
        }
    }
}

