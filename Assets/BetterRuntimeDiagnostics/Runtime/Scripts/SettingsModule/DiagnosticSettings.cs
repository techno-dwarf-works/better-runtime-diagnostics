using System.Collections.Generic;
using Better.Diagnostics.Runtime.InfoDisplayer.Interfaces;
using Better.Diagnostics.Runtime.InfoDisplayer.Models;
using UnityEngine;

namespace Better.Diagnostics.Runtime.SettingsModule
{
    [CreateAssetMenu(menuName = "Better/Diagnostic/Create Settings", fileName = "DiagnosticSettings", order = 0)]
    public class DiagnosticSettings : ScriptableObject
    {
        [ColorUsage(false, false)] 
        [SerializeField]
        private Color defaultRenderColor = Color.red;

        [Header("Frame Settings")] 
        [SerializeField]
        private bool displayFrameCount = true;

        [Tooltip("In which interval should the FPS usage be updated?")] 
        [SerializeField]
        private UpdateInterval fpsUpdateInterval = 1f;

        [Header("CPU Settings")] 
        [SerializeField]
        private bool displayCPUUsage = true;

        [Tooltip("In which interval should the CPU usage be updated?")] 
        [SerializeField]
        private UpdateInterval cpuUpdateInterval = 0.5f;

        [Header("RAM Settings")] 
        [SerializeField]
        private bool displayRAMUsage = true;

        [Tooltip("In which interval should the CPU usage be updated?")] 
        [SerializeField]
        private UpdateInterval ramUpdateInterval = 0.5f;

        [Header("Rendering Settings")] 
        [SerializeField]
        private bool displayRenderingUsage = true;

        [Tooltip("In which interval should the CPU usage be updated?")] 
        [SerializeField]
        private UpdateInterval renderingUpdateInterval = 1.5f;

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

            if (displayRenderingUsage)
            {
                infos.Add(new RenderingCounters(renderingUpdateInterval));
            }

            return infos;
        }
    }
}