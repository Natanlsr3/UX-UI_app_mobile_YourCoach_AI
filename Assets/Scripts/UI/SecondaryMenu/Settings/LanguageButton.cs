using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MVC.App.UI.SecondaryMenu.Settings
{
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

        public void SetState(bool _isOn)
        {
            if (_isOn) display.sprite = toggleOn;
            else display.sprite = toggleOff;
        }
    }
}

