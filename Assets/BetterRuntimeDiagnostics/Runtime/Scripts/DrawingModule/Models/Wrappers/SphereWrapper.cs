using System.Collections.Generic;
using Better.Diagnostics.Runtime.Calculations;
using Better.Diagnostics.Runtime.DrawingModule.Interfaces;
using UnityEngine;

namespace Better.Diagnostics.Runtime.DrawingModule
{
    public class SphereWrapper : BaseWrapper<float>
    {
        public override void OnRemoved()
        {
            RemovablePool.Instance.Add(this);
            _data.OnRemoved();
            base.OnRemoved();
        }

        private protected override List<Line> GenerateBaseLines(Color color)
        {
            var sphereCalculator = new SphereCalculator();
            var lines = sphereCalculator.PrepareHorizontalCircle(_data.Color);
            var thetaRangeMin = SphereCalculator.ThetaRange.Min + SphereCalculator.Step;
            var rangeMax = SphereCalculator.ThetaRange.Max + SphereCalculator.Step;
            var phiRangeMin = SphereCalculator.PhiRange.Min;

            for (var theta = thetaRangeMin; theta <= rangeMax; theta += SphereCalculator.Step)
            {
                var previousStep = theta - SphereCalculator.Step;

                var start = SphereCalculator.Spherical(0, previousStep);
                var end = SphereCalculator.Spherical(0, theta);
                lines.Add(RemovablePool.Instance.Get<Line, Vector3, Vector3, Color>(start, end, color));

                var start1 = SphereCalculator.Spherical(phiRangeMin, previousStep);
                var end1 = SphereCalculator.Spherical(phiRangeMin, theta);
                lines.Add(RemovablePool.Instance.Get<Line, Vector3, Vector3, Color>(start1, end1, color));
            }

            return lines;
        }

        public override IEnumerable<Line> GetLines()
        {
            var r = _data.Size;
            var baseLines = GetBaseLines();
            var lines = new Line[baseLines.Count];
            var matrix4X4 = _data.Matrix4X4;

            for (var i = 0; i < baseLines.Count; i++)
            {
                var copy = baseLines[i].Copy();
                lines[i] = copy * r * matrix4X4;
            }

            return lines;
        }
    }
}