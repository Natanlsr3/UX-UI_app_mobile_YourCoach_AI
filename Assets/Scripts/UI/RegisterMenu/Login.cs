using MVC.App.UI.RegisterMenu.RegisterPopUp;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MVC.App.UI.RegisterMenu
{
    public class Login : RegisterMenu
    {
        [Header("Screens")]
        [SerializeField] private GameObject forgetPasswordScreen;
        [SerializeField] private Button forgetPasswordButton;

        [SerializeField] private GameObject createAccountScreen;
        [SerializeField] private Button createAccountButton;

        [Header("UserLogin")]
        [SerializeField] private Button loginButton;

        private const string IS_ACTIVE = "IsActive";

        protected override void Start()
        {
            base.Start();

            // Connect buttons
            loginButton.onClick.AddListener(ConnectUser);
            forgetPasswordButton.onClick.AddListener(SetForgetPasswordWindow);
            createAccountButton.onClick.AddListener(SetCreateAccountWindow);
        }

        /// <summary>
        /// Check user validity and connect him to the application
        /// </summary>
        private void ConnectUser()
        {
            if (LogSession.Instance.ValidUser(m_TypedUsername, m_TypedPassword))
            {
                // Check if the user is a new user and send it to the right menu depending on it
                if (LogSession.Instance.CurrentUser.FreshAccount) SceneManager.LoadScene("NewUser");
                else SceneManager.LoadScene("Main");
            }
            else DisplayError(true);
        }

        /// <summary>
        /// Create and display the forget password pop-up window
        /// </summary>
        private void SetForgetPasswordWindow()
        {
            DisplayError(false);
            ResetTextField();
            m_Anim.SetBool(IS_ACTIVE, false);

            ForgetPassword window = Instantiate(forgetPasswordScreen, transform.parent).GetComponent<ForgetPassword>();
            window.SetTitleText("Forgot Password");
            window.OnLeftPopUp += DisplayLogin;
        }

        /// <summary>
        /// Create and display the create account pop-up window
        /// </summary>
        private void SetCreateAccountWindow()
        {
            DisplayError(false);
            ResetTextField();
            m_Anim.SetBool(IS_ACTIVE, false);

            CreateAccount window = Instantiate(createAccountScreen, transform.parent).GetComponent<CreateAccount>();
            window.OnLeftPopUp += DisplayLogin;
        }

        /// <summary>
        /// Animate the display of the login menu
        /// </summary>
        private void DisplayLogin() { m_Anim.SetBool(IS_ACTIVE, true); }
    }
}

