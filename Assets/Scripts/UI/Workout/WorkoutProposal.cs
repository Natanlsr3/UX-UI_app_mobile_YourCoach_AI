using System;
using System.Collections;
using System.Collections.Generic;
using MVC.App.UI.MainMenu;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace MVC.App.UI.Workout
{
    public class WorkoutProposal : WorkoutPreview
    {
        [Header("Choice")]
        [SerializeField] private Button noButton;
        [SerializeField] private GameObject startButtonPrefab;

        [HideInInspector] public Conversation Conversation;

        [Header("Additional Infos")]
        [SerializeField] private string workoutType;
        [SerializeField] private float duration;
        [SerializeField] private float calories;
        [SerializeField] private string muscles;

        protected override void Start()
        {
            base.Start();

            SetPreview(m_WorkoutName, m_Exercises);

            noButton.onClick.AddListener(RefuseProposal);

            Button _startButton = Instantiate(startButtonPrefab, m_ExerciseContainer).GetComponent<Button>();
            _startButton.onClick.AddListener(StartWorkout);

            LogSession.Instance.WorkoutExercises = m_Exercises;
            LogSession.Instance.OptExercises = m_OptExercises;
            LogSession.Instance.WorkoutName = m_WorkoutName;
        }

        private void StartWorkout()
        {
            LogSession.WorkoutSession _session = new LogSession.WorkoutSession()
            {
                WorkoutExercises = m_Exercises,
                OptExercises = m_OptExercises,
                WorkoutName = m_WorkoutName,
                WorkoutType = workoutType,
                Duration = duration,
                Date = DateTime.Today,
                CaloriesBurnt = calories,
                MusclesWorked = muscles
            };
            LogSession.Instance.WorkoutSessions.Add(_session);
            SceneManager.LoadScene("Workout");
        }

        private void RefuseProposal()
        {
            Conversation.SetConversationDisplay(true);
            Leave();
        }


    }
}

