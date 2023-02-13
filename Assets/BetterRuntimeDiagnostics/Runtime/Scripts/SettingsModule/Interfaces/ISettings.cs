using Better.Diagnostics.Runtime.InfoDisplayer.Interfaces;
using UnityEngine;

namespace Better.Diagnostics.Runtime.SettingsModule.Interfaces
{
    public interface ISettings
    {
        public Vector2 Position { get; }
        public void SetPosition(Vector2 rect);
        public IDebugInfo GetInfo();
    }
}