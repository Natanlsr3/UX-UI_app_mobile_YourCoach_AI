using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace MVC.App.UI.RegisterMenu
{
    public class RegisterMenu : MonoBehaviour
    {
        [Header("Username")]
        [SerializeField] protected TMP_InputField m_UsernameField;

        [Header("Password")]
        [SerializeField] protected TMP_InputField m_PasswordField;
        [SerializeField] protected Button m_PasswordDisplayButton;
        [SerializeField] protected Image m_PasswordDisplay;
        [SerializeField] protected Sprite m_EyeCloseImage;
        [SerializeField] protected Sprite m_EyeOpenImage;

        [Header("Error")]
        [SerializeField] protected GameObject m_ErrorPanel;

        protected Animator m_Anim;

        protected string m_TypedUsername;
        protected string m_TypedPassword;

        protected bool m_IsPasswordHidden = true;

        protected virtual void Start()
        {
            m_Anim = GetComponent<Animator>();

            // Bind to input fields
            m_UsernameField.onValueChanged.AddListener(UpdateUsername);
            m_PasswordField.onValueChanged.AddListener(UpdatePassword);

            m_PasswordDisplayButton.onClick.AddListener(SetPasswordDisplay);

            DisplayError(false);
        }

        // Bound to Username Input Field
        protected void UpdateUsername(string _username)
        {
            m_TypedUsername = _username;
        }

        // Bound to Password Input Field
        protected void UpdatePassword(string _password)
        {
            m_TypedPassword = _password;
        }

        // Bound to Password Display Button
        public virtual void SetPasswordDisplay()
        {
            m_IsPasswordHidden = !m_IsPasswordHidden;

            if (m_IsPasswordHidden)
            {
                m_PasswordField.contentType = TMP_InputField.ContentType.Password;
                m_PasswordField.ForceLabelUpdate();

                m_PasswordDisplay.sprite = m_EyeCloseImage;
            }
            else
            {
                m_PasswordField.contentType = TMP_InputField.ContentType.Standard;
                m_PasswordField.ForceLabelUpdate();

                m_PasswordDisplay.sprite = m_EyeOpenImage;
            }
        }

        protected virtual void DisplayError(bool _isVisible)
        {
            if (m_ErrorPanel.activeInHierarchy != _isVisible) m_ErrorPanel.SetActive(_isVisible);
        }
    }
}

