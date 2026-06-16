using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MVC.App.UI.MainMenu
{
    public class MenuSelection : MonoBehaviour
    {
        [SerializeField] private Button moreButton;
        [SerializeField] private Transform buttonContainer;

        // List of the different scenes for secondary menus
        private List<string> secondaryMenus = new List<string>() { "Profile", "Statistics", "Challenges", "Training", "Settings" };

        private Animator anim;
        private const string IS_DISPLAYED = "IsDisplayed";

        private bool buttonDisplayed;

        void Start()
        {
            anim = GetComponent<Animator>();

            moreButton.onClick.AddListener(DisplayButtons);

            // Connect each menu button to send to their respective scene
            int _buttonNum = buttonContainer.childCount;
            for (int i = 0; i < _buttonNum; i++)
            {
                int _menuIndex = i;
                Button _button = buttonContainer.GetChild(i).GetComponent<Button>();
                _button.onClick.AddListener(delegate { GoToMenu(secondaryMenus[_menuIndex]); });
            }
        }

        /// <summary>
        /// Switch display for menu buttons
        /// </summary>
        private void DisplayButtons()
        {
            buttonDisplayed = !buttonDisplayed;

            // Animate the display switch
            anim.SetBool(IS_DISPLAYED, buttonDisplayed);
        }

        /// <summary>
        /// Go to the given menu
        /// </summary>
        /// <param name="_menu"></param>
        private void GoToMenu(string _menu)
        {
            SceneManager.LoadScene(_menu);
        }
    }
}

