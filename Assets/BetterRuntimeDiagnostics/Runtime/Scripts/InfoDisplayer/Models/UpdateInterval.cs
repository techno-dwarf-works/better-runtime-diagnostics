using System;
using UnityEngine;

namespace Better.Diagnostics.Runtime.InfoDisplayer.Models
{
    [Serializable]
    public class UpdateInterval
    {
        [Range(0f, 10f)]
        [SerializeField] private float updateInterval;

        public float Interval => updateInterval;

        public void SetInterval(float interval)
        {
            updateInterval = interval;
        }

        public static implicit operator UpdateInterval(float value)
        {
            var interval = new UpdateInterval();
            interval.SetInterval(value);
            return interval;
        }
    }
}