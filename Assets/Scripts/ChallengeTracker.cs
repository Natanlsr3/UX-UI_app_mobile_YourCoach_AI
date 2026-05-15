using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MVC.App
{
    public class ChallengeTracker : MonoBehaviour
    {
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

        [SerializeField] private List<Challenge> challengeList = new List<Challenge>();
        [HideInInspector] public List<Challenge> Challenges = new List<Challenge>();

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

        void Start()
        {
            DontDestroyOnLoad(gameObject);
            Challenges = challengeList;
        }

        public void Progress(ChallengeType _type, float _progress)
        {
            Challenge _progressChallenge;

            int _challengeNum = Challenges.Count;
            for (int i = 0; i < _challengeNum; i++)
            {
                Challenge _challenge = Challenges[i];
                if (_challenge.Type == _type)
                {
                    _progressChallenge = _challenge;
                    _progressChallenge.Progress += _progress;

                    if (_progressChallenge.Progress >= _progressChallenge.MaxProgress) _progressChallenge.State = ChallengeState.Completed;
                    else _progressChallenge.State = ChallengeState.InProgress;

                    Challenges[i] = _progressChallenge;
                }
            }
        }
    }
}

