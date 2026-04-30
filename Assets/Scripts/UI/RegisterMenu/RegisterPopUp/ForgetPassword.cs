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
            if (LogSession.Instance.ExistingUser(m_TypedUsername) && m_TypedPassword != "")
            {
                OnPasswordReset?.Invoke(m_TypedPassword);

                LogSession.User _user = LogSession.Instance.FindUser(m_TypedUsername);
                _user.Password = m_TypedPassword;
                LogSession.Instance.UserList[LogSession.Instance.FindUserIndex(m_TypedUsername)] = _user;
                LeavePopUp();
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
