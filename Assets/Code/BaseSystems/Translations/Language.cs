using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Code.BaseSystems.Translations
{
    [System.Serializable]
    public class Language
    {
        [SerializeField] private LanguageEnum id;
        [SerializeField] private string longName;

        public LanguageEnum Id => id;
        public string Name => longName;
    }

    public enum LanguageEnum
    {
        en,
        pl,
        ua
    }
}
