using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

namespace MVC.App.UI.Workout
{
    public class WorkoutSessionManager : MonoBehaviour
    {
        [SerializeField] private Button backButton;
        [SerializeField] private Button skipButton;

        [Header("Coach Display")]
        [SerializeField] private VideoPlayer video;
        [SerializeField] private GameObject coach;

        [Header("Set x Rep")]
        [SerializeField] private GameObject repPanel;
        [SerializeField] private TextMeshProUGUI repNumberDisplay;

        [Header("Exercise Infos")]
        [SerializeField] private GameObject exercisePanel;
        [SerializeField] private TextMeshProUGUI exerciseName;
        [SerializeField] private TextMeshProUGUI exerciseDetail;

        [Header("Pause")]
        [SerializeField] private Button pauseButton;
        [SerializeField] private Sprite pauseImage;
        [SerializeField] private Sprite resumeImage;

        [Header("Time Progress")]
        [SerializeField] private Image timeProgress;
        [SerializeField] private TextMeshProUGUI currentTimeDisplay;
        [SerializeField] private TextMeshProUGUI maxTimeDisplay;

        [Header("Transition")]
        [SerializeField] private TextMeshProUGUI preparationDisplay;
        [SerializeField] private float preparationTime;
        [SerializeField] private float transitionDelay;
        [SerializeField] private float recoveryTransitionDelay;

        private Exercise currentExercise;
        private int exerciseIndex;
        private int exerciseNumber;

        private int exerciseProgressNumber;
        private int exerciseTotalNumber;

        private int repMaxNumber;
        private int repNumber;
        private float repFrequency;
        private float repProgress;

        private float maxExerciseTime;
        private float maxRecoveryTime;
        private float time;

        public const float SPEED = 1f;

        private Coroutine workoutCoroutine;
        private bool isPaused;

        private static WorkoutSessionManager instance;
        public static WorkoutSessionManager Instance { get => instance; }

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

        void Start()
        {
            skipButton.onClick.AddListener(SkipExercise);
            pauseButton.onClick.AddListener(SetPause);

            // Set total number of exercise

            exerciseTotalNumber = 0;
            int _exerciseTypeNum = LogSession.Instance.WorkoutExercises.Count;
            for (int i = 0; i < _exerciseTypeNum; i++)
            {
                exerciseTotalNumber += LogSession.Instance.WorkoutExercises[i].SetNumber;
            }
        }

        public void SetCurrentExercise()
        {
            if (exerciseIndex > LogSession.Instance.WorkoutExercises.Count - 1) return;

            currentExercise = LogSession.Instance.WorkoutExercises[exerciseIndex];

            exerciseName.text = currentExercise.Name;
            exerciseDetail.text = currentExercise.Muscle;

            repMaxNumber = currentExercise.RepNumber;
            repNumber = 0;
            repPanel.SetActive(true);
            repNumberDisplay.text = repNumber + "/" + repMaxNumber;

            maxExerciseTime = currentExercise.Duration;
            int minutes = Mathf.FloorToInt(maxExerciseTime / 60f);
            int seconds = Mathf.FloorToInt(maxExerciseTime % 60f);
            maxTimeDisplay.text = string.Format("{0:00}:{1:00}", minutes, seconds);

            repFrequency = maxExerciseTime / repMaxNumber;
            repProgress = 0f;

            time = 0f;
            UpdateTimeDisplay(maxExerciseTime);

            SetExerciseDisplay(false);

            WorkoutProgress.Instance.IsPaused = true;
            
            workoutCoroutine = StartCoroutine(PrepareExerciseCoroutine());
        }

        private void UpdateTimeDisplay(float _maxTime)
        {
            timeProgress.fillAmount = time / _maxTime;

            int minutes = Mathf.FloorToInt(time / 60f);
            int seconds = Mathf.FloorToInt(time % 60f);
            currentTimeDisplay.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }

        /// <summary>
        /// Display right informations at the end of a timer (exercise or recovery)
        /// </summary>
        /// <param name="_time"></param> Time of timer ending
        /// <param name="_changeRepDisplay"></param> Update or not display of the number of reps
        private void EndDisplay(float _time, bool _changeRepDisplay = false)
        {
            if (_changeRepDisplay) repNumberDisplay.text = repMaxNumber + "/" + repMaxNumber;

            timeProgress.fillAmount = 1f;

            int minutes = Mathf.FloorToInt(_time / 60f);
            int seconds = Mathf.FloorToInt(_time % 60f);
            currentTimeDisplay.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        }

