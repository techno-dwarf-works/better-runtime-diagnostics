using System.Collections.Generic;
using Better.Diagnostics.Runtime.Interfaces;
using UnityEngine;

namespace Better.Diagnostics.Runtime.Models
{
    public abstract class CylinderWrapper : IRendererWrapper
    {
        private protected abstract float Radius();
        private protected abstract float Height();
        private protected abstract Matrix4x4 Matrix();

        private protected List<Line> _lines;
        private protected List<Line> _horizontalLines;
        private protected SphereCalculator _calculator;

        public virtual void Initialize()
        {
            _calculator = new SphereCalculator();
            _horizontalLines = _calculator.PrepareHorizontalCircle();
            
            _lines = new List<Line>();
            _lines.Add(new Line(new Vector3(-1, 1, 0), new Vector3(-1, -1, 0)));
            _lines.Add(new Line(new Vector3(1, 1, 0), new Vector3(1, -1, 0)));
            _lines.Add(new Line(new Vector3(0, 1, 1), new Vector3(0, -1, 1)));
            _lines.Add(new Line(new Vector3(0, 1, -1), new Vector3(0, -1, -1)));
        }

        public virtual IEnumerable<Line> GetLines()
        {
            var r = Radius();
            var height = Height();
            var matrix4X4 = Matrix();

            var count = (_horizontalLines.Count * 2) + _lines.Count;
            var lines = new Line[count];

            var b = height / 2f >= r;
            var size = b ? height / 2f - r : 0;
            var up = Vector3.up * size;

            var t = 0;
            for (var i = 0; i < _horizontalLines.Count; i++)
            {
                lines[t] = (_horizontalLines[i] * r + up) * matrix4X4;
                t++;
                lines[t] = (_horizontalLines[i] * r - up) * matrix4X4;
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
    
    public abstract class CapsuleWrapper : CylinderWrapper
    {
        // x = xo + r * cos(phi) * cos(theta)
        // y = yo + r * cos(phi) * sin(theta)
        // z = zo + r * sin(phi)

        private List<Line> _topCapLines;
        private List<Line> _botCapLines;

        public override void Initialize()
        {
            base.Initialize();

            _topCapLines = _calculator.PrepareTopCap();

            _botCapLines = _calculator.PrepareBottomCap();

            _lines = new List<Line>();
            _lines.Add(new Line(new Vector3(-1, 1, 0), new Vector3(-1, -1, 0)));
            _lines.Add(new Line(new Vector3(1, 1, 0), new Vector3(1, -1, 0)));
            _lines.Add(new Line(new Vector3(0, 1, 1), new Vector3(0, -1, 1)));
            _lines.Add(new Line(new Vector3(0, 1, -1), new Vector3(0, -1, -1)));
        }

        public override IEnumerable<Line> GetLines()
        {
            var r = Radius();
            var height = Height();
            var matrix4X4 = Matrix();

            var count = _topCapLines.Count + _botCapLines.Count + _lines.Count + _horizontalLines.Count * 2;
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
            
            for (var i = 0; i < _horizontalLines.Count; i++)
            {
                lines[t] = (_horizontalLines[i] * r + up) * matrix4X4;
                t++;
                lines[t] = (_horizontalLines[i] * r - up) * matrix4X4;
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