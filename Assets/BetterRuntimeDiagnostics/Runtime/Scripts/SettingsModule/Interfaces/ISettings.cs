using Better.Diagnostics.Runtime.InfoDisplayer.Interfaces;
using UnityEngine;

namespace Better.Diagnostics.Runtime.SettingsModule.Interfaces
{
    public interface ISettings
    {
        public Rect Position { get; }
        public void SetPosition(Rect rect);
        public IDebugInfo GetInfo();

        public ISettings Copy();
    }
}