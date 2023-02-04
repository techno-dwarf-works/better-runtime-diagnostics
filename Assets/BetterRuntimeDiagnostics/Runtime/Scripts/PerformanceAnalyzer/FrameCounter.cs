using UnityEngine;

namespace Better.Diagnostics.Runtime.PerformanceAnalyzer
{
    public class FrameCounter : IDebugInfo
    {
        private double _lastUpdate;
        private double _lastFPS;
        private readonly float _updateInterval;
        private readonly GUIContent _content;

        public FrameCounter(float updateInterval)
        {
            _updateInterval = updateInterval;
            _content = new GUIContent();
        }
        
        public void Initialize()
        {
        }

        public void OnGUI()
        {
            var sinceStartup = Time.realtimeSinceStartup;
            if(_lastUpdate + _updateInterval <= sinceStartup)
            {
                _lastFPS = Time.frameCount / sinceStartup;
                _lastUpdate = sinceStartup;
                _content.text = $"FPS {_lastFPS.ToString("F1")}";
            }
            GUILayout.Label(_content);
        }

        public void Deconstruct()
        {
        }
    }
}