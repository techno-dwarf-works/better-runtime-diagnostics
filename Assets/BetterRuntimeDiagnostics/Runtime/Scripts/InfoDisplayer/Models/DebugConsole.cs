using System;
using System.Collections.Generic;
using Better.Diagnostics.Runtime.InfoDisplayer.Interfaces;
using UnityEngine;

namespace Better.Diagnostics.Runtime.InfoDisplayer.Models
{
    [Serializable]
    public class DebugConsole : IDebugInfo
    {
        private HashSet<KeyCode> _codes;

        private HashSet<KeyCode> _currentKeys;

        public DebugConsole(HashSet<KeyCode> codes)
        {
            _codes = codes;
            _currentKeys = new HashSet<KeyCode>();
        }

        public void Initialize()
        {
        }

        public void OnGUI()
        {
            var e = new Event();
            while (Event.PopEvent(e))
            {
                if (e.keyCode != KeyCode.None)
                {
                    if (e.type == EventType.KeyDown)
                    {
                        _currentKeys.Add(e.keyCode);
                    }
                    else if(e.type == EventType.KeyUp)
                    {
                        _currentKeys.Remove(e.keyCode);
                    }
                }
            }

            if (_codes.SetEquals(_currentKeys))
            {
                _currentKeys.Clear();
                e.Use();
                Debug.Log("Triggered");
            }
        }

        public void Deconstruct()
        {
        }
    }
}