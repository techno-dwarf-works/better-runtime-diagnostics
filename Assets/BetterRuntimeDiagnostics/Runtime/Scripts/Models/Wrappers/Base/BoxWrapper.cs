using System;
using System.Collections.Generic;
using UnityEngine;

namespace Better.Diagnostics.Runtime.Models
{
    public abstract class BoxWrapper : BaseWrapper
    {
        private protected abstract Vector3 Size();

        private protected override List<Line> GenerateBaseLines()
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
            points.Add(new Line(vertice1, vertice2)); //Back Bottom Edge
            points.Add(new Line(vertice2, vertice3)); //Right Bottom Edge
            points.Add(new Line(vertice3, vertice4)); //Forward Bottom Edge
            points.Add(new Line(vertice4, vertice1)); //Left Bottom Edge

            //Top
            points.Add(new Line(vertice5, vertice6)); //Back Top Edge
            points.Add(new Line(vertice6, vertice7)); //Right Top Edge
            points.Add(new Line(vertice7, vertice8)); //Forward Top Edge
            points.Add(new Line(vertice8, vertice5)); //Left Top Edge

            //Edges
            points.Add(new Line(vertice5, vertice1)); //Left Back Edge
            points.Add(new Line(vertice6, vertice2)); //Right Back Edge
            points.Add(new Line(vertice7, vertice3)); //Right Forward Edge
            points.Add(new Line(vertice8, vertice4)); //Left Forward Edge

            return points;
        }

        public override IEnumerable<Line> GetLines()
        {
            var lines = new Line[_lines.Count];
            var matrix4X4 = Matrix();
            var size = Size();
            for (int i = 0; i < _lines.Count; i++)
            {
                lines[i] = _lines[i] * (size / 2f) * matrix4X4;
            }

            return lines;
        }
    }
}