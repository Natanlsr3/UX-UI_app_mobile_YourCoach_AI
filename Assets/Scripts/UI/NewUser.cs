using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Button = UnityEngine.UI.Button;

namespace MVC.App.UI
{
    public class NewUser : MonoBehaviour
    {
        [SerializeField] private Button startButton;

        [SerializeField] private GameObject errorPanel;

        [Header("Name")]
        [SerializeField] private TMP_InputField firstNameField;
        [SerializeField] private TMP_InputField lastNameField;

        [Header("Age")]
        [SerializeField] private TMP_Dropdown age;
        [SerializeField] private int minAge;
        [SerializeField] private int maxAge;

        [Header("Weight")]
        [SerializeField] private TMP_InputField weightField;

        [Header("Fitness Info")]
        [SerializeField] private TMP_Dropdown experience;
        [SerializeField] private TMP_Dropdown goal;


        void Start()
        {
            startButton.onClick.AddListener(ConnectToMainMenu);

            // Clear dropdown
            age.ClearOptions();

            // Add options to dropdown
            List<string> _ageList = new List<string>();
            for (int i = minAge; i <= maxAge; i++)
            {
                _ageList.Add(i.ToString());
            }
            age.AddOptions(_ageList);
        }

        private void ConnectToMainMenu()
        {
            if (firstNameField.text != "" & lastNameField.text != "" & weightField.text != "")
            {
                // Set new user

                LogSession.User _user = LogSession.Instance.CurrentUser;

                _user.FirstName = firstNameField.text;
                _user.LastName = lastNameField.text;
                _user.Age = age.value + minAge;
                float weight;
                if (float.TryParse(weightField.text, out weight)) _user.Weight = weight;
                _user.Experience = (LogSession.FitnessFrequency)experience.value;
                _user.Goal = (LogSession.FitnessGoal)goal.value;
                _user.FreshAccount = false;

                LogSession.Instance.SetUser(_user);

                // Go to main menu
                SceneManager.LoadScene("Main");
            }
            else errorPanel.SetActive(true);
        }
    }
}

