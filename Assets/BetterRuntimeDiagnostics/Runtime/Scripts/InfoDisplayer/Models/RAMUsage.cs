using System;
using Better.Diagnostics.Runtime.InfoDisplayer.Interfaces;
using Better.Diagnostics.Runtime.InfoDisplayer.Utils;
using Better.Extensions.Runtime;
using UnityEngine;

namespace Better.Diagnostics.Runtime.InfoDisplayer.Models
{
    public class RAMUsage : IDebugInfo, IUpdateableInfo
    {
        private readonly Vector2 _position;
        private long _usedTotalMemory;
        private readonly GUIContent _totalContent;
        private readonly UpdateTimer _updateTimer;

        public RAMUsage(Vector2 position, UpdateInterval updateInterval)
        {
            _position = position;
            _updateTimer = new UpdateTimer(updateInterval, OnUpdate);
            _totalContent = new GUIContent();
        }

        public void Initialize()
        {
        }

        public void OnGUI()
        {
            GUI.Label(new Rect(_position, GUI.skin.label.CalcSize(_totalContent)), _totalContent);
        }

        public void Deconstruct()
        {
        }

        public void Update()
        {
            _updateTimer.Update();
        }

        private void OnUpdate()
        {
            _usedTotalMemory = GC.GetTotalMemory(false);
            _totalContent.text = _usedTotalMemory.ToMegabytes().ToString("F1") + " mb";
        }
    }
}