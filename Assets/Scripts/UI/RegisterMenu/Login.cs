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
        [SerializeField] private string username;
        [SerializeField] private string password;

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
            if (m_TypedUsername == username && m_TypedPassword == password) SceneManager.LoadScene("Main");
            else DisplayError(true);
        }

        private void SetForgetPasswordWindow()
        {
            DisplayError(false);
            ResetTextField();
            m_Anim.SetBool(IS_ACTIVE, false);

            ForgetPassword window = Instantiate(forgetPasswordScreen, transform.parent).GetComponent<ForgetPassword>();
            window.SetUserInfo(username);
            window.OnPasswordReset += ChangePassword;
            window.OnLeftPopUp += DisplayLogin;
        }

        // To change when BDD added
        private void ChangePassword(string _newPassword)
        {
            password = _newPassword;
        }

        private void SetCreateAccountWindow()
        {
            DisplayError(false);
            ResetTextField();
            m_Anim.SetBool(IS_ACTIVE, false);

            CreateAccount window = Instantiate(createAccountScreen, transform.parent).GetComponent<CreateAccount>();
            window.SetUserInfo(username);
            window.OnAccountCreated += ChangeUser;
            window.OnLeftPopUp += DisplayLogin;
        }

        // To change when BDD added
        private void ChangeUser(string _newUsername, string _newPassword)
        {
            username = _newUsername;
            password = _newPassword;
        }

        private void DisplayLogin() { m_Anim.SetBool(IS_ACTIVE, true); }
    }
}

