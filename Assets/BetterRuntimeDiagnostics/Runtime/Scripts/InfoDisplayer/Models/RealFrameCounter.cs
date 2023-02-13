using System.Text;
using UnityEngine;

namespace Better.Diagnostics.Runtime.InfoDisplayer.Models
{
    public class RealFrameCounter : BaseFrameCounter
    {
        public RealFrameCounter(Vector2 position, UpdateInterval updateInterval) : base(position, updateInterval)
        {
        }

        private protected override string ContentText(float fps)
        {
            return $"Real FPS {fps.ToString("F1")}";
        }

        private protected override float DisplayFPS()
        {
            return 1f / Time.unscaledDeltaTime;
        }
    }
}