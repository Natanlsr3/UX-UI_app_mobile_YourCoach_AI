using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace MVC.App.UI.SecondaryMenu.Training
{
    public class DayDisplay : MonoBehaviour
    {
        [SerializeField] private Image frame;
        [SerializeField] private Image indicator;
        [SerializeField] private Image buttonFrame;

        private Button dayButton;
        public TextMeshProUGUI DayText;

        private DateTime date;
        public DateTime Date { get => date; }

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
            Calendar.Instance.SelectedDate = date;
            OnDayClick?.Invoke(this);
        }

        private void SetButtonFrame(DayDisplay _selectedDay)
        {
            if (_selectedDay == this) buttonFrame.color = Color.black;
            else buttonFrame.color = Color.clear;
        }

        public void SetDate(DateTime _date)
        {
            date = _date;
            if (LogSession.Instance.CheckWorkoutDate(_date)) indicator.color = Color.white;
            else indicator.color = Color.clear;
        }

        public void SetDayState(Calendar.Day.DayState _state)
        {
            if (_state == Calendar.Day.DayState.Current)
            {
                frame.color = Color.white;
                DayText.color = Color.white;
            }
            else
            {
                frame.color = Color.clear;
                DayText.color = Color.black;
            }
        }
    }
}

