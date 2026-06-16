using System;
using System.Collections.Generic;
using UnityEngine;

namespace MVC.App
{
    public class ChallengeTracker : MonoBehaviour
    {
        // The different type of challenge, with their different way of progress
        public enum ChallengeType { CompleteWorkout, Connexion }
        public enum ChallengeState { InProgress, Completed }

        [Serializable]
        public struct Challenge
        {
            public string Title;
            public string Info;
            public float MaxProgress;
            public float Progress;
            public ChallengeType Type;
            public ChallengeState State;
        }

        // The starting list of challenges
        [SerializeField] private List<Challenge> challengeList = new List<Challenge>();
        // The complete list of challenges
        [HideInInspector] public List<Challenge> Challenges = new List<Challenge>();

        #region Singleton

        private static ChallengeTracker instance;
        public static ChallengeTracker Instance { get => instance; }

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

        #endregion

        void Start()
        {
            DontDestroyOnLoad(gameObject);
            Challenges = challengeList;
        }

        /// <summary>
        /// Increase the progress of a challenge by looking at its type
        /// </summary>
        /// <param name="_type"></param>
        /// <param name="_progress"></param>
        public void Progress(ChallengeType _type, float _progress)
        {
            Challenge _progressChallenge;

            int _challengeNum = Challenges.Count;
            for (int i = 0; i < _challengeNum; i++)
            {
                Challenge _challenge = Challenges[i];

                // Find challenges with the same type as the one given to increase their progress
                if (_challenge.Type == _type)
                {
                    _progressChallenge = _challenge;
                    _progressChallenge.Progress += _progress;

                    // Set the state of the challenge based on its progress : Completed / In Progress
                    if (_progressChallenge.Progress >= _progressChallenge.MaxProgress) _progressChallenge.State = ChallengeState.Completed;
                    else _progressChallenge.State = ChallengeState.InProgress;

                    Challenges[i] = _progressChallenge;
                }
            }
        }
    }
}

