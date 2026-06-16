using UnityEngine;

namespace MVC.App.UI.SecondaryMenu.Challenge
{
    public class Challenges : SecondaryMenu
    {
        [SerializeField] private GameObject challengeDisplay;
        [SerializeField] private Transform challengeDisplayContainer;
        protected override void Start()
        {
            base.Start();

            // Create and display the current tracked challenge with their information and state
            int _challengeNum = ChallengeTracker.Instance.Challenges.Count;
            for (int i = 0; i < _challengeNum; i++)
            {
                ChallengeDisplay _challengeDisplay = Instantiate(challengeDisplay, challengeDisplayContainer).GetComponent<ChallengeDisplay>();
                _challengeDisplay.SetDisplay(ChallengeTracker.Instance.Challenges[i]);
            }
        }
    }
}
