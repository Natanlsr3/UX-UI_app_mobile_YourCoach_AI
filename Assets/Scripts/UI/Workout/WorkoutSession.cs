using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace MVC.App.UI.Workout
{
    public class WorkoutSession : MonoBehaviour
    {
        [HideInInspector] public List<Exercise> WorkoutExercises = new List<Exercise>();
        [HideInInspector] public string WorkoutName;

        private static WorkoutSession instance;
        public static WorkoutSession Instance { get => instance; }

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
            DontDestroyOnLoad(gameObject);
        }

        void OnDestroy()
        {
            instance = null;
        }
    }
}

