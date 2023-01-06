using System.Collections.Generic;
using Better.DataStructures.Ranges;
using UnityEngine;

namespace Better.Diagnostics.Runtime
{
    public abstract class BaseSphereRenderer : IRendererWrapper
    {
        private protected abstract float Radius();
        private protected abstract Matrix4x4 Matrix();

        private static Range<float> ThetaRange = new Range<float>(0, 2f * Mathf.PI);
        private static Range<float> PhiRange = new Range<float>(-Mathf.PI / 2f, Mathf.PI / 2f);

        // x = xo + r * cos(phi) * cos(theta)
        // y = yo + r * cos(phi) * sin(theta)
        // z = zo + r * sin(phi)

        private float _step = 0.2f;

        public IEnumerable<Line> GetLines()
        {
            var r = Radius();
            var lines = new List<Line>();
            var matrix4X4 = Matrix();
            for (var theta = ThetaRange.Min + _step; theta < ThetaRange.Max; theta += _step)
            {
                for (var phi = PhiRange.Min + _step; phi < PhiRange.Max; phi += _step)
                {
                    var start = matrix4X4.MultiplyPoint3x4(CalculatePoint(phi - _step, theta - _step, r));
                    var end = matrix4X4.MultiplyPoint3x4(CalculatePoint(phi, theta, r));
                    lines.Add(new Line(start, end));
                }
            }

            return lines;
        }

        private Vector3 CalculatePoint(float phi, float theta, float r)
        {
            var cosPhi = Mathf.Cos(phi);
            var cosTheta = Mathf.Cos(theta);
            var sinTheta = Mathf.Sin(theta);
            var sinPhi = Mathf.Sin(phi);
            return new Vector3(r * cosPhi * cosTheta, r * cosPhi * sinTheta, r * sinPhi);
        }
    }
}