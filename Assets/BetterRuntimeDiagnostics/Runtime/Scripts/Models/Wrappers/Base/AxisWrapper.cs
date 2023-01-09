using System;
using System.Collections.Generic;
using UnityEngine;

namespace Better.Diagnostics.Runtime.Models
{
    public abstract class AxisWrapper : BaseWrapper
    {
        private protected override List<Line> GenerateBaseLines()
        {
            var points = new List<Line>(3);

            points.Add(new Line(Vector3.zero, Vector3.forward, Color.blue));
            points.Add(new Line(Vector3.zero, Vector3.right, Color.red));
            points.Add(new Line(Vector3.zero, Vector3.up, Color.green));

            return points;
        }

        public override IEnumerable<Line> GetLines()
        {
            var lines = new Line[_lines.Count];
            var matrix4X4 = Matrix();
            for (int i = 0; i < _lines.Count; i++)
            {
                lines[i] = _lines[i] * matrix4X4;
            }

            return lines;
        }
    }
}