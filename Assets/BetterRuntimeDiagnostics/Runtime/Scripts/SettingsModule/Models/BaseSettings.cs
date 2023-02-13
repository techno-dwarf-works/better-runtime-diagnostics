using System;
using Better.Diagnostics.Runtime.InfoDisplayer.Interfaces;
using Better.Diagnostics.Runtime.SettingsModule.Interfaces;
using UnityEngine;

namespace Better.Diagnostics.Runtime.SettingsModule.Models
{
    [Serializable]
    public abstract class BaseSettings : ISettings
    {
        [SerializeField] private Vector2 position;

        public Vector2 Position => position;
        public void SetPosition(Vector2 rect)
        {
            position = rect;
        }

        public abstract IDebugInfo GetInfo();
    }
}