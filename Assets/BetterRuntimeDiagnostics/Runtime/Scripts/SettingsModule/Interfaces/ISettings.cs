﻿using Better.Diagnostics.Runtime.InfoDisplayer.Interfaces;
using Better.Diagnostics.Runtime.NodeModule;
using UnityEngine;

namespace Better.Diagnostics.Runtime.SettingsModule.Interfaces
{
    public interface ISettings : INodeRect
    {
        public IDebugInfo GetInfo();
        public ISettings Copy();
    }
}