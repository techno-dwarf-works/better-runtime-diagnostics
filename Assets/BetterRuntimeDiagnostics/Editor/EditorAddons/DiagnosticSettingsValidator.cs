using System;
using System.IO;
using Better.Diagnostics.Runtime.SettingsModule;
using UnityEditor;
using UnityEngine;

namespace Better.Diagnostics.EditorAddons
{
    [InitializeOnLoad]
    public static class DiagnosticSettingsValidator
    {
        private static string[] _folderPaths = new string[]
            { nameof(Better), nameof(Diagnostics), "Resources" };

        private static string GenerateRelativePath()
        {
            return Path.Combine(_folderPaths);
        }

        static DiagnosticSettingsValidator()
        {
            LoadOrCreateSettings();
        }

        //TODO: Create window
        [MenuItem("Better/Diagnostics/Settings")]
        private static void HighlightSettings()
        {
            throw new NotImplementedException();
        }

        private static DiagnosticSettings LoadOrCreateSettings()
        {
            var settings = Resources.Load<DiagnosticSettings>(nameof(DiagnosticSettings));
            if (settings != null) return settings;

            settings = ScriptableObject.CreateInstance<DiagnosticSettings>();
            
            var relativePath = GenerateRelativePath();
            var absolutePath = Path.Combine(Application.dataPath, relativePath);

            if (!Directory.Exists(absolutePath))
            {
                Directory.CreateDirectory(absolutePath);
            }

            relativePath = Path.Combine("Assets", relativePath, $"{nameof(DiagnosticSettings)}.asset");
            AssetDatabase.CreateAsset(settings, relativePath);
            return settings;
        }
    }
}