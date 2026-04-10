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
        [SerializeField] private Button microButton;

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

        private enum WorkoutPhase { Explication, Exercise, Recovery };

        private WorkoutPhase currentPhase;

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
            microButton.onClick.AddListener(PrepareToSkip);

            // Set total number of exercise

            exerciseTotalNumber = 0;
            int _exerciseTypeNum = LogSession.Instance.WorkoutExercises.Count;
            for (int i = 0; i < _exerciseTypeNum; i++)
            {
                exerciseTotalNumber += LogSession.Instance.WorkoutExercises[i].SetNumber;
            }

            coach.SetActive(false);
        }

        /// <summary>
        /// Update the current exercise to be displayed
        /// </summary>
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

        /// <summary>
        /// Coroutine to handle preparation before exercise
        /// </summary>
        /// <returns></returns>
        private IEnumerator PrepareExerciseCoroutine()
        {
            currentPhase = WorkoutPhase.Explication;

            time = preparationTime;
            while (time > 0f)
            {
                time -= Time.deltaTime * SPEED;
                preparationDisplay.text = Mathf.CeilToInt(time).ToString();

                yield return new WaitForEndOfFrame();
            }

            yield return new WaitForSeconds(transitionDelay / SPEED);

            StopCoroutine(workoutCoroutine);
            SetExercise();
        }

        /// <summary>
        /// Initialize the exercise phase
        /// </summary>
        private void SetExercise()
        {
            SetExerciseDisplay(true);
            if (SoundManager.instance.soundSource.isPlaying)
                SoundManager.instance.StopClip();
            if (currentExercise.Name.Contains("Wall"))
                SoundManager.instance.GoToClip(SoundManager.instance.exercicesLines, exerciseIndex + 1);
            else
                SoundManager.instance.GoToClip(SoundManager.instance.exercicesLines, exerciseIndex);
            if (!SoundManager.instance.soundSource.isPlaying && exerciseNumber==0)
                SoundManager.instance.PlayClip();
            exerciseNumber++;
            exerciseProgressNumber++;
            WorkoutProgress.Instance.IsPaused = false;
            workoutCoroutine = StartCoroutine(ExerciseCoroutine());
        }

        /// <summary>
        /// Coroutine to handle the exercises
        /// </summary>
        /// <returns></returns>
        private IEnumerator ExerciseCoroutine()
        {
            currentPhase = WorkoutPhase.Exercise;

            video.clip = currentExercise.Video;
            coach.SetActive(true);
            video.Play();

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

        /// <summary>
        /// Initialize the recovery phase
        /// </summary>
        private void SetRecovery()
        {
            exerciseName.text = "Recovery";
            exerciseDetail.text = "Time to breath !";

            repPanel.SetActive(false);
            video.Stop();
            coach.SetActive(false);

            maxRecoveryTime = currentExercise.RecoveryTime;
            int minutes = Mathf.FloorToInt(maxRecoveryTime / 60f);
            int seconds = Mathf.FloorToInt(maxRecoveryTime % 60f);
            maxTimeDisplay.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            time = 0f;
            UpdateTimeDisplay(maxRecoveryTime);

            workoutCoroutine = StartCoroutine(RecoveryCoroutine());
        }

        /// <summary>
        /// Coroutine that handle recovery
        /// </summary>
        /// <returns></returns>
        private IEnumerator RecoveryCoroutine()
        {
            currentPhase = WorkoutPhase.Recovery;
            if (currentExercise.Name.Contains("Bird") && exerciseNumber >= currentExercise.SetNumber)
            {
                SoundManager.instance.GoToClip(SoundManager.instance.recoveryLines, 0);
                SoundManager.instance.PlayClip();
            }
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

        /// <summary>
        /// Skip a phase of the training
        /// </summary>
        private void SkipExercise()
        {
            video.Stop();
            coach.SetActive(false);
            if (SoundManager.instance.soundSource.isPlaying)
                SoundManager.instance.StopClip();
            StopAllCoroutines();
            AddRemainingTime();

            switch (currentPhase)
            {
                case WorkoutPhase.Explication:
                    SetExercise();
                    break;
                case WorkoutPhase.Exercise:
                    EndDisplay(maxExerciseTime, true);
                    SetRecovery();
                    break;
                case WorkoutPhase.Recovery:
                    if (exerciseNumber >= currentExercise.SetNumber)
                    {
                        exerciseIndex++;
                        exerciseNumber = 0;
                    }
                    SetCurrentExercise();
                    break;
            }

            if (exerciseProgressNumber >= exerciseTotalNumber) SetEndSession();
        }
        /// <summary>
        /// Prepare to skip to the "alternate" exercise 
        /// </summary>
        private void PrepareToSkip()
        {
            video.Stop();
            coach.SetActive(false);
            StopCoroutine(workoutCoroutine);
            
            if (currentExercise.Name.Contains("Rowing"))
            {
                //To test may be removed if working weirdly
                AddRemainingTime();
                StartCoroutine(SkipToAlternate());

            }

            else
            {
                video.Play();
                coach.SetActive(true);
                workoutCoroutine = StartCoroutine(PrepareExerciseCoroutine());
            }
           
        }

        /// <summary>
        /// Skip to "Alternate" exercise
        /// </summary>
        /// <returns></returns>
        IEnumerator SkipToAlternate()
        {
            
            SoundManager.instance.StopClip();
            if (SoundManager.instance.hasLineEnded)
            {
                SoundManager.instance.GoToClip(SoundManager.instance.altExercisesLines, 0);
                SoundManager.instance.PlayClip();
            }
            yield return new WaitWhile(()=>SoundManager.instance.soundSource.isPlaying);
            if (exerciseIndex >= LogSession.Instance.WorkoutExercises.Count-1)
            {
                exerciseTotalNumber -= (currentExercise.SetNumber - exerciseNumber);
                exerciseTotalNumber += (LogSession.Instance.OptExercises[0].SetNumber - exerciseNumber);
                //!!!!!Warning works only in this demo would have to rework the system to include on the fly modification later
                LogSession.Instance.WorkoutExercises[exerciseIndex] = LogSession.Instance.OptExercises[0];
                exerciseNumber = 0;
            }
            
            SetCurrentExercise();

        }


        /// <summary>
        /// Add time remaining to the timer when skipping a phase
        /// </summary>
        private void AddRemainingTime()
        {
            float _remainingTime;
            switch (currentPhase)
            {
                case WorkoutPhase.Exercise:
                    _remainingTime = maxExerciseTime - time;
                    break;
                case WorkoutPhase.Recovery:
                    _remainingTime = maxRecoveryTime - time;
                    break;
                default:
                    _remainingTime = 0f;
                    break;
            }
            WorkoutProgress.Instance.AddTime(_remainingTime);
        }

        /// <summary>
        /// Set the end of the training
        /// </summary>
        private void SetEndSession()
        {
            workoutCoroutine = StartCoroutine(EndSessionCoroutine());
        }

        /// <summary>
        /// Coroutine to call at the end of the training
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Pause/Resume the timer and the video of the training
        /// </summary>
        private void SetPause()
        {
            isPaused = !isPaused;
            WorkoutProgress.Instance.IsPaused = isPaused;
            pauseButton.image.sprite = isPaused ? resumeImage : pauseImage;

            if (isPaused) video.Pause();
            else video.Play();
        }

        void OnDestroy()
        {
            instance = null;
        }
    }
}

