﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Better.Diagnostics.EditorAddons.NodeEditor;
using Better.Diagnostics.EditorAddons.NodeEditor.Models;
using Better.Diagnostics.Runtime.NodeModule;
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
        private static DiagnosticSettings _settings;

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
            _settings = ScriptableObject.CreateInstance<DiagnosticSettings>();
            _settings.SetInstances(settings.GetInstances());
            instance.SetActions(OnCreate, OnRemove);
            instance.SetMenuList(LazyGetAllInheritedType(typeof(ISettings)));
            List<NodeItem> list = new List<NodeItem>();
            NewMethod(list, instance);
            _screenGroup = new NodeGroup(new Rect(0, 0, Screen.width, Screen.height));
            instance.AddGroup(_screenGroup);
            instance.SetOffset(new Vector2(30, 30));
            instance.OnSave += OnSave;
            instance.OnChanged += OnChanged;
            instance.OnDiscard += OnDiscard;
            instance.OnClosed += OnClosed;
        }

        private static void NewMethod(List<NodeItem> list, NodeWindow instance)
        {
            foreach (var x in _settings.GetInstances())
            {
                if(x is INodeRect nodeRect)
                {
                    list.Add(new NodeItem(nodeRect));
                }
                else
                {
                    list.Add(new NodeItem(x, Rect.zero, null));
                }
            }

            instance.SetInstancedList(list.ToArray());
        }

        private static void OnClosed()
        {
            if (_settings != null)
            {
                Object.DestroyImmediate(_settings);
            }
        }

        private static void OnChanged()
        {
            EditorUtility.SetDirty(_settings);
        }

        private static void OnDiscard()
        {
            if (_settings == null) return;
            Object.DestroyImmediate(_settings);
        }

        private static void OnSave()
        {
            if (_settings == null) return;
            if (EditorUtility.IsDirty(_settings))
            {
                var settings = LoadOrCreateSettings();
                settings.SetInstances(_settings.GetInstances());

                EditorUtility.SetDirty(settings);
                AssetDatabase.SaveAssetIfDirty(settings);
            }
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

        private static void OnRemove(NodeItem obj)
        {
            if (obj.InnerObject is ISettings settings)
            {
                _settings.Remove(settings);
            }
        }

        private static void OnCreate(NodeItem obj)
        {
            if (obj.InnerObject is ISettings settings)
            {
                _settings.Add(settings);
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