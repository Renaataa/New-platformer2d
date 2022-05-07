using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using ChoETL;
using Newtonsoft.Json;
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
            string json = String.Empty;;
            
            foreach (CultureInfo info in CultureInfo.GetCultures(CultureTypes.NeutralCultures))
            {
                if (string.IsNullOrEmpty(info.Name))
                {
                    continue;
                }
                StringBuilder sb = new StringBuilder();
                using (var p = ChoCSVReader.LoadText(csv).WithFirstLineHeader().MayHaveQuotedFields().MayContainEOLInData())
                {
                    ChoJSONRecordConfiguration config = new ChoJSONRecordConfiguration
                    {
                        Culture = info
                    };
                    using (var w = new ChoJSONWriter(sb,config))
                    {
                        w.Write(p);
                    }
                }
                
                if (sb.ToString().Substring(15, 1) != "\"")
                {
                    continue;
                }
                
                json = sb.ToString();
                break;
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
            File.WriteAllText(outPath, json);
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