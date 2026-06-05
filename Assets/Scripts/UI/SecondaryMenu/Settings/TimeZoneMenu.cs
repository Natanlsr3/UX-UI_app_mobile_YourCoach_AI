using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MVC.App.UI.SecondaryMenu.Settings
{
    public class TimeZoneMenu : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown timeZoneChoice;
        [SerializeField] private Toggle autoDetectToggle;

        private const int MIN_UTC = -12;
        private const int MAX_UTC = 12;

        private const string UTC_START = "UTC";
        private const string UTC_END = ":00";

        void Start()
        {
            // Clear dropdown
            timeZoneChoice.ClearOptions();

            // Add options to dropdown
            List<string> _timeZoneList = new List<string>();
            string _sign;
            string _textDisplay;
            for (int i = MIN_UTC; i <= MAX_UTC; i++)
            {
                _sign = (i < 0) ? "-" : "+";
                _textDisplay = UTC_START + _sign + Mathf.Abs(i).ToString("00") + UTC_END;
                _timeZoneList.Add(_textDisplay);
            }
            timeZoneChoice.AddOptions(_timeZoneList);
        }
    }
}

