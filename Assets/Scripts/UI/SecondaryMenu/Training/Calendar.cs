using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

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
            /// Call this when updating the state so that both the state is updated, as well as the visual display on the screen
            /// </summary>
            public void UpdateState(DayState _newState)
            {
                State = _newState;
                Display.SetDayState(_newState);
            }

            /// <summary>
            /// When updating the day we decide whether we should show the dayNum based on the color of the day
            /// This means the color should always be updated before the day is updated
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

        /// <summary>
        /// All the days in the month. After we make our first calendar we store these days in this list so we do not have to recreate them every time.
        /// </summary>
        private List<Day> days = new List<Day>();

        /// <summary>
        /// Setup in editor since there will always be six weeks. 
        /// </summary>
        [SerializeField] private Transform[] weeks;

        /// <summary>
        /// This is the text object that displays the current month and year
        /// </summary>
        [SerializeField] private TextMeshProUGUI MonthAndYear;

        /// <summary>
        /// this currentDate is the date our Calendar is currently on. The year and month are based on the calendar, 
        /// while the day itself is almost always just 1
        /// If you have some option to select a day in the calendar, you would want the change this objects day value to the last selected day
        /// </summary>
        private DateTime currentDate = DateTime.Now;

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

        /// <summary>
        /// In start we set the Calendar to the current date
        /// </summary>
        private void Start()
        {
            UpdateCalendar(DateTime.Now.Year, DateTime.Now.Month);
        }

        /// <summary>
        /// Anytime the Calendar is changed we call this to make sure we have the right days for the right month/year
        /// </summary>
        void UpdateCalendar(int _year, int _month)
        {
            DateTime _date = new DateTime(_year, _month, 1);
            currentDate = _date;
            MonthAndYear.text = _date.ToString("MMMM") + " " + _date.Year;
            int _startDay = GetMonthStartDay(_year, _month);
            int _endDay = GetTotalNumberOfDays(_year, _month);

            //Create the days
            //This only happens for our first Update Calendar when we have no Day objects therefore we must create them

            if (days.Count == 0)
            {
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
            }
            //loop through days
            //Since we already have the days objects, we can just update them rather than creating new ones
            else
            {
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
            }

            //This just checks if today is on our calendar. If so, we highlight it in green
            if (DateTime.Now.Year == _year && DateTime.Now.Month == _month)
            {
                days[(DateTime.Now.Day - 1) + _startDay].UpdateState(Day.DayState.Current);
            }

        }

        /// <summary>
        /// This returns which day of the week the month is starting on
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
        /// This either adds or subtracts month from our currentDate.
        /// The arrows will use this function to switch to past or future months
        /// </summary>
        public void SwitchMonth(int _direction)
        {
            currentDate = currentDate.AddMonths(_direction);
            UpdateCalendar(currentDate.Year, currentDate.Month);
            OnMonthSwitch?.Invoke();
        }

        private void OnDestroy()
        {
            instance = null;
        }
    }
}