        private IEnumerator PrepareExerciseCoroutine()
        {
            time = preparationTime;
            while (time > 0f)
            {
                time -= Time.deltaTime * SPEED;
                preparationDisplay.text = Mathf.CeilToInt(time).ToString();

                yield return new WaitForEndOfFrame();
            }

            yield return new WaitForSeconds(transitionDelay / SPEED);

            StopCoroutine(workoutCoroutine);

            SetExerciseDisplay(true);
            if (SoundManager.instance.soundSource.isPlaying)
                SoundManager.instance.StopClip();
            SoundManager.instance.GoToClip(SoundManager.instance.exercicesLines, exerciseIndex);
            if(!SoundManager.instance.soundSource.isPlaying)
                SoundManager.instance.PlayClip();
            exerciseNumber++;
            exerciseProgressNumber++;
            WorkoutProgress.Instance.IsPaused = false;
            workoutCoroutine = StartCoroutine(ExerciseCoroutine());
        }

        private IEnumerator ExerciseCoroutine()
        {
            while (time < maxExerciseTime)
            {
                // Handle Pause/Play
                while (isPaused)
                {
                    yield return null;
                }

                time += Time.deltaTime * SPEED;
                repProgress += Time.deltaTime * SPEED;
                if (repProgress >= repFrequency)
                {
                    repProgress = 0f;
                    repNumber++;
                    repNumberDisplay.text = repNumber + "/" + repMaxNumber;
                }
                UpdateTimeDisplay(maxExerciseTime);
                yield return new WaitForEndOfFrame();
            }
            EndDisplay(maxExerciseTime, true);

            WorkoutProgress.Instance.IsPaused = true;
            yield return new WaitForSeconds(recoveryTransitionDelay / SPEED);
            WorkoutProgress.Instance.IsPaused = false;

            StopCoroutine(workoutCoroutine);

            if (exerciseProgressNumber >= exerciseTotalNumber) SetEndSession();
            else SetRecovery();
        }

        private void SetRecovery()
        {
            exerciseName.text = "Recovery";
            exerciseDetail.text = "Time to breath !";

            repPanel.SetActive(false);

            maxRecoveryTime = currentExercise.RecoveryTime;
            int minutes = Mathf.FloorToInt(maxRecoveryTime / 60f);
            int seconds = Mathf.FloorToInt(maxRecoveryTime % 60f);
            maxTimeDisplay.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            time = 0f;
            UpdateTimeDisplay(maxRecoveryTime);

            workoutCoroutine = StartCoroutine(RecoveryCoroutine());
        }

        private IEnumerator RecoveryCoroutine()
        {
            while (time < maxRecoveryTime)
            {
                // Handle Pause/Play
                while (isPaused)
                {
                    yield return null;
                }

                time += Time.deltaTime * SPEED;
                UpdateTimeDisplay(maxRecoveryTime);
                yield return new WaitForEndOfFrame();
            }
            EndDisplay(maxRecoveryTime);

            WorkoutProgress.Instance.IsPaused = true;
            yield return new WaitForSeconds(transitionDelay / SPEED);
            WorkoutProgress.Instance.IsPaused = false;

            StopCoroutine(workoutCoroutine);

            if (exerciseNumber >= currentExercise.SetNumber)
            {
                exerciseIndex++;
                exerciseNumber = 0;
            }

            SetCurrentExercise();
        }

        private void SkipExercise()
        {
            video.Stop();
            coach.SetActive(false);

            StopAllCoroutines();
            exerciseIndex++;
            exerciseNumber = 0;
            SetCurrentExercise();
        }

        private void SetEndSession()
        {
            workoutCoroutine = StartCoroutine(EndSessionCoroutine());
        }

        private IEnumerator EndSessionCoroutine()
        {
            yield return new WaitForSeconds(3f / SPEED);

            StopCoroutine(workoutCoroutine);
            PlayerPrefs.SetInt("TrainingSessionDone", 0);
            SceneManager.LoadScene("Main");
        }

        private void SetExerciseDisplay(bool _display)
        {
            preparationDisplay.gameObject.SetActive(!_display);
            exercisePanel.SetActive(_display);
            repPanel.SetActive(_display);
        }

        private void SetPause()
        {
            isPaused = !isPaused;
            WorkoutProgress.Instance.IsPaused = isPaused;
            pauseButton.image.sprite = isPaused ? resumeImage : pauseImage;
        }

        void OnDestroy()
        {
            instance = null;
        }
    }
}

