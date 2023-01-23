using System.Collections.Generic;
using Better.Diagnostics.Runtime.Interfaces;

namespace Better.Diagnostics.Runtime.Models
{
    public class SphereWrapper : BaseWrapper<float>
    {
        public SphereWrapper(ITrackableData<float> data) : base(data)
        {
        }

        private protected override List<Line> GenerateBaseLines()
        {
            var sphereCalculator = new SphereCalculator();
            var lines = sphereCalculator.PrepareHorizontalCircle();
            var thetaRangeMin = SphereCalculator.ThetaRange.Min + SphereCalculator.Step;
            var rangeMax = SphereCalculator.ThetaRange.Max + SphereCalculator.Step;
            var phiRangeMin = SphereCalculator.PhiRange.Min;

            for (var theta = thetaRangeMin; theta <= rangeMax; theta += SphereCalculator.Step)
            {
                var previousStep = theta - SphereCalculator.Step;

                var start = SphereCalculator.Spherical(0, previousStep);
                var end = SphereCalculator.Spherical(0, theta);
                lines.Add(new Line(start, end));

                var start1 = SphereCalculator.Spherical(phiRangeMin, previousStep);
                var end1 = SphereCalculator.Spherical(phiRangeMin, theta);
                lines.Add(new Line(start1, end1));
            }

            return lines;
        }

        public override IEnumerable<Line> GetLines()
        {
            var r = _data.Size;
            var lines = new Line[_lines.Count];
            var matrix4X4 = _data.Matrix4X4;

            for (var i = 0; i < _lines.Count; i++)
            {
                lines[i] = _lines[i] * r * matrix4X4;
            }

            return lines;
        }
    }
}