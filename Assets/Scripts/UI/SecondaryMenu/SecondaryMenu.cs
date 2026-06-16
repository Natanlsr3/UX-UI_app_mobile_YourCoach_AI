using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MVC.App.UI.SecondaryMenu
{
    public class SecondaryMenu : MonoBehaviour
    {
        [SerializeField] protected Button m_BackButton;

        protected virtual void Start()
        {
            // Connect button
            m_BackButton.onClick.AddListener(BackToMainMenu);
        }

        /// <summary>
        /// Return to main menu
        /// </summary>
        protected virtual void BackToMainMenu()
        {
            SceneManager.LoadScene("Main");
        }
    }
}
