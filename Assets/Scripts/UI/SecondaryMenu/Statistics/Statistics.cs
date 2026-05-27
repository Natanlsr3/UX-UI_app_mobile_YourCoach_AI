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
        [SerializeField] private Color disableColor;
        [SerializeField] private Color enableColor;

        [SerializeField] private Transform buttonsContainer;
        private List<Button> buttonsList = new List<Button>();

        public enum TimePeriod { Day, Week, Month }

        private TimePeriod selectedPeriod;
        public TimePeriod SelectedPeriod { get => selectedPeriod; }

        public event Action OnTimePeriodChanged;

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

        protected override void Start()
        {
            base.Start();

            Button _button;
            int _buttonsNum = buttonsContainer.childCount;
            for (int i = 0; i < _buttonsNum; i++)
            {
                int _index = i;
                _button = buttonsContainer.GetChild(i).GetComponent<Button>();
                _button.onClick.AddListener(delegate{SwitchPeriod(_index);});
                buttonsList.Add(_button);
            }

            SwitchPeriod(0);
        }

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
