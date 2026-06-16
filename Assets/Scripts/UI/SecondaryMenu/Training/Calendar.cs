using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

namespace MVC.App.UI.SecondaryMenu.Training
{
    public class Calendar : MonoBehaviour
    {
        /// <summary>
        /// Cell or slot in the calendar. All the information each day should now about itself
        /// </summary>
        public class Day
        {
            public int DayNum;
            public enum DayState { InMonth, OutMonth, Current }
            public DayState State;
            public DayDisplay Display;

            /// <summary>
            /// Constructor of Day
            /// </summary>
            public Day(DateTime _date, DayState _dayState, DayDisplay _display)
            {
                DayNum = _date.Day;
                Display = _display;
                UpdateState(_dayState);
                UpdateDay(_date);
            }

            /// <summary>
            /// Update the state of the day cell and update its display
            /// </summary>
            public void UpdateState(DayState _newState)
            {
                State = _newState;
                Display.SetDayState(_newState);
            }

            /// <summary>
            /// Update day number displayed based on day state.
            /// Should be called after UpdateState() method
            /// </summary>
            public void UpdateDay(DateTime _date)
            {
                DayNum = _date.Day;
                if (State == DayState.InMonth || State == DayState.Current)
                {
                    Display.DayText.text = DayNum.ToString();
                }
                else
                {
                    Display.DayText.text = "";
                }
                Display.SetDate(_date);
            }
        }

        // All the days in the month
        private List<Day> days = new List<Day>();

        // All the weeks in the month (already setup in editor)
        [SerializeField] private Transform[] weeks;

        [SerializeField] private TextMeshProUGUI MonthAndYear;

        // Date the calendar is currently on.
        // The year and month are based on the calendar, while the day itself is almost always just 1 (because not needed)
        private DateTime currentDate = DateTime.Now;

        // Date selected by the user when clicking on a day button
        public DateTime SelectedDate;

        private const int WEEK_NUM = 5;
        private const int DAY_NUM = 7;

        public event Action OnMonthSwitch;

        #region Singleton

        private static Calendar instance;
        public static Calendar Instance { get => instance; }

        private void Awake()
        {
            if (instance != null)
            {
                print("Another Instance already exist !");
                Destroy(this);
                return;
            }
            instance = this;
        }

        #endregion

        private void Start()
        {
            // Set calendar to current date
            InitCalendar(DateTime.Now.Year, DateTime.Now.Month);
        }

        /// <summary>
        /// Create all days and fill the calendar with them
        /// </summary>
        /// <param name="_year"></param>
        /// <param name="_month"></param>
        private void InitCalendar(int _year, int _month)
        {
            DateTime _date = new DateTime(_year, _month, 1);
            currentDate = _date;
            MonthAndYear.text = _date.ToString("MMMM") + " " + _date.Year;
            int _startDay = GetMonthStartDay(_year, _month);
            int _endDay = GetTotalNumberOfDays(_year, _month);

            for (int i = 0; i < WEEK_NUM; i++)
            {
                for (int j = 0; j < DAY_NUM; j++)
                {
                    Day _newDay;
                    int _currentDay = (i * 7) + j;
                    if (_currentDay < _startDay || _currentDay - _startDay >= _endDay)
                    {
                        _newDay = new Day(new DateTime(_date.Year, _date.Month, Mathf.Clamp(_currentDay - _startDay + 1, 1, _endDay)), Day.DayState.OutMonth, weeks[i].GetChild(j).GetComponent<DayDisplay>());
                    }
                    else
                    {
                        _newDay = new Day(new DateTime(_date.Year, _date.Month, Mathf.Clamp(_currentDay - _startDay + 1, 1, _endDay)), Day.DayState.InMonth, weeks[i].GetChild(j).GetComponent<DayDisplay>());
                    }
                    days.Add(_newDay);
                }
            }

            //Change the state and display of the current day if on the calendar
            if (DateTime.Now.Year == _year && DateTime.Now.Month == _month)
            {
                days[(DateTime.Now.Day - 1) + _startDay].UpdateState(Day.DayState.Current);
            }
        }

        /// <summary>
        /// Update days, month and year of calendar based on year and month
        /// </summary>
        void UpdateCalendar(int _year, int _month)
        {
            DateTime _date = new DateTime(_year, _month, 1);
            currentDate = _date;
            MonthAndYear.text = _date.ToString("MMMM") + " " + _date.Year;
            int _startDay = GetMonthStartDay(_year, _month);
            int _endDay = GetTotalNumberOfDays(_year, _month);

            // Update the state and information of days in calendar
            for (int i = 0; i < WEEK_NUM * DAY_NUM; i++)
            {
                if (i < _startDay || i - _startDay >= _endDay)
                {
                    days[i].UpdateState(Day.DayState.OutMonth);
                }
                else
                {
                    days[i].UpdateState(Day.DayState.InMonth);
                }
                days[i].UpdateDay(new DateTime(_date.Year, _date.Month, Mathf.Clamp(i - _startDay + 1, 1, _endDay)));
            }

            //Change the state and display of the current day if on the calendar
            if (DateTime.Now.Year == _year && DateTime.Now.Month == _month)
            {
                days[(DateTime.Now.Day - 1) + _startDay].UpdateState(Day.DayState.Current);
            }

        }

        /// <summary>
        /// Get the day of week the month is starting on
        /// </summary>
        int GetMonthStartDay(int _year, int _month)
        {
            DateTime _date = new DateTime(_year, _month, 1);

            //Correct the day offset 
            int _dayOfWeek = (int)_date.DayOfWeek;
            if (_dayOfWeek == DAY_NUM - 1) _dayOfWeek = 0;
            else if (_dayOfWeek == 0) _dayOfWeek = DAY_NUM - 1;
            else _dayOfWeek -= 1;

            //DayOfWeek : Monday == 0, Sunday == 6 etc
            return _dayOfWeek;
        }

        /// <summary>
        /// Gets the number of days in the given month.
        /// </summary>
        int GetTotalNumberOfDays(int _year, int _month)
        {
            return DateTime.DaysInMonth(_year, _month);
        }

        /// <summary>
        /// Set the calendar to previous or next month depending on direction.
        /// Connected to calendar arrows
        /// </summary>
        public void SwitchMonth(int _direction)
        {
            currentDate = currentDate.AddMonths(MathF.Sign(_direction));
            UpdateCalendar(currentDate.Year, currentDate.Month);
            OnMonthSwitch?.Invoke();
        }

        private void OnDestroy()
        {
            instance = null;
        }
    }
}

