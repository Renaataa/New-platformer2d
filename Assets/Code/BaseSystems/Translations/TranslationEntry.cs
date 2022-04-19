using UnityEngine;

namespace Code.BaseSystems.Translations
{
    [System.Serializable]
    public class TranslationEntry
    {
        [SerializeField] private string id;
        [SerializeField] private string en;
        [SerializeField] private string pl;
        [SerializeField] private string ua;
        public string Id => id;

        public string Get(LanguageEnum e)
        {
            if(string.IsNullOrEmpty(Id))
            {
                return "###";
            }
            switch (e)
            {
                case LanguageEnum.en: return en;
                case LanguageEnum.pl: return string.IsNullOrEmpty(pl)? en : pl;
                case LanguageEnum.ua: return string.IsNullOrEmpty(ua) ? en : ua;
                default: return en;
            }
        }
    }
}