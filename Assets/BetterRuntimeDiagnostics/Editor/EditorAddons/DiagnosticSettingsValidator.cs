using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Better.Diagnostics.EditorAddons.SettingsEditor;
using Better.Diagnostics.Runtime.SettingsModule;
using Better.Diagnostics.Runtime.SettingsModule.Interfaces;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Object = UnityEngine.Object;

namespace Better.Diagnostics.EditorAddons
{
    [InitializeOnLoad]
    public static class DiagnosticSettingsValidator
    {
        private static string[] _folderPaths = new string[]
            { nameof(Better), nameof(Diagnostics), "Resources" };

        private static NodeGroup _screenGroup;

        private static string GenerateRelativePath()
        {
            return Path.Combine(_folderPaths);
        }

        static DiagnosticSettingsValidator()
        {
            LoadOrCreateSettings();
        }

        [MenuItem("Better/Diagnostics/Settings")]
        private static void HighlightSettings()
        {
            var instance = NodeWindow.OpenWindow();
            var settings = LoadOrCreateSettings();
            instance.SetActions((obj) => OnCreate(obj, settings), (obj) => OnRemove(obj, settings));
            instance.SetMenuList(LazyGetAllInheritedType(typeof(ISettings)));
            instance.SetInstancedList(settings.GetInstances().Select(x => new NodeItem(x, x.Position, x.SetPosition)).ToArray());
            _screenGroup = new NodeGroup(new Rect(0,0, Screen.width, Screen.height));
            instance.AddGroup(_screenGroup);
            instance.SetOffset(new Vector2(30, 30));
        }

        private static Type[] LazyGetAllInheritedType(Type baseType)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes()).Where(p => ArgIsValueType(baseType, p)).ToArray();
        }

        private static bool ArgIsValueType(Type baseType, Type iterateValue)
        {
            return CheckType(baseType, iterateValue) &&
                   (iterateValue.IsClass || iterateValue.IsValueType) &&
                   !iterateValue.IsAbstract && !iterateValue.IsSubclassOf(typeof(Object));
        }

        private static bool CheckType(Type baseType, Type p)
        {
            return baseType.IsAssignableFrom(p);
        }

        private static void OnRemove(NodeItem obj, DiagnosticSettings diagnosticSettings)
        {
            if (obj.InnerObject is ISettings settings)
            {
                diagnosticSettings.Remove(settings);
            }
        }

        private static void OnCreate(NodeItem obj, DiagnosticSettings diagnosticSettings)
        {
            if (obj.InnerObject is ISettings settings)
            {
                diagnosticSettings.Add(settings);
            }
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