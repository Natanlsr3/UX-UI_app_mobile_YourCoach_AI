using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MVC.App.UI.SecondaryMenu.Training
{
    public class DayDisplay : MonoBehaviour
    {
        [SerializeField] private Image Frame;
        [SerializeField] private Image Indicator;
        [SerializeField] private Image buttonFrame;

        private Button dayButton;
        public TextMeshProUGUI DayText;

        public static event Action<DayDisplay> OnDayClick; 

        void Start()
        {
            dayButton = GetComponent<Button>();
            dayButton.onClick.AddListener(CallButtonClick);

            OnDayClick += SetButtonFrame;
            Calendar.Instance.OnMonthSwitch += delegate { SetButtonFrame(null); };
        }

        private void CallButtonClick()
        {
            OnDayClick?.Invoke(this);
        }

        private void SetButtonFrame(DayDisplay _selectedDay)
        {
            if (_selectedDay == this) buttonFrame.color = Color.black;
            else buttonFrame.color = Color.clear;
        }

        public void SetDayState(Calendar.Day.DayState _state)
        {
            if (_state == Calendar.Day.DayState.Current)
            {
                Frame.color = Color.white;
                DayText.color = Color.white;
            }
            else
            {
                Frame.color = Color.clear;
                DayText.color = Color.black;
            }
        }
    }
}

