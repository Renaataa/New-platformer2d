using System;
using Code.BaseSystems.Settings;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Code.BaseSystems.Translations
{
    public class TranslationsManager : MonoBehaviour
    {
        [SerializeField]
        private LanguageSO languageSO;
        [SerializeField]
        private TranslationsSO dictionary;

        private Language currentLanguage;

        public static TranslationsManager Instance;

        public event Action OnLanguageChange;
        
        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(Instance.gameObject);
            }
            if(dictionary == null)
            {
                dictionary = AssetDatabase.LoadAssetAtPath("Assets/Data/TranslationsDictionary.asset", typeof(TranslationsSO)) as TranslationsSO;
            }
            dictionary?.Prepare();
            
            SettingsManager.Instance.OnLanguageChange += SettingsManager_OnLanguageChange;
            SettingsManager_OnLanguageChange(SettingsManager.Instance.Language);
        }

        public void SetDictionary(TranslationsSO newDictionary)
        {
            dictionary = newDictionary;
        }
        
        public string GetPhrase(string id) => dictionary.GetPhrase(id, currentLanguage.Id);

        #region Language
        public List<string> GetLanguagesList()
        {
            List<string> list = new List<string>();
            languageSO.Languages.ForEach(l => list.Add(l.Name));
            return list;
        }

        private Language Language(LanguageEnum e) => languageSO.GetLanguage(e);

        public Language LanguageFromDropdownValue(int val)
        {
            return Language((LanguageEnum)val);
        }

        private void SettingsManager_OnLanguageChange(LanguageEnum l)
        {
            currentLanguage = Language(l);
            OnLanguageChange?.Invoke();
        }
        #endregion
    }
}