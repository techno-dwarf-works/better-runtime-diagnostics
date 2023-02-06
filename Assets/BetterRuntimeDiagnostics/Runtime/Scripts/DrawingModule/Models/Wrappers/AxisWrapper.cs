using System.Collections.Generic;
using Better.Diagnostics.Runtime.DrawingModule.Interfaces;
using UnityEngine;

namespace Better.Diagnostics.Runtime.DrawingModule
{
    public class AxisWrapper : BaseWrapper<float>
    {
        public override void OnRemoved()
        {
            RemovablePool.Instance.Add(this);
            _data.OnRemoved();
            base.OnRemoved();
        }

        private protected override List<Line> GenerateBaseLines(Color color)
        {
            var points = new List<Line>(3);

            
            points.Add(RemovablePool.Instance.Get<Line, Vector3, Vector3, Color>(Vector3.zero, Vector3.forward, Color.blue));
            points.Add(RemovablePool.Instance.Get<Line, Vector3, Vector3, Color>(Vector3.zero, Vector3.right, Color.red));
            points.Add(RemovablePool.Instance.Get<Line, Vector3, Vector3, Color>(Vector3.zero, Vector3.up, Color.green));

            return points;
        }

        public override IEnumerable<Line> GetLines()
        {
            var baseLines = GetBaseLines();
            var lines = new Line[baseLines.Count];
            var matrix4X4 = _data.Matrix4X4;
            for (var i = 0; i < baseLines.Count; i++)
            {
                var copy = baseLines[i].Copy();
                lines[i] = copy * matrix4X4;
            }

            return lines;
        }
    }
}