using System;
using Better.Diagnostics.Runtime.InfoDisplayer.Interfaces;
using Better.Diagnostics.Runtime.InfoDisplayer.Models;
using UnityEngine;

namespace Better.Diagnostics.Runtime.SettingsModule.Models
{
    [Serializable]
    public class RenderingCountersSettings : BaseSettings
    {
        [Tooltip("In which interval should the counters usage be updated?")] [SerializeField]
        private UpdateInterval renderingUpdateInterval = 0.5f;

        public override IDebugInfo GetInfo()
        {
            return new RenderingCounters(Position, renderingUpdateInterval);
        }
    }
}