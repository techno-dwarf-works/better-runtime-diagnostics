using System;
using System.Collections.Generic;
using Better.Diagnostics.Runtime.InfoDisplayer.Interfaces;
using Better.Diagnostics.Runtime.InfoDisplayer.Models;
using Better.Diagnostics.Runtime.SettingsModule.Interfaces;
using UnityEngine;

namespace Better.Diagnostics.Runtime.SettingsModule.Models
{
    [Serializable]
    public class ConsoleSettings : ISettings
    {
        [SerializeField] private List<KeyCode> keyCodes;
        [SerializeField] private KeyCode keyCode;

        public IDebugInfo GetInfo()
        {
            return new DebugConsole(new HashSet<KeyCode>());
        }

        public ISettings Copy()
        {
            return new ConsoleSettings()
            {
                keyCode = keyCode,
                keyCodes = keyCodes
            };
        }
    }
}