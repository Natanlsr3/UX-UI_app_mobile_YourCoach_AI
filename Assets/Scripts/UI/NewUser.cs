using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Button = UnityEngine.UI.Button;

public class NewUser : MonoBehaviour
{
    [SerializeField] private Button startButton;

    [SerializeField] private GameObject errorPanel;

    [SerializeField] private TMP_InputField firstName;
    [SerializeField] private TMP_InputField lastName;

    [Header("Age")]
    [SerializeField] private TMP_Dropdown age;
    [SerializeField] private int minAge;
    [SerializeField] private int maxAge;

    [Header("Weight")]
    [SerializeField] private TMP_Dropdown weight;
    [SerializeField] private int minWeight;
    [SerializeField] private int maxWeight;

    void Start()
    {
        startButton.onClick.AddListener(ConnectToMainMenu);

        // Clear dropdown
        age.ClearOptions();
        weight.ClearOptions();

        // Add options to dropdown

        List<string> ageList = new List<string>();
        for (int i = minAge; i <= maxAge; i++)
        {
            ageList.Add(i.ToString());
        }
        age.AddOptions(ageList);

        List<string> weightList = new List<string>();
        for (int i = minWeight; i <= maxWeight; i++)
        {
            weightList.Add(i + "kg");
        }
        weight.AddOptions(weightList);
    }

    private void ConnectToMainMenu()
    {
        if (firstName.text != "" & lastName.text != "") SceneManager.LoadScene("Main");
        else errorPanel.SetActive(true);
    }
}
