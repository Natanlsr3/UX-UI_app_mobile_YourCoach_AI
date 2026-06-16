using MVC.App.UI.Workout;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MVC.App.UI.SecondaryMenu.Training
{
    public class Training : SecondaryMenu
    {
        [SerializeField] private GameObject sessionDisplay;

        [SerializeField] private TextMeshProUGUI nameDisplay;
        [SerializeField] private TextMeshProUGUI typeDisplay;
        [SerializeField] private TextMeshProUGUI durationDisplay;
        [SerializeField] private TextMeshProUGUI dateDisplay;
        [SerializeField] private TextMeshProUGUI caloriesDisplay;
        [SerializeField] private TextMeshProUGUI musclesDisplay;

        [SerializeField] private Button detailButton;
        [SerializeField] private GameObject workoutPreviewPrefab;
        private WorkoutPreview workoutPreview;

        private bool isPreviewDisplayed;

        protected override void Start()
        {
            base.Start();
            detailButton.onClick.AddListener(DisplayExercises);
            sessionDisplay.SetActive(false);

            // Connect to button click
            DayDisplay.OnDayClick += UpdateDisplay;
        }

        /// <summary>
        /// Update the workout display based on the given date
        /// </summary>
        /// <param name="_day"></param>
        private void UpdateDisplay(DayDisplay _day)
        {
            // Check if there is a workout at the given date
            if (LogSession.Instance.CheckWorkoutDate(_day.Date))
            {
                sessionDisplay.SetActive(true);

                LogSession.WorkoutSession _session = LogSession.Instance.GetSessionAtDate(_day.Date);
                nameDisplay.text = _session.WorkoutName;
                typeDisplay.text = _session.WorkoutType;

                // Update duration display
                float _durationHours = Mathf.Floor(_session.Duration / 60f);
                float _durationMinutes = _session.Duration % 60f;
                if (_durationHours >= 1 && _durationMinutes > 0) durationDisplay.text = _durationHours + "h" + _durationMinutes;
                else if (_durationHours >= 1) durationDisplay.text = _durationHours + " h";
                else durationDisplay.text = _durationMinutes + " min";

                dateDisplay.text = _session.Date.ToShortDateString();
                caloriesDisplay.text = _session.CaloriesBurnt + " kcal";
                musclesDisplay.text = _session.MusclesWorked;
            }
            else sessionDisplay.SetActive(false);
        }

        /// <summary>
        /// Create and display the exercise list of the workout, with their information.
        /// If there is already a pop-up active, leave the pop-up window
        /// </summary>
        private void DisplayExercises()
        {
            if (isPreviewDisplayed)
            {
                workoutPreview.Leave();
            }
            else
            {
                // Create and set the display of the preview with the selected date workout
                LogSession.WorkoutSession _session = LogSession.Instance.GetSessionAtDate(Calendar.Instance.SelectedDate);
                workoutPreview = Instantiate(workoutPreviewPrefab, transform).GetComponent<WorkoutPreview>();
                workoutPreview.SetPreview(_session.WorkoutName, _session.WorkoutExercises);
            }
            isPreviewDisplayed = !isPreviewDisplayed;
        }
    }
}

