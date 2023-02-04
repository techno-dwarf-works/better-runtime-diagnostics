using System;
using System.Globalization;
using Better.Extensions.Runtime;
using UnityEngine;

namespace Better.Diagnostics.Runtime.PerformanceAnalyzer
{
    public class RAMUsage : IDebugInfo
    {
        private readonly float _updateInterval;
        private double _lastUpdate;
        private long _usedTotalMemory;
        private long _usedMainThreadMemory;
        private readonly GUIContent _totalContent;

        public RAMUsage(float updateInterval)
        {
            _updateInterval = updateInterval;
            _totalContent = new GUIContent();
        }

        public void Initialize()
        {
        }

        public void OnGUI()
        {
            var sinceStartup = Time.realtimeSinceStartup;
            if (_lastUpdate + _updateInterval <= sinceStartup)
            {
                _lastUpdate = sinceStartup;
                _usedTotalMemory = GC.GetTotalMemory(false);
                _totalContent.text = _usedTotalMemory.ToMegabytes().ToString("F1") + " mb";
            }

            GUILayout.Label(_totalContent);
        }

        public void Deconstruct()
        {
        }
    }
}