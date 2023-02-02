using System.Collections.Generic;
using Better.Diagnostics.Runtime.Interfaces;
using UnityEngine;

namespace Better.Diagnostics.Runtime.Models
{
    public class GenericLineWrapper : BaseWrapper<float>
    {
        public GenericLineWrapper(ITrackableData<float> data) : base(data)
        {
        }

        private protected override List<Line> GenerateBaseLines(Color color)
        {
            var points = new List<Line>(3)
            {
                new Line(Vector3.zero, Vector3.forward, color),
            };

            return points;
        }

        public override IEnumerable<Line> GetLines()
        {
            var baseLines = GetBaseLines();
            var lines = new Line[baseLines.Count];
            for (int i = 0; i < baseLines.Count; i++)
            {
                lines[i] = baseLines[i] * _data.Matrix4X4;
            }

            return lines;
        }
    }
}