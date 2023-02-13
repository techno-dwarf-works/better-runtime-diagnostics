using System.Collections.Generic;
using System.Linq;
using Better.Diagnostics.Runtime.InfoDisplayer.Interfaces;
using Better.Diagnostics.Runtime.SettingsModule.Interfaces;
using UnityEngine;

namespace Better.Diagnostics.Runtime.SettingsModule
{
    [CreateAssetMenu(menuName = "Better/Diagnostic/Create Settings", fileName = "DiagnosticSettings", order = 0)]
    public class DiagnosticSettings : ScriptableObject
    {
        [ColorUsage(false, false)] [SerializeField]
        private Color defaultRenderColor = Color.red;

        [SerializeReference] private List<ISettings> settings;

        public List<ISettings> GetInstances()
        {
            return new List<ISettings>(settings);
        }

        public List<IDebugInfo> SetUpInfos()
        {
            return settings.Select(x => x.GetInfo()).ToList();
        }

        public void Remove(ISettings settingsInstance)
        {
            settings.Remove(settingsInstance);
        }

        public void Add(ISettings settingsInstance)
        {
            settings.Add(settingsInstance);
        }
    }
}