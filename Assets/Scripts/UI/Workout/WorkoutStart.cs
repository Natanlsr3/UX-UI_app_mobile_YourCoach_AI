using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MVC.App.UI.Workout
{
    public class WorkoutStart : MonoBehaviour
    {
        [SerializeField] private Button backButton;
        [SerializeField] private Button detailButton;
        [SerializeField] private Button startButton;

        [SerializeField] private TextMeshProUGUI workoutName;

        [SerializeField] private GameObject workoutPreviewPrefab;
        private WorkoutPreview workoutPreview;

        // Arrow images displayed on the detail button
        [SerializeField] private Image[] arrows;

        private bool isPreviewDisplayed;

        void Start()
        {
            // Connect buttons
            backButton.onClick.AddListener(Back);
            detailButton.onClick.AddListener(SetWorkoutPreview);
            startButton.onClick.AddListener(StartWorkout);

            workoutName.text = LogSession.Instance.WorkoutName;

            // Disable the session manager until the workout is not starting
            WorkoutSessionManager.Instance.gameObject.SetActive(false);
        }

        /// <summary>
        /// Create and display the workout preview, with the list of exercises and their information
        /// </summary>
        private void SetWorkoutPreview()
        {
            isPreviewDisplayed = !isPreviewDisplayed;
            if (isPreviewDisplayed)
            {
                workoutPreview = Instantiate(workoutPreviewPrefab, transform).GetComponent<WorkoutPreview>();
                workoutPreview.SetPreview("Upcoming Exercises", LogSession.Instance.WorkoutExercises);

                // Rotate the arrows of the detail button
                foreach (Image _arrow in arrows)
                {
                    _arrow.transform.rotation = Quaternion.AngleAxis(180f, Vector3.forward);
                }
            }
            else
            {
                workoutPreview.Leave();

                // Reset the arrows of the detail button
                foreach (Image _arrow in arrows)
                {
                    _arrow.transform.rotation = Quaternion.Euler(0, 0, 0);
                }
            }
        }

        /// <summary>
        /// Return to the main menu
        /// </summary>
        private void Back()
        {
            SceneManager.LoadScene("Main");
        }

        /// <summary>
        /// Start the workout
        /// </summary>
        private void StartWorkout()
        {
            gameObject.SetActive(false);

            // Enable and initialize the session manager
            WorkoutSessionManager.Instance.gameObject.SetActive(true);
            WorkoutSessionManager.Instance.SetCurrentExercise();
            WorkoutProgress.Instance.StartProgress();
        }
    }

}
