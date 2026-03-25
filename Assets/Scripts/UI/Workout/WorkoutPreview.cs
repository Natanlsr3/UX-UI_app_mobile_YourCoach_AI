using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace MVC.App.UI.Workout
{
    public class WorkoutPreview : MonoBehaviour
    {
        [Header("Exercises")]
        [SerializeField] protected string m_WorkoutName;
        [SerializeField] protected TextMeshProUGUI m_NameDisplay;
        [SerializeField] protected Transform m_ExerciseContainer;
        [SerializeField] protected GameObject m_ExerciseDisplayPrefab;

        [SerializeField] protected List<Exercise> m_Exercies;

        protected virtual void Start()
        {
        }

        public void SetPreview(string _name, List<Exercise> _exercises)
        {
            m_WorkoutName = _name;
            m_NameDisplay.text = m_WorkoutName;

            m_Exercies = _exercises;

            m_NameDisplay.text = m_WorkoutName;

            int _exerciseNum = m_Exercies.Count;
            for (int i = 0; i < _exerciseNum; i++)
            {
                Exercise _exercise = m_Exercies[i];
                ExerciseDisplay _exerciseDisplay = Instantiate(m_ExerciseDisplayPrefab, m_ExerciseContainer).GetComponent<ExerciseDisplay>();
                _exerciseDisplay.SetExercise(_exercise.Name, _exercise.ExercisePreview, _exercise.Duration, _exercise.Level, _exercise.SetNumber, _exercise.RepNumber, _exercise.Muscle, _exercise.RecoveryTime);
            }
        }

        public void Leave()
        {
            Destroy(gameObject);
        }

    }
}

