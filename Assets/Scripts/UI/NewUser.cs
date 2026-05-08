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
        [SerializeField] private Button previousButton;

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

        private Animator anim;
        private const string FORWARD = "MoveForward";
        private const string BACKWARD = "MoveBackward";

        private int step;

        private TextMeshProUGUI startButtonText;
        private const string START = "START";
        private const string NEXT = "NEXT";

        void Start()
        {
            anim = GetComponent<Animator>();
            startButtonText = startButton.GetComponentInChildren<TextMeshProUGUI>();
            startButton.onClick.AddListener(delegate{ MoveToNextStep();});
            previousButton.onClick.AddListener(delegate{ MoveToNextStep(false);});
            previousButton.gameObject.SetActive(false);

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

        private void MoveToNextStep(bool _isForward = true)
        {
            if (_isForward)
            {
                if (step == 0)
                {
                    if (firstNameField.text != "" & lastNameField.text != "" & weightField.text != "")
                    {
                        errorPanel.SetActive(false);

                        anim.SetTrigger(FORWARD);
                        startButtonText.text = START;
                        previousButton.gameObject.SetActive(true);

                        step++;
                    }
                    else errorPanel.SetActive(true);
                }
                else if (step == 1) ConnectToMainMenu();
            }
            else
            {
                step--;
                anim.SetTrigger(BACKWARD);
                startButtonText.text = NEXT;
                previousButton.gameObject.SetActive(false);
            }
        }

        private void ConnectToMainMenu()
        {
            // Set new user

            LogSession.User _user = LogSession.Instance.CurrentUser;

            _user.FirstName = firstNameField.text;
            _user.LastName = lastNameField.text;
            _user.Age = age.value;
            float weight;
            if (float.TryParse(weightField.text, out weight)) _user.Weight = weight;
            _user.Experience = (LogSession.FitnessFrequency)experience.value;
            _user.Goal = (LogSession.FitnessGoal)goal.value;
            _user.FreshAccount = false;

            LogSession.Instance.SetUser(_user);

            // Go to main menu
            SceneManager.LoadScene("Main");
        }
    }
}

