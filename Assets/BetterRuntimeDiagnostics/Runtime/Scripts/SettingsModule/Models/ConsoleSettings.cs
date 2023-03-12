using System;
using Better.Diagnostics.Runtime.InfoDisplayer.Interfaces;
using Better.Diagnostics.Runtime.InfoDisplayer.Models;
using Better.Diagnostics.Runtime.SettingsModule.Interfaces;
using UnityEngine;

namespace Better.Diagnostics.Runtime.SettingsModule.Models
{
    [Serializable]
    public class ConsoleSettings : ISettings
    {
        [SerializeField] private KeyCode keyCode = KeyCode.BackQuote;

        public IDebugInfo GetInfo()
        {
            return new DebugConsole(keyCode);
        }

        public ISettings Copy()
        {
            return new ConsoleSettings()
            {
                keyCode = keyCode
            };
        }
    }
}