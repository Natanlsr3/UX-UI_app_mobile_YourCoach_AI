using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace MVC.App.UI.Workout
{
    public class WorkoutPreview : MonoBehaviour
    {
        [Header("Exercises")]
        [SerializeField] protected string m_WorkoutName;
        [SerializeField] protected TextMeshProUGUI m_NameDisplay;
        [SerializeField] protected Transform m_ExerciseContainer;
        [SerializeField] protected GameObject m_ExerciseDisplayPrefab;

        // List of workout exercises
        [SerializeField] protected List<Exercise> m_Exercises;

        // List of optional workout exercises (= replacement exercises)
        [SerializeField] protected List<Exercise> m_OptExercises;

        protected virtual void Start()
        {
        }

        /// <summary>
        /// Create and display the list of exercises in the workout and their information
        /// </summary>
        /// <param name="_name"></param>
        /// <param name="_exercises"></param>
        public void SetPreview(string _name, List<Exercise> _exercises)
        {
            m_WorkoutName = _name;
            m_NameDisplay.text = m_WorkoutName;

            m_Exercises = _exercises;

            m_NameDisplay.text = m_WorkoutName;

            int _exerciseNum = m_Exercises.Count;
            for (int i = 0; i < _exerciseNum; i++)
            {
                Exercise _exercise = m_Exercises[i];
                ExerciseDisplay _exerciseDisplay = Instantiate(m_ExerciseDisplayPrefab, m_ExerciseContainer).GetComponent<ExerciseDisplay>();
                _exerciseDisplay.SetExercise(_exercise.Name, _exercise.ExercisePreview, _exercise.Duration, _exercise.Level, _exercise.SetNumber, _exercise.RepNumber, _exercise.Muscle, _exercise.RecoveryTime);
            }
        }

        /// <summary>
        /// Exit and destroy the preview
        /// </summary>
        public void Leave()
        {
            Destroy(gameObject);
        }
    }
}

