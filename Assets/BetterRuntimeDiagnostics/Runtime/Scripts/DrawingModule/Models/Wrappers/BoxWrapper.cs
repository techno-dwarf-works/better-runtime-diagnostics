using System.Collections.Generic;
using Better.Diagnostics.Runtime.DrawingModule.Interfaces;
using UnityEngine;

namespace Better.Diagnostics.Runtime.DrawingModule
{
    public class BoxWrapper : BaseWrapper<Vector3>
    {
        private protected override List<Line> GenerateBaseLines(Color dataColor)
        {
            var points = new List<Line>(12);

            var vertice1 = new Vector3(-1, -1, -1);
            var vertice2 = new Vector3(1, -1, -1);
            var vertice3 = new Vector3(1, -1, 1);
            var vertice4 = new Vector3(-1, -1, 1);
            var vertice5 = new Vector3(-1, 1, -1);
            var vertice6 = new Vector3(1, 1, -1);
            var vertice7 = new Vector3(1, 1, 1);
            var vertice8 = new Vector3(-1, 1, 1);
            
            //Bottom
            points.Add(RemovablePool.Instance.Get<Line, Vector3, Vector3, Color>(vertice1, vertice2, dataColor)); //Back Bottom Edge
            points.Add(RemovablePool.Instance.Get<Line, Vector3, Vector3, Color>(vertice2, vertice3, dataColor)); //Right Bottom Edge
            points.Add(RemovablePool.Instance.Get<Line, Vector3, Vector3, Color>(vertice3, vertice4, dataColor)); //Forward Bottom Edge
            points.Add(RemovablePool.Instance.Get<Line, Vector3, Vector3, Color>(vertice4, vertice1, dataColor)); //Left Bottom Edge
            //Top
            points.Add(RemovablePool.Instance.Get<Line, Vector3, Vector3, Color>(vertice5, vertice6, dataColor)); //Back Top Edge
            points.Add(RemovablePool.Instance.Get<Line, Vector3, Vector3, Color>(vertice6, vertice7, dataColor)); //Right Top Edge
            points.Add(RemovablePool.Instance.Get<Line, Vector3, Vector3, Color>(vertice7, vertice8, dataColor)); //Forward Top Edge
            points.Add(RemovablePool.Instance.Get<Line, Vector3, Vector3, Color>(vertice8, vertice5, dataColor)); //Left Top Edge
            //Edges
            points.Add(RemovablePool.Instance.Get<Line, Vector3, Vector3, Color>(vertice5, vertice1, dataColor)); //Left Back Edge
            points.Add(RemovablePool.Instance.Get<Line, Vector3, Vector3, Color>(vertice6, vertice2, dataColor)); //Right Back Edge
            points.Add(RemovablePool.Instance.Get<Line, Vector3, Vector3, Color>(vertice7, vertice3, dataColor)); //Right Forward Edge
            points.Add(RemovablePool.Instance.Get<Line, Vector3, Vector3, Color>(vertice8, vertice4, dataColor)); //Left Forward Edge

            return points;
        }

        public override IEnumerable<Line> GetLines()
        {
            var baseLines = GetBaseLines();
            var lines = new Line[baseLines.Count];
            var trackableData = _data;
            var matrix4X4 = trackableData.Matrix4X4;
            var size = trackableData.Size;

            for (var i = 0; i < baseLines.Count; i++)
            {
                var copy = baseLines[i].Copy();
                lines[i] = copy * (size / 2f) * matrix4X4;
            }

            return lines;
        }

        public override void OnRemoved()
        {
            RemovablePool.Instance.Add(this);
            _data.OnRemoved();
            base.OnRemoved();
        }
    }
}