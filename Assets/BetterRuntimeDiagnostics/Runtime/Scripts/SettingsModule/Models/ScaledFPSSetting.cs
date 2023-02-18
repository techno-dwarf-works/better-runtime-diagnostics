﻿using System;
using Better.Diagnostics.Runtime.InfoDisplayer.Interfaces;
using Better.Diagnostics.Runtime.InfoDisplayer.Models;
using Better.Diagnostics.Runtime.SettingsModule.Interfaces;
using UnityEngine;

namespace Better.Diagnostics.Runtime.SettingsModule.Models
{
    [Serializable]
    public class ScaledFPSSetting : BaseSettings
    {
        [Tooltip("In which interval should the FPS usage be updated?")] [SerializeField]
        private UpdateInterval fpsUpdateInterval = 1f;

        public override IDebugInfo GetInfo()
        {
            return new ScaledFrameCounter(Position, fpsUpdateInterval);
        }
        
        public override ISettings Copy()
        {
            return new ScaledFPSSetting()
            {
                fpsUpdateInterval = fpsUpdateInterval.Copy(),
                position = position
            };
        }
    }
}