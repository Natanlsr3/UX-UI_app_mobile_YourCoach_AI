using System;
using MVC.App.UI.MainMenu;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MVC.App.UI.Workout
{
    public class WorkoutProposal : WorkoutPreview
    {
        [Header("Choice")]
        [SerializeField] private Button noButton;
        [SerializeField] private GameObject startButtonPrefab;

        // The conversation instance that created this object. It is set by the conversation instance when created
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

        /// <summary>
        /// Add the workout to the session list and go to the workout menu
        /// </summary>
        private void StartWorkout()
        {
            // Create a new session with the registered information and add it to the list of sessions
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

            // Go to workout menu
            SceneManager.LoadScene("Workout");
        }

        /// <summary>
        /// Refuse the proposal and leave the pop-up
        /// </summary>
        private void RefuseProposal()
        {
            Conversation.SetConversationDisplay(true);
            Leave();
        }
    }
}

