using System;
using UnityEngine;

namespace MVC.App.UI.SecondaryMenu.Stats
{
    public class CalorieMenu : MonoBehaviour
    {
        [SerializeField] private CalorieStat todayCalories;
        [SerializeField] private Transform weekCaloriesContainer;
        [SerializeField] private Transform monthCaloriesContainer;

        [SerializeField] private float caloriesBurntToday;
        [SerializeField] private float[] caloriesBurntPerDay = new float[7];
        [SerializeField] private float[] caloriesBurntPerWeek = new float[5];

        void Start()
        {
            SetDayCalories();
            SetWeekCalories();
            SetMonthCalories();

            Statistics.Instance.OnTimePeriodChanged += UpdateDisplay;
        }

        /// <summary>
        /// Set the value of the day calories display
        /// </summary>
        private void SetDayCalories()
        {
            todayCalories.SetValue(caloriesBurntToday, 0.5f, true);
        }

        /// <summary>
        /// Set the value of the week calories display for each day
        /// </summary>
        private void SetWeekCalories()
        {
            // Get current day of the week
            int _currentDay;

            //Correct the day offset
            if (DateTime.Today.DayOfWeek == DayOfWeek.Sunday) _currentDay = 6;
            else _currentDay = (int)DateTime.Today.DayOfWeek - 1;

            // Calculate total calories burnt during the week
            float _totalCaloriesBurnt = 0f;
            int _daysNum = caloriesBurntPerDay.Length;
            for (int i = 0; i < _daysNum; i++)
            {
                _totalCaloriesBurnt += caloriesBurntPerDay[i];
            }

            // Display the stats with the right ratio between them
            CalorieStat _stat;
            float _ratio;
            bool _isCurrentDay;
            for (int i = 0; i < _daysNum; i++)
            {
                // Get if index is the current day
                if (i == _currentDay) _isCurrentDay = true;
                else _isCurrentDay = false;

                _ratio = caloriesBurntPerDay[i] / _totalCaloriesBurnt;
                _stat = weekCaloriesContainer.GetChild(i).GetComponent<CalorieStat>();
                _stat.SetValue(caloriesBurntPerDay[i], _ratio, _isCurrentDay);
            }
        }

        /// <summary>
        /// Set the value of the month calories display for each week
        /// </summary>
        private void SetMonthCalories()
        {
            // Get current week of the month
            DateTime _firstDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            int _firstDayOffset = (int)_firstDate.DayOfWeek;
            if (_firstDate.DayOfWeek == DayOfWeek.Sunday) _firstDayOffset = 7;
            int _currentWeek = (int)MathF.Ceiling((DateTime.Now.Day + _firstDayOffset - 1) / 7f) - 1;

            // Calculate total calories burnt during the month
            float _totalCaloriesBurnt = 0f;
            int _weeksNum = caloriesBurntPerWeek.Length;
            for (int i = 0; i < _weeksNum; i++)
            {
                _totalCaloriesBurnt += caloriesBurntPerWeek[i];
            }

            // Display the stats with the right ratio between them
            CalorieStat _stat;
            float _ratio;
            bool _isCurrentWeek;
            for (int i = 0; i < _weeksNum; i++)
            {
                // Get if index is the current week
                if (i == _currentWeek) _isCurrentWeek = true;
                else _isCurrentWeek = false;

                _ratio = caloriesBurntPerWeek[i] / _totalCaloriesBurnt;
                _stat = monthCaloriesContainer.GetChild(i).GetComponent<CalorieStat>();
                _stat.SetValue(caloriesBurntPerWeek[i], _ratio, _isCurrentWeek);
            }
        }

        /// <summary>
        /// Display the right calories display depending on current period selected
        /// </summary>
        private void UpdateDisplay()
        {
            // Hide all calories display
            todayCalories.gameObject.SetActive(false);
            weekCaloriesContainer.gameObject.SetActive(false);
            monthCaloriesContainer.gameObject.SetActive(false);

            // Display only the ones for the selected period
            switch (Statistics.Instance.SelectedPeriod)
            {
                case Statistics.TimePeriod.Day:
                    todayCalories.gameObject.SetActive(true);
                    break;
                case Statistics.TimePeriod.Week:
                    weekCaloriesContainer.gameObject.SetActive(true);
                    break;
                case Statistics.TimePeriod.Month:
                    monthCaloriesContainer.gameObject.SetActive(true);
                    break;
            }
        }
    }
}

