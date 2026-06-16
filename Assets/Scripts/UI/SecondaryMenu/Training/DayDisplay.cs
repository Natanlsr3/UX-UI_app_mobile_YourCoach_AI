using System;
using TMPro;
using UnityEngine;
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

        // The date of this day display
        private DateTime date;
        public DateTime Date { get => date; }

        // Send a reference of itself to give the sender of the event
        public static event Action<DayDisplay> OnDayClick; 

        void Start()
        {
            dayButton = GetComponent<Button>();
            dayButton.onClick.AddListener(CallButtonClick);

            OnDayClick += SetButtonFrame;
            Calendar.Instance.OnMonthSwitch += delegate { SetButtonFrame(null); };
        }

        /// <summary>
        /// Method called when clicking on this day button.
        /// Set the selected date to this date
        /// </summary>
        private void CallButtonClick()
        {
            Calendar.Instance.SelectedDate = date;
            OnDayClick?.Invoke(this);
        }

        /// <summary>
        /// Update the button frame depending on if this day is selected or not
        /// </summary>
        /// <param name="_selectedDay"></param>
        private void SetButtonFrame(DayDisplay _selectedDay)
        {
            if (_selectedDay == this) buttonFrame.color = Color.black;
            else buttonFrame.color = Color.clear;
        }

        /// <summary>
        /// Set the date of this day and update its display
        /// </summary>
        /// <param name="_date"></param>
        public void SetDate(DateTime _date)
        {
            date = _date;

            // Update the display depending on if there is a session at this date
            if (LogSession.Instance.CheckWorkoutDate(_date)) indicator.color = Color.white;
            else indicator.color = Color.clear;
        }

        /// <summary>
        /// Set the state of this day and update its display based on it
        /// </summary>
        /// <param name="_state"></param>
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

