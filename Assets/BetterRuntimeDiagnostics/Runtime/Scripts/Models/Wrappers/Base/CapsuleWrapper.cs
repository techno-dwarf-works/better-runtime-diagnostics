using System.Collections.Generic;
using Better.Diagnostics.Runtime.Interfaces;
using UnityEngine;

namespace Better.Diagnostics.Runtime.Models
{
    public abstract class CapsuleWrapper : IRendererWrapper
    {
        private protected abstract float Radius();
        private protected abstract float Height();
        private protected abstract Matrix4x4 Matrix();

        // x = xo + r * cos(phi) * cos(theta)
        // y = yo + r * cos(phi) * sin(theta)
        // z = zo + r * sin(phi)

        private List<Line> _topCapLines;
        private List<Line> _botCapLines;
        private List<Line> _lines;

        public void Initialize()
        {
            var calculator = new SphereCalculator();
            var horizontal = calculator.PrepareHorizontalCircle();

            _topCapLines = calculator.PrepareTopCap();
            _topCapLines.AddRange(horizontal);

            _botCapLines = calculator.PrepareBottomCap();
            _botCapLines.AddRange(horizontal);

            _lines = new List<Line>();
            _lines.Add(new Line(new Vector3(-1, 1, 0), new Vector3(-1, -1, 0)));
            _lines.Add(new Line(new Vector3(1, 1, 0), new Vector3(1, -1, 0)));
            _lines.Add(new Line(new Vector3(0, 1, 1), new Vector3(0, -1, 1)));
            _lines.Add(new Line(new Vector3(0, 1, -1), new Vector3(0, -1, -1)));
        }

        public IEnumerable<Line> GetLines()
        {
            var r = Radius();
            var height = Height();
            var matrix4X4 = Matrix();

            var count = _topCapLines.Count + _botCapLines.Count + _lines.Count;
            var lines = new Line[count];

            var b = height / 2f >= r;
            var size = b ? height / 2f - r : 0;
            var up = Vector3.up * size;

            var t = 0;
            for (var i = 0; i < _topCapLines.Count; i++)
            {
                lines[t] = (_topCapLines[i] * r + up) * matrix4X4;
                t++;
            }

            for (var i = 0; i < _botCapLines.Count; i++)
            {
                lines[t] = (_botCapLines[i] * r - up) * matrix4X4;
                t++;
            }

            for (var i = 0; i < _lines.Count; i++)
            {
                lines[t] = _lines[i].Multiply(r, size, r) * matrix4X4;
                t++;
            }

            return lines;
        }
    }
}