using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Better.Diagnostics.Runtime.DrawingModule.Interfaces;
using UnityEngine;

namespace Better.Diagnostics.Runtime.DrawingModule
{
    public class CapsuleWrapper : CylinderWrapper
    {
        // x = xo + r * cos(phi) * cos(theta)
        // y = yo + r * cos(phi) * sin(theta)
        // z = zo + r * sin(phi)

        private List<Line> _topCapLines;
        private List<Line> _botCapLines;

        public CapsuleWrapper(ITrackableData<float> data) : base(data)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            _topCapLines = _calculator.PrepareTopCap();

            _botCapLines = _calculator.PrepareBottomCap();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private protected override int Lenght()
        {
            return base.Lenght() + _botCapLines.Count + _topCapLines.Count;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private protected override void TransformLines(Line[] lines, ref int index, float size, float radius, Vector3 up, Matrix4x4 matrix4X4)
        {
            for (var i = 0; i < _topCapLines.Count; i++)
            {
                lines[index] = (_topCapLines[i] * radius + up) * matrix4X4;
                index++;
            }

            for (var i = 0; i < _botCapLines.Count; i++)
            {
                lines[index] = (_botCapLines[i] * radius - up) * matrix4X4;
                index++;
            }
            base.TransformLines(lines, ref index, size, radius, up, matrix4X4);
        }
    }
}