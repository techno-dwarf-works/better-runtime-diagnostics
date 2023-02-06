using System.Collections.Generic;
using Better.Diagnostics.Runtime.InfoDisplayer.Interfaces;
using Better.Diagnostics.Runtime.SettingsModule;
using UnityEngine;

namespace Better.Diagnostics.Runtime.InfoDisplayer
{
    public class DebugUIManager : MonoBehaviour
    {
        private DiagnosticSettings _settings;
        private List<IDebugInfo> _list;
        private List<IUpdateableInfo> _updateableInfos;

        private void Awake()
        {
            _settings = Resources.Load<DiagnosticSettings>(nameof(DiagnosticSettings));
            _list = _settings.SetUpInfos();
            _updateableInfos = new List<IUpdateableInfo>();
            foreach (var debugInfo in _list)
            {
                debugInfo.Initialize();
                if (debugInfo is IUpdateableInfo updateableInfo)
                {
                    _updateableInfos.Add(updateableInfo);
                }
            }
        }

        private void Update()
        {
            foreach (var updateableInfo in _updateableInfos)
            {
                updateableInfo.Update();
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