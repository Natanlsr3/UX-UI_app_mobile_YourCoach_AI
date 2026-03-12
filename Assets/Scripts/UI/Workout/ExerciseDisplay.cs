using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MVC.App.UI.Workout
{
    [Serializable]
    public struct Exercise
    {
        public string Name;
        public Sprite ExercisePreview;
        public float Duration;
        public string Level;
        public int SetNumber;
        public int RepNumber;
        public string Muscle;
        public float RecoveryTime;
    }

    public class ExerciseDisplay : MonoBehaviour
    {
        [Header("Exercise Infos")] 
        [SerializeField] private TextMeshProUGUI name;
        [SerializeField] private Image exercisePreview;
        [SerializeField] private TextMeshProUGUI duration;
        [SerializeField] private TextMeshProUGUI level;
        [SerializeField] private TextMeshProUGUI setsReps;
        [SerializeField] private TextMeshProUGUI muscle;
        [SerializeField] private TextMeshProUGUI recoveryTime;

        void Start()
        {

        }

        public void SetExercise(string _name, Sprite _exercisePreview,float _duration, string _level, int _setNum, int _repNum, string _muscle, float _recoveryTime)
        {
            name.text = _name;
            exercisePreview.sprite = _exercisePreview;

            float _durationMinutes = Mathf.Floor(_duration / 60f);
            float _durationSeconds = _duration % 60f;
            if (_durationMinutes >= 1 && _durationSeconds > 0) duration.text = _durationMinutes.ToString() + "m " + _durationSeconds.ToString() + "s";
            else if (_durationMinutes >= 1) duration.text = _durationMinutes.ToString() + " min ";
            else duration.text = _durationSeconds.ToString() + " sec";

            level.text = _level;
            setsReps.text = _setNum + " x " + _repNum;
            muscle.text = _muscle;

            float _recoveryTimeMinutes = Mathf.Floor(_recoveryTime / 60f);
            float _recoveryTimeSeconds = _recoveryTime % 60;
            if (_recoveryTimeMinutes >= 1 && _recoveryTimeSeconds > 0) recoveryTime.text = _recoveryTimeMinutes.ToString() + "m " + _recoveryTimeSeconds.ToString() + "s Recovery";
            else if (_recoveryTimeMinutes >= 1) recoveryTime.text = _recoveryTimeMinutes.ToString() + " min Recovery";
            else recoveryTime.text = _recoveryTimeSeconds.ToString() + " sec Recovery";
        }
    }
}


