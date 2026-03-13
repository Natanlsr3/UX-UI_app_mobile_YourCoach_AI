using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MVC.App.UI.Workout
{
    public class WorkoutManager : MonoBehaviour
    {
        [SerializeField] private Button backButton;
        [SerializeField] private Button detailButton;
        [SerializeField] private Button startButton;

        [SerializeField] private GameObject workoutPreviewPrefab;
        private WorkoutPreview workoutPreview;

        [SerializeField] private Image[] arrows;

        private bool isPreviewDisplayed;

        void Start()
        {
            backButton.onClick.AddListener(Back);
            detailButton.onClick.AddListener(SetWorkoutPreview);
        }

        private void SetWorkoutPreview()
        {
            isPreviewDisplayed = !isPreviewDisplayed;
            if (isPreviewDisplayed)
            {
                workoutPreview = Instantiate(workoutPreviewPrefab, transform).GetComponent<WorkoutPreview>();
                workoutPreview.SetPreview(WorkoutSession.Instance.WorkoutName, WorkoutSession.Instance.WorkoutExercises);

                foreach (Image _arrow in arrows)
                {
                    _arrow.transform.rotation = Quaternion.AngleAxis(180f, Vector3.forward);
                }
            }
            else
            {
                workoutPreview.Leave();

                foreach (Image _arrow in arrows)
                {
                    _arrow.transform.rotation = Quaternion.Euler(0, 0, 0);
                }
            }
        }

        private void Back()
        {
            SceneManager.LoadScene("Main");
        }
    }

}
