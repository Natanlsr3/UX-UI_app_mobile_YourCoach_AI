using UnityEngine;
using UnityEngine.UI;

namespace MVC.App.UI.SecondaryMenu.Settings
{
    // This class is just for the display of the button and not the language change
    // Language change is handle by the LanguageMenu script

    public class LanguageButton : MonoBehaviour
    {
        [SerializeField] private Sprite toggleOff;
        [SerializeField] private Sprite toggleOn;

        private Image display;
        public Button Button { get => GetComponentInChildren<Button>(); }

        void Start()
        {
            display = GetComponentInChildren<Image>();
        }

        /// <summary>
        /// Set the display of the button : selected or not
        /// </summary>
        /// <param name="_isOn"></param>
        public void SetState(bool _isOn)
        {
            if (_isOn) display.sprite = toggleOn;
            else display.sprite = toggleOff;
        }
    }
}

