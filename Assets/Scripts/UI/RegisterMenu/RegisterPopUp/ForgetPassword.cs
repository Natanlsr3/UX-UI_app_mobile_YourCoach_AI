using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MVC.App.UI.RegisterMenu.RegisterPopUp
{
    public class ForgetPassword : RegisterPopUp
    {
        public event Action<string> OnPasswordReset;

        private void ResetPassword()
        {
            if (m_TypedUsername == m_Username)
            {
                DisplayError(false);
                OnPasswordReset?.Invoke(m_TypedPassword);
                m_Anim.SetTrigger(HIDE);
            }
            else DisplayError(true);
        }

        protected override void Confirm()
        {
            base.Confirm();
            ResetPassword();
        }
    }

}
