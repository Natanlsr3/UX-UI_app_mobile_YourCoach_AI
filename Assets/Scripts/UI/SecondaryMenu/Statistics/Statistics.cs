using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MVC.App.UI.SecondaryMenu.Stats
{
    public class Statistics : SecondaryMenu
    {
        // Colors for the period buttons
        [SerializeField] private Color disableColor;
        [SerializeField] private Color enableColor;

        // List of period buttons
        [SerializeField] private Transform buttonsContainer;
        private List<Button> buttonsList = new List<Button>();

        // Time period to base the stats on : day, week or month
        public enum TimePeriod { Day, Week, Month }

        private TimePeriod selectedPeriod;
        public TimePeriod SelectedPeriod { get => selectedPeriod; }

        public event Action OnTimePeriodChanged;

        #region Singleton

        private static Statistics instance;
        public static Statistics Instance { get => instance; }

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

        protected override void Start()
        {
            base.Start();

            // Get all period buttons and connect to them
            Button _button;
            int _buttonsNum = buttonsContainer.childCount;
            for (int i = 0; i < _buttonsNum; i++)
            {
                int _index = i;
                _button = buttonsContainer.GetChild(i).GetComponent<Button>();
                _button.onClick.AddListener(delegate{SwitchPeriod(_index);});
                buttonsList.Add(_button);
            }

            // Set default time period : 0 = day
            SwitchPeriod(0);
        }

        /// <summary>
        /// Set the new selected time period and update the display of period buttons
        /// </summary>
        /// <param name="_periodIndex"></param>
        private void SwitchPeriod(int _periodIndex)
        {
            selectedPeriod = (TimePeriod)_periodIndex;

            // Update buttons display
            foreach (Button _button in buttonsList)
            {
                _button.GetComponentInChildren<TextMeshProUGUI>().color = disableColor;
            }
            buttonsList[_periodIndex].GetComponentInChildren<TextMeshProUGUI>().color = enableColor;

            OnTimePeriodChanged?.Invoke();
        }

        void OnDestroy()
        {
            instance = null;
        }
    }
}
