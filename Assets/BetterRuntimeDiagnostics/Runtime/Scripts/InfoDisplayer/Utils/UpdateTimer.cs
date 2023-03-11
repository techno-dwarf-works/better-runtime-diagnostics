using System;
using UnityEngine;

namespace Better.Diagnostics.Runtime.InfoDisplayer.Utils
{
    public class UpdateTimer
    {
        private readonly UpdateInterval _updateInterval;
        private readonly Action _onTimerElapsed;
        private float _lastUpdate;


        public UpdateTimer(UpdateInterval updateInterval, Action onTimerElapsed)
        {
            _updateInterval = updateInterval;
            _onTimerElapsed = onTimerElapsed;
        }

        public void Update()
        {
            if (!(Time.unscaledTime > _lastUpdate)) return;
            _lastUpdate += _updateInterval.Interval;
            _onTimerElapsed.Invoke();
        }
    }
}