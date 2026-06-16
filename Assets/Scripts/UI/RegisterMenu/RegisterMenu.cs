using TMPro;
using UnityEngine;
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

        // Default text of username and password text field
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

        /// <summary>
        /// Update username with given username.
        /// Bound to username input field
        /// </summary>
        /// <param name="_username"></param>
        protected void UpdateUsername(string _username)
        {
            m_TypedUsername = _username;
        }

        /// <summary>
        /// Update password with given password.
        /// Bound to password input field
        /// </summary>
        /// <param name="_password"></param>
        protected void UpdatePassword(string _password)
        {
            m_TypedPassword = _password;
        }

        /// <summary>
        /// Switch the password display (displayed or hidden).
        /// Bound to password display button
        /// </summary>
        public virtual void SetPasswordDisplay()
        {
            m_IsPasswordHidden = !m_IsPasswordHidden;

            if (m_IsPasswordHidden)
            {
                // Change input field type to password and update it
                m_PasswordField.contentType = TMP_InputField.ContentType.Password;
                m_PasswordField.ForceLabelUpdate();

                m_PasswordDisplay.sprite = m_EyeCloseImage;
            }
            else
            {
                // Change input field type to standard and update it
                m_PasswordField.contentType = TMP_InputField.ContentType.Standard;
                m_PasswordField.ForceLabelUpdate();

                m_PasswordDisplay.sprite = m_EyeOpenImage;
            }
        }

        /// <summary>
        /// Display or hide error panel
        /// </summary>
        /// <param name="_isVisible"></param>
        protected virtual void DisplayError(bool _isVisible)
        {
            if (m_ErrorPanel.activeInHierarchy != _isVisible) m_ErrorPanel.SetActive(_isVisible);
        }

        /// <summary>
        /// Set username and password text field to default value
        /// </summary>
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

