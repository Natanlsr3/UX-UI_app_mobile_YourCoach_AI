using System;
using UnityEngine;
using UnityEngine.UI;

namespace MVC.App.UI.RegisterMenu.RegisterPopUp
{
    public class RegisterPopUp : RegisterMenu
    {
        [Header("Pop-Up")]
        [SerializeField] protected Button m_ConfirmButton;
        [SerializeField] protected Button m_LeavePopUpButton;

        protected const string HIDE = "Hide";

        public event Action OnLeftPopUp;

        protected override void Start()
        {
            base.Start();

            // Connect buttons
            m_ConfirmButton.onClick.AddListener(Confirm);
            m_LeavePopUpButton.onClick.AddListener(LeavePopUp);
        }

        /// <summary>
        /// Validate the registered information
        /// </summary>
        protected virtual void Confirm() { }

        /// <summary>
        /// Quit the pop-up window and play leave animation
        /// </summary>
        protected virtual void LeavePopUp()
        {
            DisplayError(false);
            m_Anim.SetTrigger(HIDE);
            OnLeftPopUp?.Invoke();
        }

        /// <summary>
        /// Destroy the pop-up window
        /// </summary>
        public void DestroyWindow() { Destroy(gameObject); }
    }
}

