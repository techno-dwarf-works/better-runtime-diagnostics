using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Better.Diagnostics.Runtime.Calculations;
using Better.Diagnostics.Runtime.Interfaces;
using UnityEngine;

namespace Better.Diagnostics.Runtime.Models
{
    public class CylinderWrapper : IRendererWrapper
    {
        private Line[] _lines;
        private List<Line> _horizontalLines;
        private protected SphereCalculator _calculator;
        private readonly ITrackableData<float> _data;

        public bool IsMarkedForRemove => _data.IsMarkedForRemove;

        public CylinderWrapper(ITrackableData<float> data)
        {
            _data = data;
        }

        public void MarkForRemove()
        {
            _data.MarkForRemove();
        }

        public virtual void Initialize()
        {
            _calculator = new SphereCalculator();
            _horizontalLines = _calculator.PrepareHorizontalCircle();

            _lines = new Line[4];
            _lines[0] = new Line(new Vector3(-1, 1, 0), new Vector3(-1, -1, 0));
            _lines[1] = new Line(new Vector3(1, 1, 0), new Vector3(1, -1, 0));
            _lines[2] = new Line(new Vector3(0, 1, 1), new Vector3(0, -1, 1));
            _lines[3] = new Line(new Vector3(0, 1, -1), new Vector3(0, -1, -1));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private protected virtual int Lenght()
        {
            return _horizontalLines.Count * 2 + _lines.Length;
        }

        public virtual IEnumerable<Line> GetLines()
        {
            var r = _data.Size;
            var height = _data.OptionalSize;
            var matrix4X4 = _data.Matrix4X4;

            var count = Lenght();
            var lines = new Line[count];

            var b = height / 2f >= r;
            var size = b ? height / 2f - r : 0;
            var up = Vector3.up * size;

            var t = 0;
            TransformLines(lines, ref t, size, r, up, matrix4X4);

            return lines;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private protected virtual void TransformLines(Line[] lines, ref int index, float size, float radius, Vector3 up, Matrix4x4 matrix4X4)
        {
            for (var i = 0; i < _horizontalLines.Count; i++)
            {
                lines[index] = (_horizontalLines[i] * radius + up) * matrix4X4;
                index++;
                lines[index] = (_horizontalLines[i] * radius - up) * matrix4X4;
                index++;
            }

            for (var i = 0; i < _lines.Length; i++)
            {
                lines[index] = _lines[i].Multiply(radius, size, radius) * matrix4X4;
                index++;
            }
        }
    }
}