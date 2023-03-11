using System;
using System.Diagnostics;
using System.Threading;
using Better.Diagnostics.Runtime.InfoDisplayer.Interfaces;
using Better.Diagnostics.Runtime.InfoDisplayer.Utils;
using UnityEngine;
using UnityEngine.UIElements;

//TODO: Implement own process get https://answers.unity.com/questions/1792187/systemdiagnosticsprocessgetprocesses-is-not-workin.html

namespace Better.Diagnostics.Runtime.InfoDisplayer.Models
{
    public class CPUUsage : IDebugInfo, IUpdateableInfo
    {
        private int _processorCount;
        private float _cpuTime;

        private Thread _cpuThread;
        private float _lasCpuUsage;
        private readonly UpdateInterval _updateInterval;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly Label _label;
        private readonly UpdateTimer _updateTimer;

        public CPUUsage(Rect position, UpdateInterval updateInterval)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _updateInterval = updateInterval;
            _updateTimer = new UpdateTimer(updateInterval.Interval + 0.1f, OnUpdate);
            _label = VisualElementsFactory.CreateElement<Label>(position, Color.white);
        }

        public void Initialize(UIDocument uiDocument)
        {
            uiDocument.rootVisualElement.Add(_label);
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

        private void OnUpdate()
        {
            _label.text = _cpuTime.ToString("F1") + "ms";
        }

        public void Deconstruct()
        {
            // Just to be sure kill the thread if this object is destroyed
            _cancellationTokenSource.Cancel(false);
            _cpuThread?.Abort();
        }

        public void Update()
        {
            _updateTimer?.Update();
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

                    var allProcesses = Process.GetCurrentProcess();
                    var currCPUPc = allProcesses.TotalProcessorTime;

                    var newCPUTime = currCPUPc - lastCpuTime;
                    _cpuTime = (float)(100f * newCPUTime.TotalSeconds / (_updateInterval.Interval * _processorCount));

                    Thread.Sleep(Mathf.RoundToInt(_updateInterval.Interval * 1000));

                    lastCpuTime = currCPUPc;
                }
            }
        }
    }
}