using UnityEngine;

namespace Better.Diagnostics.Runtime.PerformanceAnalyzer
{
    public class FrameCounter : IDebugInfo
    {
        private double _lastUpdate;
        private double _lastFPS;
        public void Initialize()
        {
        }

        public void OnGUI()
        {
            var sinceStartup = Time.realtimeSinceStartup;
            if(_lastUpdate + 1f <= sinceStartup)
            {
                _lastFPS = Time.renderedFrameCount / sinceStartup;
                _lastUpdate = sinceStartup;
            }
            GUILayout.Label(new GUIContent($"FPS {_lastFPS.ToString("F1")}"));
        }

        public void Deconstruct()
        {
        }
    }
}