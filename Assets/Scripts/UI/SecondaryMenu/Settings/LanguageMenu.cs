using System.Collections.Generic;
using UnityEngine;

namespace MVC.App.UI.SecondaryMenu.Settings
{
    public class LanguageMenu : MonoBehaviour
    {
        [SerializeField] private Transform buttonContainer;

        private List<LanguageButton> buttonList = new List<LanguageButton>();
        private int buttonNum;

        void Start()
        {
            // Assign to each button the changement of their corresponding language
            int _buttonNum = buttonContainer.childCount;
            for (int i = 0; i < _buttonNum; i++)
            {
                int _index = i;
                LanguageButton _button = buttonContainer.GetChild(i).GetComponent<LanguageButton>();
                _button.Button.onClick.AddListener(delegate{ SetLanguage(_index); });
                buttonList.Add(_button);
            }

            buttonNum = buttonList.Count;

            // Change the language with the default language
            SetLanguage((int)UserSettings.Instance.CurrentLanguage);
        }

        /// <summary>
        /// Change the language used in the application using a language index
        /// </summary>
        /// <param name="_index"></param> The index of the selected language in the Language enum of the UserSettings
        private void SetLanguage(int _index)
        {
            // Update button display
            for (int i = 0; i < buttonNum; i++)
            {
                buttonList[i].SetState(false);
            }
            buttonList[_index].SetState(true);

            // Set and load new language
            UserSettings.Instance.CurrentLanguage = (UserSettings.Language)_index;
            UserSettings.Instance.LoadLanguage();
        }
    }
}

