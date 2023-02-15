using System;
using Better.Diagnostics.Runtime.InfoDisplayer.Interfaces;
using Better.Diagnostics.Runtime.InfoDisplayer.Models;
using Better.Diagnostics.Runtime.SettingsModule.Interfaces;
using UnityEngine;

namespace Better.Diagnostics.Runtime.SettingsModule.Models
{
    [Serializable]
    public class FixedUpdateTrackerSettings : BaseSettings
    {
        [Min(5)] [SerializeField] private int trackerSize = 5;
        
        public override IDebugInfo GetInfo()
        {
            return new FixedUpdateTracker(Position, trackerSize);
        }

        public override ISettings Copy()
        {
            return new FixedUpdateTrackerSettings()
            {
                trackerSize = trackerSize,
                position = position
            };
        }
    }
}