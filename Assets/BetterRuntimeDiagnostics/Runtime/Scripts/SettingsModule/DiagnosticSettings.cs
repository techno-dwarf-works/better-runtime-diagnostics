using System.Collections.Generic;
using Better.DataStructures.Ranges;
using Better.Diagnostics.Runtime.PerformanceAnalyzer;
using UnityEngine;

namespace Better.Diagnostics.Runtime.SettingsModule
{
    [CreateAssetMenu(menuName = "Better/Diagnostic/Create Settings", fileName = "DiagnosticSettings", order = 0)]
    public class DiagnosticSettings : ScriptableObject
    {
        [ColorUsage(false, false)] [SerializeField]
        private Color defaultRenderColor = Color.red;

        [Header("Frame Settings")] [SerializeField]
        private bool displayFrameCount;

        [Tooltip("In which interval should the FPS usage be updated?")] 
        [Range(0.1f, 10f)] [SerializeField]
        private float fpsUpdateInterval = 1f;

        [Header("CPU Settings")] [SerializeField]
        private bool displayCPUUsage;

        [Tooltip("In which interval should the CPU usage be updated?")] 
        [Range(0.1f, 10f)] [SerializeField]
        private float cpuUpdateInterval = 0.5f;
        
        [Header("RAM Settings")] [SerializeField]
        private bool displayRAMUsage;

        [Tooltip("In which interval should the CPU usage be updated?")] 
        [Range(0.1f, 10f)] [SerializeField]
        private float ramUpdateInterval = 0.5f;

        public List<IDebugInfo> SetUpInfos()
        {
            var infos = new List<IDebugInfo>();

            if (displayCPUUsage)
            {
                infos.Add(new CPUUsage(cpuUpdateInterval));
            }

            if (displayFrameCount)
            {
                infos.Add(new FrameCounter(fpsUpdateInterval));
            }

            if (displayRAMUsage)
            {
                infos.Add(new RAMUsage(ramUpdateInterval));
            }

            return infos;
        }
    }
}