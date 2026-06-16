using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace MVC.App
{
    public class UserSettings : MonoBehaviour
    {
        // Volume settings
        [HideInInspector] public float Volume = 1f;
        [HideInInspector] public bool IsMute;

        // Language settings
        public enum Language { French, English, Spanish }
        public Language CurrentLanguage = Language.English;
        [HideInInspector] public List<string> LanguageCodes = new List<string>() { "fr", "en", "es" };

        // Time zone settings
        [HideInInspector] public int TimeZoneIndex;
        [HideInInspector] public bool IsAutoDetect;

        // Notification settings
        [HideInInspector] public int NotificationFromIndex;
        [HideInInspector] public int NotificationToIndex;
        [HideInInspector] public bool IsNotificationActive = true;

        // Theme settings
        [HideInInspector] public bool IsDarkMode;

        #region Singleton

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

        #endregion

        void Start()
        {
            DontDestroyOnLoad(gameObject);
            LoadLanguage();
        }

        /// <summary>
        /// Generate the localization based on the current selected language
        /// </summary>
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