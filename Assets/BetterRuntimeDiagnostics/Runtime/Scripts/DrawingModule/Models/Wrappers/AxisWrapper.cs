using System.Collections.Generic;
using Better.Diagnostics.Runtime.DrawingModule.Interfaces;
using UnityEngine;

namespace Better.Diagnostics.Runtime.DrawingModule
{
    public class AxisWrapper : BaseWrapper<float>
    {
        public AxisWrapper(ITrackableData<float> data) : base(data)
        {
        }
        
        private protected override List<Line> GenerateBaseLines(Color color)
        {
            var points = new List<Line>(3);

            points.Add(new Line(Vector3.zero, Vector3.forward, Color.blue));
            points.Add(new Line(Vector3.zero, Vector3.right, Color.red));
            points.Add(new Line(Vector3.zero, Vector3.up, Color.green));

            return points;
        }

        public override IEnumerable<Line> GetLines()
        {
            var baseLines = GetBaseLines();
            var lines = new Line[baseLines.Count];
            var matrix4X4 = _data.Matrix4X4;
            for (int i = 0; i < baseLines.Count; i++)
            {
                lines[i] = baseLines[i] * matrix4X4;
            }

            return lines;
        }
    }
}