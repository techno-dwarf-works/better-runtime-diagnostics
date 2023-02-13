using System;
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
        private List<IFixedUpdateableInfo> _fixedUpdateableInfos;

        private void Awake()
        {
            _settings = Resources.Load<DiagnosticSettings>(nameof(DiagnosticSettings));
            _list = _settings.SetUpInfos();
            _updateableInfos = new List<IUpdateableInfo>();
            _fixedUpdateableInfos = new List<IFixedUpdateableInfo>();
            foreach (var debugInfo in _list)
            {
                debugInfo.Initialize();
                if (debugInfo is IUpdateableInfo updateableInfo)
                {
                    _updateableInfos.Add(updateableInfo);
                }

                if (debugInfo is IFixedUpdateableInfo fixedUpdateableInfo)
                {
                    _fixedUpdateableInfos.Add(fixedUpdateableInfo);
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

        private void FixedUpdate()
        {
            foreach (var updateableInfo in _fixedUpdateableInfos)
            {
                updateableInfo.FixedUpdate();
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