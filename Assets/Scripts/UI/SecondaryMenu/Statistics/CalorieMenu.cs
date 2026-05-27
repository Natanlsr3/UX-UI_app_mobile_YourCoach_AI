using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

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

        private void SetDayCalories()
        {
            todayCalories.SetValue(0.5f, true);
            todayCalories.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = caloriesBurntToday + " kcal";
        }

        private void SetWeekCalories()
        {
            // Get current day of the week
            int currentDay = 0;
            if (DateTime.Today.DayOfWeek == DayOfWeek.Sunday) currentDay = 6;
            else currentDay = (int)DateTime.Today.DayOfWeek - 1;

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
            bool isCurrentDay;

            for (int i = 0; i < _daysNum; i++)
            {
                if (i == currentDay) isCurrentDay = true;
                else isCurrentDay = false;

                _ratio = caloriesBurntPerDay[i] / _totalCaloriesBurnt;
                _stat = weekCaloriesContainer.GetChild(i).GetComponent<CalorieStat>();
                _stat.SetValue(_ratio, isCurrentDay);
            }
        }

        private void SetMonthCalories()
        {
            // Get current week of the month
            DateTime firstDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            int firstDayOffset = (int)firstDate.DayOfWeek;
            if (firstDate.DayOfWeek == DayOfWeek.Sunday) firstDayOffset = 7;
            int currentWeek = (int)MathF.Ceiling((DateTime.Now.Day + firstDayOffset - 1) / 7f) - 1;

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
            bool isCurrentWeek;

            for (int i = 0; i < _weeksNum; i++)
            {
                if (i == currentWeek) isCurrentWeek = true;
                else isCurrentWeek = false;

                _ratio = caloriesBurntPerWeek[i] / _totalCaloriesBurnt;
                _stat = monthCaloriesContainer.GetChild(i).GetComponent<CalorieStat>();
                _stat.SetValue(_ratio, isCurrentWeek);
            }
        }

        private void UpdateDisplay()
        {
            todayCalories.gameObject.SetActive(false);
            weekCaloriesContainer.gameObject.SetActive(false);
            monthCaloriesContainer.gameObject.SetActive(false);

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

