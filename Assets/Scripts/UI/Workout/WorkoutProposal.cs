using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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

        protected override void Start()
        {
            base.Start();
            SetPreview(m_WorkoutName, m_Exercies);

            noButton.onClick.AddListener(RefuseProposal);

            Button _startButton = Instantiate(startButtonPrefab, m_ExerciseContainer).GetComponent<Button>();
            _startButton.onClick.AddListener(StartWorkout);

            WorkoutSession.Instance.WorkoutExercises = m_Exercies;
            WorkoutSession.Instance.WorkoutName = m_WorkoutName;
        }

        private void StartWorkout()
        {
            SceneManager.LoadScene("Workout");
        }

        private void RefuseProposal()
        {
            Leave();
        }


    }
}

