using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MVC.App.UI.Workout
{
    public class WorkoutProposal : MonoBehaviour
    {
        [Header("Exercises")]
        [SerializeField] private Transform exerciseContainer;
        [SerializeField] private GameObject exerciseDisplay;

        [SerializeField] private List<Exercise> exercies;

        [Header("Choice")]
        [SerializeField] private Button yesButton;
        [SerializeField] private Button noButton;

        void Start()
        {
            yesButton.onClick.AddListener(AcceptProposal);
            noButton.onClick.AddListener(RefuseProposal);

            int _exerciseNum = exercies.Count;
            for (int i = 0; i < _exerciseNum; i++)
            {
                Exercise _exercise = exercies[i];
                ExerciseDisplay _exerciseDisplay = Instantiate(exerciseDisplay, exerciseContainer).GetComponent<ExerciseDisplay>();
                _exerciseDisplay.SetExercise(_exercise.Name, _exercise.ExercisePreview,_exercise.Duration, _exercise.Level, _exercise.SetNumber, _exercise.RepNumber, _exercise.Muscle, _exercise.RecoveryTime);
            }
        }

        private void AcceptProposal()
        {
            LeaveProposal();
        }

        private void RefuseProposal()
        {
            LeaveProposal();
        }

        private void LeaveProposal()
        {
            Destroy(gameObject);
        }
    }
}

