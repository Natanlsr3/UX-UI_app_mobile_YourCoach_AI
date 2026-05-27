using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MVC.App.UI.SecondaryMenu.Stats
{
    public class SessionMenu : MonoBehaviour
    {
        [SerializeField] private Image progressDisplay;
        [SerializeField] private TextMeshProUGUI sessionNumDisplay;

        [Serializable]
        public struct Session
        {
            public int SessionDoneNumber;
            public int SessionMaxNumber;
        }
        
        public Session[] Sessions = new Session[3];

        private const string DONE_TEXT = " done successfuly";

        void Start()
        {
            Statistics.Instance.OnTimePeriodChanged += UpdateDisplay;
        }

        private void UpdateDisplay()
        {
            int _index = (int)Statistics.Instance.SelectedPeriod;

            float _sessionDoneNumber = Sessions[_index].SessionDoneNumber;
            float _sessionMaxNumber = Sessions[_index].SessionMaxNumber;

            progressDisplay.fillAmount = _sessionDoneNumber / _sessionMaxNumber;

            string _sessionText = " session";
            if (_sessionDoneNumber > 1) _sessionText += "s";
            sessionNumDisplay.text = _sessionDoneNumber + _sessionText + DONE_TEXT;
        }
    }
}

