using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace MVC.App.UI.SecondaryMenu.Settings
{
    public class NotificationMenu : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown notificationFrom;
        [SerializeField] private TMP_Dropdown notificationTo;

        private const int MIN_INDEX = 1;
        private const int MAX_INDEX = 11;

        void Start()
        {
            notificationFrom.ClearOptions();
            notificationTo.ClearOptions();

            List<string> _notificationList = new List<string>();
            _notificationList.Add("12:00 AM");
            for (int i = MIN_INDEX; i <= MAX_INDEX; i++)
            {
                string _text = i.ToString("00") + ":00 AM";
                _notificationList.Add(_text);
            }
            _notificationList.Add("12:00 PM");
            for (int i = MIN_INDEX; i <= MAX_INDEX; i++)
            {
                string _text = i.ToString("00") + ":00 PM";
                _notificationList.Add(_text);
            }

            notificationFrom.AddOptions(_notificationList);
            notificationTo.AddOptions(_notificationList);
        }
    }
}

