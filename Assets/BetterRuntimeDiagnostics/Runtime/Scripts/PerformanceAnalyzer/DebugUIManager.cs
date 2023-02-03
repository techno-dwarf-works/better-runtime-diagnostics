using System;
using System.Collections.Generic;
using Better.Diagnostics.Runtime.SettingsModule;
using UnityEngine;

namespace Better.Diagnostics.Runtime.PerformanceAnalyzer
{
    public class DebugUIManager : MonoBehaviour
    {
        private DiagnosticSettings _settings;
        private List<IDebugInfo> _list;

        private void Awake()
        {
            _settings = Resources.Load<DiagnosticSettings>(nameof(DiagnosticSettings));
            _list = _settings.SetUpInfos();
            foreach (var debugInfo in _list)
            {
                debugInfo.Initialize();
            }
        }

        private void OnGUI()
        {
            GUILayout.BeginVertical();
            foreach (var debugInfo in _list)
            {
                debugInfo.OnGUI();
            }
            GUILayout.EndVertical();
        }

        private void OnDestroy()
        {
            foreach (var debugInfo in _list)
            {
                debugInfo.Deconstruct();
            }
        }
    }
}