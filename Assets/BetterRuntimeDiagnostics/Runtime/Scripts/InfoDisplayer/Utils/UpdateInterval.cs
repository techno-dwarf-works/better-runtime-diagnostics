using System;
using UnityEngine;

namespace Better.Diagnostics.Runtime.InfoDisplayer.Utils
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

        public UpdateInterval()
        {
            
        }

        public UpdateInterval(float value)
        {
            updateInterval = value;
        }

        public UpdateInterval Copy()
        {
            return new UpdateInterval(updateInterval);
        }
        
        public static implicit operator UpdateInterval(float value)
        {
            return new UpdateInterval(value);
        }
    }
}