using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace MVC.App.UI.SecondaryMenu.Settings
{
    public class LanguageMenu : MonoBehaviour
    {
        [SerializeField] private Transform buttonContainer;

        private List<LanguageButton> buttonList = new List<LanguageButton>();
        private int buttonNum;

        void Start()
        {
            int _buttonNum = buttonContainer.childCount;
            for (int i = 0; i < _buttonNum; i++)
            {
                int _index = i;
                LanguageButton _button = buttonContainer.GetChild(i).GetComponent<LanguageButton>();
                _button.Button.onClick.AddListener(delegate{ SetLanguage(_index); });
                buttonList.Add(_button);
            }

            buttonNum = buttonList.Count;

            SetLanguage((int)UserSettings.Instance.CurrentLanguage);
        }

        private void SetLanguage(int _index)
        {
            // Update button display
            for (int i = 0; i < buttonNum; i++)
            {
                buttonList[i].SetState(false);
            }
            buttonList[_index].SetState(true);

            UserSettings.Instance.CurrentLanguage = (UserSettings.Language)_index;
            UserSettings.Instance.LoadLanguage();
        }
    }
}

