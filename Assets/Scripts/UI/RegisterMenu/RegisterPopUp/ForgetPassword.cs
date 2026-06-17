using System;
using TMPro;
using UnityEngine;

namespace MVC.App.UI.RegisterMenu.RegisterPopUp
{
    public class ForgetPassword : RegisterPopUp
    {
        [SerializeField] private TextMeshProUGUI titleText;

        // Send new password
        public event Action<string> OnPasswordReset;

        /// <summary>
        /// Change the user password with the registered one
        /// </summary>
        private void ResetPassword()
        {
            // Check if the field is filled and if there is a user with this username
            if (LogSession.Instance.ExistingUser(m_TypedUsername) && m_TypedPassword != "")
            {
                OnPasswordReset?.Invoke(m_TypedPassword);

                // Find the user with the registered username, change his password and update the user list with the new information
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

        /// <summary>
        /// Set the title text display of the pop-up.
        /// It can change depending on if it is for a password reset or password forgotten
        /// </summary>
        /// <param name="_title"></param>
        public void SetTitleText(string _title)
        {
            titleText.text = _title;
        }
    }

}
