using System;
using Code.BaseSystems.Translations;
using UnityEngine;

namespace Code.BaseSystems.Settings
{
    public class SettingsManager : MonoBehaviour
    {
        #region Singleton

        private static SettingsManager instance;

        public static SettingsManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<SettingsManager>();
                    if (instance == null)
                    {
                        instance = new GameObject("SettingsManager").AddComponent<SettingsManager>();
                        instance.LoadSettings();
                        DontDestroyOnLoad(instance.gameObject);
                    }
                }

                return instance;
            }
        }
        
        #endregion Singleton
        
        public event Action<LanguageEnum> OnLanguageChange;

        private Settings currentSettings;

        public bool Music
        {
            get => currentSettings.music;
            set => currentSettings.music = value;
        }

        public bool Sound
        {
            get => currentSettings.sound;
            set => currentSettings.sound = value;
        }

        public LanguageEnum Language
        {
            get => currentSettings.language;
            set
            {
                currentSettings.language = value;
                OnLanguageChange?.Invoke(value);
            }
        }
        
        private void Awake()
        {
            LoadSettings();
        }

        private void LoadSettings()
        {
            if (PlayerPrefs.HasKey("Settings"))
            {
                string settingsJson = PlayerPrefs.GetString("Settings");
                currentSettings = JsonUtility.FromJson<Settings>(settingsJson);
            }
            else
            {
                currentSettings = new Settings();
            }
            OnLanguageChange?.Invoke(currentSettings.language);
        }

        public void SaveSettings()
        {
            string settingsJson = JsonUtility.ToJson(currentSettings);
            PlayerPrefs.SetString("Settings",settingsJson);
        }
        
    }
}