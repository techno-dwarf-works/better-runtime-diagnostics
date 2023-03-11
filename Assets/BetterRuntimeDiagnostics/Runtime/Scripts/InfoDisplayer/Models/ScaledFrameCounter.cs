using Better.Diagnostics.Runtime.InfoDisplayer.Utils;
using UnityEngine;

namespace Better.Diagnostics.Runtime.InfoDisplayer.Models
{
    public class ScaledFrameCounter : BaseFrameCounter
    {
        public ScaledFrameCounter(Rect position, UpdateInterval updateInterval) : base(position, updateInterval)
        {
        }


        private protected override string ContentText(float fps)
        {
            return $"Scaled FPS {fps.ToString("F1")}";
        }

        private int _frameCount;
        private float _start;

        public override void Update()
        {
            if (_frameCount <= 0)
            {
                _start = Time.unscaledTime;
            }
            _frameCount++;
            base.Update();
        }

        private protected override float DisplayFPS()
        {
            var time = Time.unscaledTime - _start;
            var unscaledTime = (_frameCount / time) * Time.timeScale;
            _frameCount = 0;
            return unscaledTime;
        }
    }
}