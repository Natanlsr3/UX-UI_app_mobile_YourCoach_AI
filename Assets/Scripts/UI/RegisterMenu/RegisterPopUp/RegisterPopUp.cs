using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
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
            m_ConfirmButton.onClick.AddListener(Confirm);
            m_LeavePopUpButton.onClick.AddListener(LeavePopUp);
        }

        protected virtual void Confirm() { }

        protected virtual void LeavePopUp()
        {
            DisplayError(false);
            m_Anim.SetTrigger(HIDE);
            OnLeftPopUp?.Invoke();
        }

        public void DestroyWindow() { Destroy(gameObject); }
    }
}

