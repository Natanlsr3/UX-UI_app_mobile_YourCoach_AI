using System;
using System.Collections;
using System.Collections.Generic;
using MVC.App.UI.Workout;
using UnityEngine;
using UnityEngine.Serialization;

namespace MVC.App
{
    public class LogSession : MonoBehaviour
    {
        [Serializable]
        private struct User
        {
            public string Name;
            public string Password;
        }
        [SerializeField] private List<User> possibleUsers;

        private int userIndex;
        public int UserIndex { get => userIndex; }

        [HideInInspector] public List<Exercise> WorkoutExercises = new List<Exercise>();
        [HideInInspector] public string WorkoutName;

        private static LogSession instance;
        public static LogSession Instance { get => instance; }

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

        public bool ValidUser(string _name, string _password)
        {
            bool _isValidUser = false;

            int _userNum = possibleUsers.Count;
            for (int i = 0; i < _userNum; i++)
            {
                User _user = possibleUsers[i];
                if (_name == _user.Name && _password == _user.Password)
                {
                    userIndex = i;
                    _isValidUser = true;
                    break;
                }
            }

            return _isValidUser;
        }

        void OnDestroy()
        {
            instance = null;
        }
    }
}

