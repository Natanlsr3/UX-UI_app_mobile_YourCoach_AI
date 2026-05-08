using System;
using System.Collections;
using System.Collections.Generic;
using MVC.App.UI.RegisterMenu.RegisterPopUp;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
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

            loginButton.onClick.AddListener(ConnectUser);
            forgetPasswordButton.onClick.AddListener(SetForgetPasswordWindow);
            createAccountButton.onClick.AddListener(SetCreateAccountWindow);
        }

        private void ConnectUser()
        {
            if (LogSession.Instance.ValidUser(m_TypedUsername, m_TypedPassword))
            {
                if (LogSession.Instance.CurrentUser.FreshAccount) SceneManager.LoadScene("NewUser");
                else SceneManager.LoadScene("Main");
            }
            else DisplayError(true);
        }

        private void SetForgetPasswordWindow()
        {
            DisplayError(false);
            ResetTextField();
            m_Anim.SetBool(IS_ACTIVE, false);

            ForgetPassword window = Instantiate(forgetPasswordScreen, transform.parent).GetComponent<ForgetPassword>();
            window.SetTitleText("Forgot Password");
            window.OnLeftPopUp += DisplayLogin;
        }

        private void SetCreateAccountWindow()
        {
            DisplayError(false);
            ResetTextField();
            m_Anim.SetBool(IS_ACTIVE, false);

            CreateAccount window = Instantiate(createAccountScreen, transform.parent).GetComponent<CreateAccount>();
            window.OnLeftPopUp += DisplayLogin;
        }

        private void DisplayLogin() { m_Anim.SetBool(IS_ACTIVE, true); }
    }
}

