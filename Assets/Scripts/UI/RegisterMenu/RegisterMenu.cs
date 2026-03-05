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

        protected TMP_Text m_UsernamePlaceholder;
        protected TMP_Text m_PasswordPlaceholder;

        protected Animator m_Anim;

        protected string m_DefaultUsernameText;
        protected string m_DefaultPasswordText;

        protected string m_TypedUsername;
        protected string m_TypedPassword;

        protected bool m_IsPasswordHidden = true;

        protected const string NULL_TEXT = "";

        protected virtual void Start()
        {
            m_Anim = GetComponent<Animator>();

            // Setup input field's placeholder text reference and default value
            m_UsernamePlaceholder = m_UsernameField.placeholder.GetComponent<TMP_Text>();
            m_PasswordPlaceholder = m_PasswordField.placeholder.GetComponent<TMP_Text>();
            m_DefaultUsernameText = m_UsernamePlaceholder.text;
            m_DefaultPasswordText = m_PasswordPlaceholder.text;

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

        protected void ResetTextField()
        {
            // Reset content
            m_UsernameField.text = NULL_TEXT;
            m_PasswordField.text = NULL_TEXT;

            // Reset placeholder
            m_UsernamePlaceholder.text = m_DefaultUsernameText;
            m_PasswordPlaceholder.text = m_DefaultPasswordText;
        }
    }
}

