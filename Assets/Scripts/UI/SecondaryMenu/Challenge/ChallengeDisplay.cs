using System.Collections;
using System.Collections.Generic;
using MVC.App;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace MVC.App.UI.SecondaryMenu.Challenge
{
    public class ChallengeDisplay : MonoBehaviour
    {
        [Header("Description")]
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI info;

        [Header("Progress")]
        [SerializeField] private TextMeshProUGUI progressDisplay;
        [SerializeField] private Image progressBar;

        [Header("State")]
        [SerializeField] private TextMeshProUGUI stateInfo;
        [SerializeField] private Image stateDisplay;
        [SerializeField] private Sprite inProgressDisplay;
        [SerializeField] private Sprite completedDisplay;

        private const string IN_PROGRESS_TEXT = "In Progress";
        private const string COMPLETED_TEXT = "Completed";
        void Start()
        {

        }

        public void SetDisplay(ChallengeTracker.Challenge _challenge)
        {
            title.text = _challenge.Title;
            info.text = _challenge.Info;

            float _currentProgress = _challenge.Progress;
            float _maxProgress = _challenge.MaxProgress;
            progressDisplay.text = _currentProgress + "/" + _maxProgress;

            progressBar.fillAmount = _currentProgress / _maxProgress;

            switch (_challenge.State)
            {
                case ChallengeTracker.ChallengeState.InProgress :
                    stateInfo.text = IN_PROGRESS_TEXT;
                    stateInfo.color = Color.cyan;
                    progressBar.color = Color.cyan;
                    stateDisplay.sprite = inProgressDisplay;
                    break;
                case ChallengeTracker.ChallengeState.Completed :
                    stateInfo.text = COMPLETED_TEXT;
                    stateInfo.color = Color.green;
                    progressBar.color = Color.green;
                    stateDisplay.sprite = completedDisplay;
                    break;
            }
        }
    }
}

