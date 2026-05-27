using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MVC.App.UI.SecondaryMenu.Stats
{
    public class RepSetMenu : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown choice;
        [SerializeField] private TextMeshProUGUI choiceDisplay;

        [SerializeField] private Image progressDisplay;
        [SerializeField] private TextMeshProUGUI progressNumDisplay;

        [Serializable]
        public struct RepSet
        {
            public int RepNumber;
            public int RepMaxNumber;

            public int SetNumber;
            public int SetMaxNumber;
        }

        public RepSet[] RepsSets = new RepSet[3];

        private const string REPS = "Reps";
        private const string SETS = "Sets";

        private int timePeriodIndex;
        private int choiceIndex;

        void Start()
        {
            choice.onValueChanged.AddListener(SwitchChoice);
            Statistics.Instance.OnTimePeriodChanged += UpdateDisplay;
        }

        private void SwitchChoice(int _choice)
        {
            choiceIndex = _choice;
            if (_choice == 0)
            {
                choiceDisplay.text = REPS;
                float _repNumber = RepsSets[timePeriodIndex].RepNumber;
                float _repMaxNumber = RepsSets[timePeriodIndex].RepMaxNumber;
                progressDisplay.fillAmount = _repNumber / _repMaxNumber;
                progressNumDisplay.text = _repNumber + "/" + _repMaxNumber;
            }
            else
            {
                choiceDisplay.text = SETS;
                float _setNumber = RepsSets[timePeriodIndex].SetNumber;
                float _setMaxNumber = RepsSets[timePeriodIndex].SetMaxNumber;
                progressDisplay.fillAmount = _setNumber / _setMaxNumber;
                progressNumDisplay.text = _setNumber + "/" + _setMaxNumber;
            }
        }

        private void UpdateDisplay()
        {
            timePeriodIndex = (int)Statistics.Instance.SelectedPeriod;
            SwitchChoice(choiceIndex);
        }
    }
}
