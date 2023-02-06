using System.Collections.Generic;
using Better.Diagnostics.Runtime.DrawingModule.Interfaces;
using UnityEngine;

namespace Better.Diagnostics.Runtime.DrawingModule
{
    public class GenericLineWrapper : BaseWrapper<float>
    {
        public override void OnRemoved()
        {
            RemovablePool.Instance.Add(this);
            _data.OnRemoved();
            base.OnRemoved();
        }

        private protected override List<Line> GenerateBaseLines(Color color)
        {
            var points = new List<Line>(1)
            {
                RemovablePool.Instance.Get<Line, Vector3, Vector3, Color>(Vector3.zero, Vector3.forward, color)
            };

            return points;
        }

        public override IEnumerable<Line> GetLines()
        {
            var baseLines = GetBaseLines();
            var lines = new Line[baseLines.Count];
            for (var i = 0; i < baseLines.Count; i++)
            {
                var copy = baseLines[i].Copy();
                lines[i] = copy * _data.Matrix4X4;
            }

            return lines;
        }
    }
}