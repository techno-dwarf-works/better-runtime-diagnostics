using System;
using Better.Diagnostics.Runtime.InfoDisplayer.Interfaces;
using Better.Diagnostics.Runtime.SettingsModule.Interfaces;
using UnityEngine;

namespace Better.Diagnostics.Runtime.SettingsModule.Models
{
    [Serializable]
    public abstract class BaseSettings : ISettings
    {
        [SerializeField] private protected Rect position;

        public Rect Position => position;
        public void SetPosition(Rect rect)
        {
            position = rect;
        }

        public abstract IDebugInfo GetInfo();
        public abstract ISettings Copy();
    }
}