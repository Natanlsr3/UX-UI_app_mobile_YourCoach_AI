using System;
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
            public int SessionDoneNumber; // Session done
            public int SessionMaxNumber; // Goal number
        }
        
        public Session[] Sessions = new Session[3]; // 3 = day | week | month

        private const string DONE_TEXT = " done successfuly";

        void Start()
        {
            Statistics.Instance.OnTimePeriodChanged += UpdateDisplay;
        }

        /// <summary>
        /// Display number of session done compared to goal number, based on selected period
        /// </summary>
        private void UpdateDisplay()
        {
            int _index = (int)Statistics.Instance.SelectedPeriod;

            // Set progress bar value based on session done and session goal number
            float _sessionDoneNumber = Sessions[_index].SessionDoneNumber;
            float _sessionMaxNumber = Sessions[_index].SessionMaxNumber;
            progressDisplay.fillAmount = _sessionDoneNumber / _sessionMaxNumber;

            string _sessionText = " session";
            if (_sessionDoneNumber > 1) _sessionText += "s";
            sessionNumDisplay.text = _sessionDoneNumber + _sessionText + DONE_TEXT;
        }
    }
}

