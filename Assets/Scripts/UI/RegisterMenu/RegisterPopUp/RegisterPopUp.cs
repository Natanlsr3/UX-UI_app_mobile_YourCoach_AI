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
        [SerializeField] protected Button m_LeavePopUpButton;

        protected string m_Username;

        protected const string HIDE = "Hide";

        protected override void Start()
        {
            base.Start();
            m_LeavePopUpButton.onClick.AddListener(LeavePopUp);
        }
        public void SetUserInfo(string _username)
        {
            m_Username = _username;
        }

        protected virtual void LeavePopUp() { }

        public void DestroyWindow() { Destroy(gameObject); }
    }
}

