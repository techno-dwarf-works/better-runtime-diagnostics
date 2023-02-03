using System;
//TODO: Implement own process get https://answers.unity.com/questions/1792187/systemdiagnosticsprocessgetprocesses-is-not-workin.html
using System.Diagnostics;
using System.Threading;
using UnityEngine;

namespace Better.Diagnostics.Runtime.PerformanceAnalyzer
{
    public class CPUUsage : IDebugInfo
    {
        private int _processorCount;
        private float _cpuTime;

        private Thread _cpuThread;
        private float _lasCpuUsage;
        private float _updateInterval;
        private readonly CancellationTokenSource _cancellationTokenSource;

        public CPUUsage(float updateInterval)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _updateInterval = updateInterval;
        }

        public void SetInterval(float updateInterval)
        {
            _updateInterval = Mathf.Clamp(1f, 60f, updateInterval);
        }

        public void Initialize()
        {
            _processorCount = Environment.ProcessorCount;
            _cpuThread = new Thread(UpdateCPUUsage)
            {
                IsBackground = true,
                // we don't want that our measurement thread
                // steals performance
                Priority = System.Threading.ThreadPriority.BelowNormal
            };

            // start the cpu usage thread
            _cpuThread.Start(_cancellationTokenSource.Token);
        }

        public void OnGUI()
        {
            GUILayout.Label(new GUIContent(_cpuTime.ToString("F1") + "ms"));
        }

        public void Deconstruct()
        {
            // Just to be sure kill the thread if this object is destroyed
            _cancellationTokenSource.Cancel(false);
            _cpuThread?.Abort();
        }


        /// <summary>
        /// Runs in Thread
        /// </summary>
        private void UpdateCPUUsage(object obj)
        {
            if (obj is CancellationToken cancellationToken)
            {
                var lastCpuTime = new TimeSpan(0);

                // This is ok since this is executed in a background thread
                while (!cancellationToken.IsCancellationRequested)
                {
                    // Get a list of all running processes in this PC
                    // lastCpuTime = LastCpuTime(lastCpuTime);
                    // Wait for UpdateInterval
                    
                    var currCPUPc = new TimeSpan(0);
                    Process[] allProcesses = Process.GetProcesses();
                    for (int cnt = 0; cnt < allProcesses.Length; cnt++)
                        currCPUPc += allProcesses[cnt].TotalProcessorTime;
 
                    TimeSpan newCPUTime = currCPUPc - lastCpuTime;
                    _cpuTime = (int)((100 * newCPUTime.TotalSeconds / _updateInterval) / Environment.ProcessorCount);
                    
                    Thread.Sleep(Mathf.RoundToInt(_updateInterval * 1000));
                    
                    lastCpuTime = currCPUPc;
                }
            }
        }
    }
}