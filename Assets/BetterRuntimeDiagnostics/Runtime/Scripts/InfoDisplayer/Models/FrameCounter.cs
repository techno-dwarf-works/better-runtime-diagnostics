using Better.Diagnostics.Runtime.InfoDisplayer.Interfaces;
using Better.Diagnostics.Runtime.InfoDisplayer.Utils;
using UnityEngine;

namespace Better.Diagnostics.Runtime.InfoDisplayer.Models
{
    public class FrameCounter : IDebugInfo, IUpdateableInfo
    {
        private float _deltaTime;
        private float _lastFPS;
        private readonly GUIContent _content;
        private float _nextUpdate;
        private readonly UpdateTimer _updateTimer;

        public FrameCounter(UpdateInterval updateInterval)
        {
            _updateTimer = new UpdateTimer(updateInterval, OnUpdate);
            _content = new GUIContent();
        }

        public void Initialize()
        {
        }

        public void Update()
        {
            _updateTimer.Update();
        }

        private void OnUpdate()
        {
            _lastFPS = 1f / Time.unscaledDeltaTime;;
            _content.text = $"FPS {_lastFPS.ToString("F1")}";
        }

        public void OnGUI()
        {
            GUILayout.Label(_content);
        }

        public void Deconstruct()
        {
        }
    }
}