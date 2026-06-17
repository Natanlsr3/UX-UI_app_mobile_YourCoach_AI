using System;

namespace MVC.App.UI.RegisterMenu.RegisterPopUp
{
    public class CreateAccount : RegisterPopUp
    {
        // Send new account username and password
        public event Action<string, string> OnAccountCreated;

        /// <summary>
        /// Create a new account with registered username and password
        /// </summary>
        private void CreateNewAccount()
        {
            // Check if the fields are filled and if there is a user with this username
            if (!LogSession.Instance.ExistingUser(m_TypedUsername) && m_TypedUsername != "" && m_TypedPassword != "")
            {
                OnAccountCreated?.Invoke(m_TypedUsername, m_TypedPassword);
                LogSession.Instance.CreateUser(m_TypedUsername, m_TypedPassword);
                LeavePopUp();
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

