using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace MVC.App.UI.RegisterMenu.RegisterPopUp
{
    public class CreateAccount : RegisterPopUp
    {
        // New account username and password

        public event Action<string, string> OnAccountCreated;

        protected override void Start()
        {
            base.Start();
        }

        private void CreateNewAccount()
        {
            if (m_TypedUsername != m_Username)
            {
                DisplayError(false);
                OnAccountCreated?.Invoke(m_TypedUsername, m_TypedPassword);
                m_Anim.SetTrigger(HIDE);
            }
            else DisplayError(true);
        }

        protected override void Confirm()
        {
            base.Confirm();
            CreateNewAccount();
        }
    }
}

