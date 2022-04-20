using System.IO;
using System.Text;
using UnityEngine;
using ChoETL;
using UnityEditor;

namespace Code.Utils.Editor
{
    public class CSVJsonConverter : EditorWindow
    {
        private string inputFileName = "Translations.csv";
        private string outputFileName = "Translations.json";
        
        private const string inputPath = "Data/Input/";
        private const string outputPath = "Data/Output/";

        private void ConvertCSV()
        {
            if (!ArePathsValid())
            {
                return;
            }

            string inPath = Path.Combine(Application.dataPath, inputPath, inputFileName);
            string outPath = Path.Combine(Application.dataPath, outputPath);
            
            string csv = File.ReadAllText(inPath);
            StringBuilder sb = new StringBuilder();
            using (var p = ChoCSVReader.LoadText(csv).WithFirstLineHeader().MayHaveQuotedFields().MayContainEOLInData())
            {
                using (var w = new ChoJSONWriter(sb))
                {
                    w.Write(p);
                }
            }

            if (!Directory.Exists(outPath))
            {
                Directory.CreateDirectory(outPath);
            }

            outPath = Path.Combine(outPath, outputFileName);
            if (!File.Exists(outPath))
            {
               var file = File.Create(outPath);
               file.Close();
            }
            File.WriteAllText(outPath, sb.ToString());
            AssetDatabase.Refresh();
        }

        private bool ArePathsValid()
        {
            bool pathsValid = true;
            if (string.IsNullOrEmpty(inputFileName))
            {
                Debug.LogError("CSV-JSON : Input File Name is empty!");
                pathsValid = false;
            }

            if (string.IsNullOrEmpty(outputFileName))
            {
                Debug.LogError("CSV-JSON : Output File Name is empty!");
                pathsValid = false;
            }

            var path = Path.Combine(Application.dataPath,inputPath, inputFileName);
            if (!File.Exists(path))
            {
                Debug.LogError($"CSV-JSON : Input path \"{path}\" not pointing to a file!");
                pathsValid = false;
            }
            
            return pathsValid;
        }

        [MenuItem("Tools/CSVJsonConverter")]
        public static void ShowWindow()
        {
            GetWindow<CSVJsonConverter>("CSV-JSON Converter");
        }
        
        private void OnGUI()
        {
            inputFileName = EditorGUILayout.TextField("CSV File Path", inputFileName);
            outputFileName = EditorGUILayout.TextField("JSON File Path", outputFileName);
            if (GUILayout.Button("Convert CSV to JSON"))
            {
                ConvertCSV();
            }

            if (GUILayout.Button("Update Translations Asset"))
            {
                var path = Path.Combine(Application.dataPath, outputPath, outputFileName);
                TranslationsTools.PopulateDictionaries(path);
            }
        }
    }
}