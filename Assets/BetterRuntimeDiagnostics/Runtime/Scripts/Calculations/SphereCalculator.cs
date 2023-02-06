using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Better.DataStructures.Ranges;
using Better.Diagnostics.Runtime.DrawingModule;
using UnityEngine;

namespace Better.Diagnostics.Runtime.Calculations
{
    public struct SphereCalculator
    {
        public static Range<float> ThetaRange { get; } = new Range<float>(0, 2f * Mathf.PI);
        public static Range<float> PhiRange { get; } = new Range<float>(-Mathf.PI / 2f, Mathf.PI / 2f);
        public const float Step = 0.1f;

        // x = xo + r * cos(phi) * cos(theta)
        // y = yo + r * cos(phi) * sin(theta)
        // z = zo + r * sin(phi)

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public List<Line> PrepareHorizontalCircle(Color color)
        {
            var lines = new List<Line>();

            var thetaRangeMax = ThetaRange.Max / 2f;
            var phiRangeMax = PhiRange.Max + Step;
            var thetaRange = ThetaRange.Min;

            for (var phi = PhiRange.Min + Step; phi <= phiRangeMax; phi += Step)
            {
                var f = phi - Step;
                var start = CalculatePoint(f, thetaRange);
                var end = CalculatePoint(phi, thetaRange);

                lines.Add(RemovablePool.Instance.Get<Line, Vector3, Vector3, Color>(start, end, color));

                var start1 = CalculatePoint(f, thetaRangeMax);
                var end1 = CalculatePoint(phi, thetaRangeMax);
                lines.Add(RemovablePool.Instance.Get<Line, Vector3, Vector3, Color>(start1, end1, color));
            }

            return lines;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public List<Line> PrepareTopCap(Color color)
        {
            var lines = new List<Line>();
            var rangeMax = (ThetaRange.Max / 4f) + Step;
            for (var theta = -rangeMax; theta <= rangeMax; theta += Step)
            {
                var previousStep = theta - Step;
                var start = Spherical(0, previousStep);
                var end = Spherical(0, theta);
                lines.Add(RemovablePool.Instance.Get<Line, Vector3, Vector3, Color>(start, end, color));

                var start1 = Spherical(PhiRange.Min, previousStep);
                var end1 = Spherical(PhiRange.Min, theta);
                lines.Add(RemovablePool.Instance.Get<Line, Vector3, Vector3, Color>(start1, end1, color));
            }

            return lines;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public List<Line> PrepareBottomCap(Color color)
        {
            var lines = new List<Line>();
            var thetaStart = (ThetaRange.Max / 4f) + Step;
            var thetaEnd = thetaStart + Mathf.PI;
            for (var theta = thetaStart; theta <= thetaEnd; theta += Step)
            {
                var previousStep = theta - Step;
                var start = Spherical(0, previousStep);
                var end = Spherical(0, theta);
                lines.Add(RemovablePool.Instance.Get<Line, Vector3, Vector3, Color>(start, end, color));

                var start1 = Spherical(PhiRange.Min, previousStep);
                var end1 = Spherical(PhiRange.Min, theta);
                lines.Add(RemovablePool.Instance.Get<Line, Vector3, Vector3, Color>(start1, end1, color));
            }

            return lines;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Spherical(float phi, float theta)
        {
            var sinTheta = Mathf.Sin(theta);
            var cosTheta = Mathf.Cos(theta);
            var sinPhi = Mathf.Sin(phi);
            var cosPhi = Mathf.Cos(phi);
            return new Vector3(sinTheta * cosPhi, cosTheta, -sinTheta * sinPhi);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 CalculatePoint(float phi, float theta)
        {
            var cosPhi = Mathf.Cos(phi);
            var cosTheta = Mathf.Cos(theta);
            var sinTheta = Mathf.Sin(theta);
            var sinPhi = Mathf.Sin(phi);
            return new Vector3(cosPhi * cosTheta, cosPhi * sinTheta, sinPhi);
        }
    }
}