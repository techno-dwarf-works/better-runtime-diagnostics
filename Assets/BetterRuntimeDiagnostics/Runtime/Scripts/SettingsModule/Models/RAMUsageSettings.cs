using System;
using Better.Diagnostics.Runtime.InfoDisplayer.Interfaces;
using Better.Diagnostics.Runtime.InfoDisplayer.Models;
using Better.Diagnostics.Runtime.InfoDisplayer.Utils;
using Better.Diagnostics.Runtime.SettingsModule.Interfaces;
using UnityEngine;

namespace Better.Diagnostics.Runtime.SettingsModule.Models
{
    [Serializable]
    public class RAMUsageSettings : BaseSettings
    {
        [Tooltip("In which interval should the RAM usage be updated?")] [SerializeField]
        private UpdateInterval ramUpdateInterval = 0.5f;

        public override IDebugInfo GetInfo()
        {
            return new RAMUsage(Position, ramUpdateInterval);
        }
        
        public override ISettings Copy()
        {
            return new RAMUsageSettings()
            {
                ramUpdateInterval = ramUpdateInterval.Copy(),
                position = position
            };
        }
    }
}