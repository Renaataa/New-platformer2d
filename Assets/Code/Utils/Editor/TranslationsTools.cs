#if UNITY_EDITOR
using System.IO;
using System.Linq;
using Code.BaseSystems.Translations;
using UnityEditor;
using UnityEngine;

namespace Code.Utils.Editor
{
    public static class TranslationsTools
    {
        public static void PopulateDictionaries(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Debug.Log("Can't update Translations. Input file doesn't exists!");
                return;
            }
            StreamReader reader = new StreamReader(filePath);
            string json = reader.ReadToEnd();
            reader.Close();
            TranslationEntry[] entries = Extensions.FromJson<TranslationEntry>(json);
            var dictionary = ScriptableObject.CreateInstance<TranslationsSO>();
            dictionary.Set(entries.ToList());
            if (AssetDatabase.DeleteAsset("Assets/Data/TranslationsDictionary.asset"))
            {
                Debug.Log("Previous Translations Dictionary has been deleted. Replacing with new one.");
            }
            AssetDatabase.CreateAsset(dictionary, "Assets/Data/TranslationsDictionary.asset");
            AssetDatabase.Refresh();
            TryToUpdateTranslationsManager(dictionary);
        }

        private static void TryToUpdateTranslationsManager(TranslationsSO dictionary)
        {
            TranslationsManager manager = GameObject.FindObjectOfType<TranslationsManager>();
            manager.SetDictionary(dictionary);
        }
    }
}
#endif