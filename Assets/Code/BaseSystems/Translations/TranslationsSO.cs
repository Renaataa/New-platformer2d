using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Code.BaseSystems.Translations
{
    [CreateAssetMenu(fileName ="Translations Dictionary",menuName ="Data/New Translation Dictionary")]
    public class TranslationsSO : ScriptableObject
    {
        [SerializeField]
        private List<TranslationEntry> entriesList;
        private Dictionary<string, TranslationEntry> entries = new Dictionary<string, TranslationEntry>();

        public void Set(List<TranslationEntry> e)
        {
            entriesList = e;
            Prepare();
        }
        public void Prepare()
        {
            entriesList.ForEach(AddEntry);
        }

        private void AddEntry(TranslationEntry entry)
        {
            if (entries.ContainsKey(entry.Id))
            {
                Debug.LogWarning($"Dictionary already contains ID : {entry.Id}. Skipping.");
                return;
            }

            entries.Add(entry.Id, entry);
        }
        
        public TranslationEntry GetEntry(string id)
        {
            try
            {
                return entries[id];
            }
            catch
            {
                return new TranslationEntry();
            }
        }
        public string GetPhrase(string id, LanguageEnum lang)
        {
            string phrase = GetEntry(id).Get(lang);
            if(phrase == "###")
            {
                phrase += id;
            }
            return phrase;
        }
    }
}
