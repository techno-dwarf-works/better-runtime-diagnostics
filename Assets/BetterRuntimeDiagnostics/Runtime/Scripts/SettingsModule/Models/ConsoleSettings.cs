using System;
using Better.Diagnostics.Runtime.InfoDisplayer.Interfaces;
using Better.Diagnostics.Runtime.SettingsModule.Interfaces;

namespace Better.Diagnostics.Runtime.SettingsModule.Models
{
    [Serializable]
    public class ConsoleSettings : ISettings
    {
        public IDebugInfo GetInfo()
        {
            throw new NotImplementedException();
        }

        public ISettings Copy()
        {
            throw new NotImplementedException();
        }
    }
}