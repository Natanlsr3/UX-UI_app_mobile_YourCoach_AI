using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace MVC.App.UI.SecondaryMenu
{
    public class Profile : SecondaryMenu
    {
        [Header("Personal infos")]
        [SerializeField] private TextMeshProUGUI firstNameDisplay;
        [SerializeField] private TextMeshProUGUI lastNameDisplay;
        [SerializeField] private TMP_InputField passwordDisplay;

        [Header("Age")]
        [SerializeField] private TMP_Dropdown age;
        [SerializeField] private int minAge;
        [SerializeField] private int maxAge;

        [Header("")]
        [SerializeField] private TMP_InputField weightField;
        [SerializeField] private TMP_Dropdown goal;

        protected override void Start()
        {
            base.Start();

            firstNameDisplay.text = LogSession.Instance.CurrentUser.FirstName;
            lastNameDisplay.text = LogSession.Instance.CurrentUser.LastName;
            passwordDisplay.text = LogSession.Instance.CurrentUser.Password;

            // Clear dropdown
            age.ClearOptions();
            // Add options to dropdown
            List<string> _ageList = new List<string>();
            for (int i = minAge; i <= maxAge; i++)
            {
                _ageList.Add(i.ToString());
            }
            age.AddOptions(_ageList);
            // Set dropdown index
            age.value = LogSession.Instance.CurrentUser.Age;

            weightField.text = LogSession.Instance.CurrentUser.Weight.ToString();

            goal.value = (int)LogSession.Instance.CurrentUser.Goal;
        }
    }
}
