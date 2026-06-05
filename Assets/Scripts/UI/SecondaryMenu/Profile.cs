using System.Collections;
using System.Collections.Generic;
using MVC.App.UI.RegisterMenu.RegisterPopUp;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MVC.App.UI.SecondaryMenu
{
    public class Profile : SecondaryMenu
    {
        [SerializeField] private Button settingsButton;

        [SerializeField] private GameObject forgetPasswordScreen;
        [SerializeField] private Button forgetPasswordButton;

        [Header("Personal infos")]
        [SerializeField] private TextMeshProUGUI firstNameDisplay;
        [SerializeField] private TextMeshProUGUI lastNameDisplay;
        [SerializeField] private TMP_InputField passwordDisplay;

        [SerializeField] private TMP_InputField weightField;
        [SerializeField] private TMP_Dropdown goal;

        [Header("Age")]
        [SerializeField] private TMP_Dropdown age;
        [SerializeField] private int minAge;
        [SerializeField] private int maxAge;

        private Animator anim;
        private const string IS_ACTIVE = "IsActive";

        protected override void Start()
        {
            base.Start();

            anim = GetComponent<Animator>();
            settingsButton.onClick.AddListener(GoToSettings);
            forgetPasswordButton.onClick.AddListener(SetForgetPasswordWindow);

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

        private void GoToSettings()
        {
            SceneManager.LoadScene("Settings");
        }

        private void SetForgetPasswordWindow()
        {
            anim.SetBool(IS_ACTIVE, false);

            ForgetPassword window = Instantiate(forgetPasswordScreen, transform).GetComponent<ForgetPassword>();
            window.SetTitleText("Change Password");
            window.OnLeftPopUp += DisplayProfile;
        }

        private void DisplayProfile()
        {
            passwordDisplay.text = LogSession.Instance.CurrentUser.Password;
            anim.SetBool(IS_ACTIVE, true);
        }
    }
}
