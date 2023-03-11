using System;
using Better.Diagnostics.Runtime.InfoDisplayer.Interfaces;
using Better.Diagnostics.Runtime.InfoDisplayer.Utils;
using Better.Extensions.Runtime;
using UnityEngine;
using UnityEngine.UIElements;

namespace Better.Diagnostics.Runtime.InfoDisplayer.Models
{
    public class RAMUsage : IDebugInfo, IUpdateableInfo
    {
        private long _usedTotalMemory;
        private readonly UpdateTimer _updateTimer;
        private readonly Label _label;

        public RAMUsage(Rect position, UpdateInterval updateInterval)
        {
            _label = VisualElementsFactory.CreateElement<Label>(position, Color.white);
            _updateTimer = new UpdateTimer(updateInterval, OnUpdate);
        }

        public void Initialize(UIDocument uiDocument)
        {
            uiDocument.rootVisualElement.Add(_label);
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
            _label.text = _usedTotalMemory.ToMegabytes().ToString("F1") + " mb";
        }
    }
}