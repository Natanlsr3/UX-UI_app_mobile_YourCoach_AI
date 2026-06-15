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
        public enum FitnessFrequency { Regularly = 0, Sometimes = 1, AWhile = 2, FirstTime = 3 }

        [Serializable]
        public enum FitnessGoal { Health = 0, Aesthetics = 1, Performance = 2, Mobility = 3, Preparation = 4 }

        [Serializable]
        public struct User
        {
            public string UserName;
            public string Password;

            public string FirstName;
            public string LastName;

            public int Age;
            public float Weight;

            public FitnessFrequency Experience;
            public FitnessGoal Goal;

            public bool FreshAccount;
        }
        public List<User> UserList;
        public User CurrentUser { get => UserList[userIndex]; }

        private int userIndex;
        public int UserIndex { get => userIndex; }

        [HideInInspector] public List<Exercise> WorkoutExercises = new List<Exercise>();
        [HideInInspector] public List<Exercise> OptExercises = new List<Exercise>();
        [HideInInspector] public string WorkoutName;

        [Serializable]
        public struct WorkoutSession
        {
            public List<Exercise> WorkoutExercises;
            public List<Exercise> OptExercises;
            public string WorkoutName;
            public string WorkoutType;
            public float Duration;
            public DateTime Date;
            public float CaloriesBurnt;
            public string MusclesWorked;
        }

        public List<WorkoutSession> WorkoutSessions;

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
            int _userNum = UserList.Count;
            for (int i = 0; i < _userNum; i++)
            {
                User _user = UserList[i];
                if (_name == _user.UserName && _password == _user.Password)
                {
                    userIndex = i;
                    return true;
                } 
            }

            return false;
        }

        public bool ExistingUser(string _name)
        {
            int _userNum = UserList.Count;
            for (int i = 0; i < _userNum; i++)
            {
                User _user = UserList[i];
                if (_name == _user.UserName)
                {
                    return true;
                }
            }

            return false;
        }

        public void SetUser(User _user)
        {
            UserList[userIndex] = _user;
        }

        public void CreateUser(string _username, string _password)
        {
            User _newUser = new User();
            _newUser.UserName = _username;
            _newUser.Password = _password;
            _newUser.FreshAccount = true;

            UserList.Add(_newUser);
        }

        public User FindUser(string _username)
        {
            int _userNum = UserList.Count;
            for (int i = 0; i < _userNum; i++)
            {
                User _user = UserList[i];
                if (_username == _user.UserName)
                {
                    return _user;
                }
            }

            return new User();
        }

        public int FindUserIndex(string _username)
        {
            int _userNum = UserList.Count;
            for (int i = 0; i < _userNum; i++)
            {
                User _user = UserList[i];
                if (_username == _user.UserName)
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// Check if there is an existing workout session at the given date
        /// </summary>
        /// <param name="_date"></param>
        /// <returns></returns>
        public bool CheckWorkoutDate(DateTime _date)
        {
            int _sessionNum = WorkoutSessions.Count;
            for (int i = 0; i < _sessionNum; i++)
            {
                if (WorkoutSessions[i].Date == _date) return true;
            }

            return false;
        }

        /// <summary>
        /// Get the workout session at the given date
        /// </summary>
        /// <param name="_date"></param>
        /// <returns></returns>
        public WorkoutSession GetSessionAtDate(DateTime _date)
        {
            int _sessionNum = WorkoutSessions.Count;
            for (int i = 0; i < _sessionNum; i++)
            {
                if (WorkoutSessions[i].Date == _date) return WorkoutSessions[i];
            }

            // if no session found, return a default workout session
            return new WorkoutSession();
        }

        void OnDestroy()
        {
            instance = null;
        }
    }
}

