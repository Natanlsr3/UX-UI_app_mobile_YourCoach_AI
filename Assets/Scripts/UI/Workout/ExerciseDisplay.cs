using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

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
        public VideoClip Video;
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

        /// <summary>
        /// Set the display of the exercise with the given information
        /// </summary>
        /// <param name="_name"></param>
        /// <param name="_exercisePreview"></param>
        /// <param name="_duration"></param>
        /// <param name="_level"></param>
        /// <param name="_setNum"></param>
        /// <param name="_repNum"></param>
        /// <param name="_muscle"></param>
        /// <param name="_recoveryTime"></param>
        public void SetExercise(string _name, Sprite _exercisePreview,float _duration, string _level, int _setNum, int _repNum, string _muscle, float _recoveryTime)
        {
            name.text = _name;
            exercisePreview.sprite = _exercisePreview;

            // Display exercise duration in minutes + seconds
            float _durationMinutes = Mathf.Floor(_duration / 60f);
            float _durationSeconds = _duration % 60f;
            if (_durationMinutes >= 1 && _durationSeconds > 0) duration.text = _durationMinutes + "m " + _durationSeconds + "s";
            else if (_durationMinutes >= 1) duration.text = _durationMinutes + " min";
            else duration.text = _durationSeconds + " sec";

            level.text = _level;
            setsReps.text = _setNum + " x " + _repNum;
            muscle.text = _muscle;

            // Display recovery duration in minutes + seconds
            float _recoveryTimeMinutes = Mathf.Floor(_recoveryTime / 60f);
            float _recoveryTimeSeconds = _recoveryTime % 60;
            if (_recoveryTimeMinutes >= 1 && _recoveryTimeSeconds > 0) recoveryTime.text = _recoveryTimeMinutes + "m " + _recoveryTimeSeconds + "s Recovery";
            else if (_recoveryTimeMinutes >= 1) recoveryTime.text = _recoveryTimeMinutes + " min Recovery";
            else recoveryTime.text = _recoveryTimeSeconds + " sec Recovery";
        }
    }
}


