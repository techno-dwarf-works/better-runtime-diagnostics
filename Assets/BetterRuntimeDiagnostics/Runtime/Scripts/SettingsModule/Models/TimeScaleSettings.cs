using System;
using Better.Diagnostics.Runtime.InfoDisplayer.Interfaces;
using Better.Diagnostics.Runtime.InfoDisplayer.Models;
using Better.Diagnostics.Runtime.SettingsModule.Interfaces;

namespace Better.Diagnostics.Runtime.SettingsModule.Models
{
    [Serializable]
    public class TimeScaleSettings : BaseSettings
    {
        public override IDebugInfo GetInfo()
        {
            return new TimeSettings(Position);
        }
        
        public override ISettings Copy()
        {
            return new TimeScaleSettings()
            {
                position = position
            };
        }
    }
}