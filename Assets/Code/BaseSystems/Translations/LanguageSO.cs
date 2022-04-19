using System.Collections.Generic;
using UnityEngine;

namespace Code.BaseSystems.Translations
{
    [CreateAssetMenu(fileName = "Languages", menuName = "Data/Languages")]
    public class LanguageSO : ScriptableObject
    {
        [SerializeField] private List<Language> languages;
        public List<Language> Languages => languages;

        public Language GetLanguage(LanguageEnum lang)
        {
            return languages.Find(c => c.Id == lang);
        }
    }
}
