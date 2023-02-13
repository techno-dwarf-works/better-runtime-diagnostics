using System;
using Better.Diagnostics.Runtime.InfoDisplayer.Interfaces;
using Better.Diagnostics.Runtime.InfoDisplayer.Models;
using UnityEngine;

namespace Better.Diagnostics.Runtime.SettingsModule.Models
{
    [Serializable]
    public class RamUsageSettings : BaseSettings
    {
        [Tooltip("In which interval should the RAM usage be updated?")] [SerializeField]
        private UpdateInterval ramUpdateInterval = 0.5f;

        public override IDebugInfo GetInfo()
        {
            return new RAMUsage(Position, ramUpdateInterval);
        }
    }
}