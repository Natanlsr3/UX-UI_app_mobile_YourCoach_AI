using System;
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

        // Gather all information in a single struct
        [Serializable]
        public struct RepSet
        {
            public int RepNumber;
            public int RepMaxNumber;

            public int SetNumber;
            public int SetMaxNumber;
        }

        public RepSet[] RepsSets = new RepSet[3]; // 3 = day | week | month

        private const string REPS = "Reps";
        private const string SETS = "Sets";

        private int timePeriodIndex;
        private int choiceIndex;

        void Start()
        {
            // When choice change, set display of rep or set stats depending on choice
            choice.onValueChanged.AddListener(SwitchChoice);

            // When period change, update display based on the new selected one
            Statistics.Instance.OnTimePeriodChanged += UpdateDisplay;
        }

        /// <summary>
        /// Display the rep or set stats depending on choice and based on selected period
        /// </summary>
        /// <param name="_choice"></param>
        private void SwitchChoice(int _choice)
        {
            choiceIndex = _choice;
            // If choice = 0 : display rep stats, else display set stats
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

        /// <summary>
        /// Update the display of rep or set based on selected period
        /// </summary>
        private void UpdateDisplay()
        {
            timePeriodIndex = (int)Statistics.Instance.SelectedPeriod;
            SwitchChoice(choiceIndex);
        }
    }
}
