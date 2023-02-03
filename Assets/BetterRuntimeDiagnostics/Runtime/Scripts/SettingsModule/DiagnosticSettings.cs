using System.Collections.Generic;
using Better.Diagnostics.Runtime.PerformanceAnalyzer;
using UnityEngine;

namespace Better.Diagnostics.Runtime.SettingsModule
{
    [CreateAssetMenu(menuName = "Better/Diagnostic/Create Settings", fileName = "DiagnosticSettings", order = 0)]
    public class DiagnosticSettings : ScriptableObject
    {
        [ColorUsage(false, false)] [SerializeField]
        private Color defaultRenderColor = Color.red;

        [Header("Frame Settings")] 
        [SerializeField] private bool displayFrameCount;
        
        [Header("CPU Settings")] 
        [SerializeField] private bool displayCPUUsage;

        [Tooltip("In which interval should the CPU usage be updated?")] 
        [Range(0.5f, 60f)] [SerializeField] private float updateInterval = 1f;

        public List<IDebugInfo> SetUpInfos()
        {
            var infos = new List<IDebugInfo>();

            if (displayCPUUsage)
            {
                infos.Add(new CPUUsage(updateInterval));
            }

            if (displayFrameCount)
            {
                infos.Add(new FrameCounter());
            }

            return infos;
        }
    }
}