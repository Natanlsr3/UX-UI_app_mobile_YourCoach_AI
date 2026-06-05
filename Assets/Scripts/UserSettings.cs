using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace MVC.App
{
    public class UserSettings : MonoBehaviour
    {
        [HideInInspector] public float Volume = 1f;
        [HideInInspector] public bool IsMute;

        public enum Language { French, English, Spanish }
        [HideInInspector] public Language CurrentLanguage = Language.English;
        [HideInInspector] public List<string> LanguageCodes = new List<string>() { "fr", "en", "es" };

        [HideInInspector] public int TimeZoneIndex;
        [HideInInspector] public bool IsAutoDetect;

        [HideInInspector] public int NotificationFromIndex;
        [HideInInspector] public int NotificationToIndex;
        [HideInInspector] public bool IsNotificationActive = true;

        [HideInInspector] public bool IsDarkMode;


        private static UserSettings instance;
        public static UserSettings Instance { get => instance; }

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
            LoadLanguage();
        }

        public void LoadLanguage()
        {
            LocalizationSettings.SelectedLocale = Locale.CreateLocale(LanguageCodes[(int)CurrentLanguage]);
        }

        private void OnDestroy()
        {
            instance = null;
        }
    }
}