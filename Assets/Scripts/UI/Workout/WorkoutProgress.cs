using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MVC.App.UI.Workout
{
    public class WorkoutProgress : MonoBehaviour
    {
        [SerializeField] private GameObject exerciseSeparation;
        [SerializeField] private Transform separationContainer;
        [SerializeField] private RectTransform progressBar;
        [SerializeField] private Image workoutProgress;
        [SerializeField] private TextMeshProUGUI progressPercent;

        // The list that contain the total duration of the different types of exercise, including recovery times
        private List<float> exercisesDuration = new List<float>();

        private float totalTime;
        private float time;

        private float progressMaxLength;

        [HideInInspector] public bool IsPaused;

        #region Singleton

        private static WorkoutProgress instance;
        public static WorkoutProgress Instance { get => instance; }

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

        #endregion

        void Start()
        {
            SetWorkoutProgress();
        }

        /// <summary>
        /// Initialize the workout progress bar with the different separations between exercises
        /// </summary>
        private void SetWorkoutProgress()
        {
            // Set the total duration of the workout

            float _exerciseTotalDuration;
            totalTime = 0f;

            int _exerciseNum = LogSession.Instance.WorkoutExercises.Count;
            for (int i = 0; i < _exerciseNum; i++)
            {
                // Calculate the total duration with the number of exercise, the number of recovery and their duration
                _exerciseTotalDuration = LogSession.Instance.WorkoutExercises[i].Duration * LogSession.Instance.WorkoutExercises[i].SetNumber
                                         + LogSession.Instance.WorkoutExercises[i].RecoveryTime * LogSession.Instance.WorkoutExercises[i].SetNumber;

                exercisesDuration.Add(_exerciseTotalDuration);
                totalTime += _exerciseTotalDuration;
            }

            // Place indicators on the progress bar

            List<Transform> _separationList = new List<Transform>();

            progressMaxLength = progressBar.rect.width;
            Vector3 _progressStartPos = Vector3.zero + Vector3.left * (progressMaxLength / 2f);

            int _separationNum = _exerciseNum - 1;
            for (int i = 0; i < _separationNum; i++)
            {
                // Get the position of the previous separation, if there is no previous separation get the start position instead
                Vector3 _previousSeparationPos = (i > 0) ? _separationList[i - 1].localPosition : _progressStartPos;

                // Calculate the separation position
                float _durationRatio = exercisesDuration[i] / totalTime;
                Vector3 _position = _previousSeparationPos + Vector3.right * progressMaxLength * _durationRatio;

                // Create and place the separation
                GameObject _separation = Instantiate(exerciseSeparation, separationContainer);
                _separation.transform.localPosition = _position;
                _separationList.Add(_separation.transform);
            }
        }

        /// <summary>
        /// Start the tracking of the workout progress
        /// </summary>
        public void StartProgress()
        {
            StartCoroutine(ProgressCoroutine());
        }

        /// <summary>
        /// Progress through the workout and update the progress bar
        /// </summary>
        /// <returns></returns>
        private IEnumerator ProgressCoroutine()
        {
            while (time < totalTime)
            {
                // Handle Pause/Play
                while (IsPaused)
                {
                    yield return null;
                }

                time += Time.deltaTime * WorkoutSessionManager.SPEED;
                workoutProgress.fillAmount = time / totalTime;
                progressPercent.text = Mathf.FloorToInt(time / totalTime * 100) + "%";

                yield return new WaitForEndOfFrame();
            }
        }

        /// <summary>
        /// Add time to the current workout progress
        /// </summary>
        /// <param name="_time"></param>
        public void AddTime(float _time){ time += _time; }

        private void OnDestroy()
        {
            instance = null;
        }
    }
}

