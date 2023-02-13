using System;
using Better.Diagnostics.Runtime.InfoDisplayer.Interfaces;
using Better.Diagnostics.Runtime.InfoDisplayer.Models;
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
    }
}