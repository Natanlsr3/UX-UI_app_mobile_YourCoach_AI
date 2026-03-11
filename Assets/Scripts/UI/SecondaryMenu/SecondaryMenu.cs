using System.Collections;
using System.Collections.Generic;
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
            m_BackButton.onClick.AddListener(BackToMainMenu);
        }

        protected virtual void BackToMainMenu()
        {
            SceneManager.LoadScene("Main");
        }
    }
}
