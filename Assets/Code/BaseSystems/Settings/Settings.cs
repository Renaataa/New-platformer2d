using Code.BaseSystems.Translations;

namespace Code.BaseSystems.Settings
{
    [System.Serializable]
    public class Settings
    {
        public bool music;
        public bool sound;
        public LanguageEnum language;

        public Settings()
        {
            music = true;
            sound = true;
            language = LanguageEnum.en;
        }
    }
}