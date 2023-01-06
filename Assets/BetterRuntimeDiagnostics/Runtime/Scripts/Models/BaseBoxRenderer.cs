using System.Collections.Generic;
using UnityEngine;

namespace Better.Diagnostics.Runtime.Models
{
    public abstract class BaseBoxRenderer : IRendererWrapper
    {
        private protected abstract Vector3 Size();
        private protected abstract Matrix4x4 Matrix();
        
        public IEnumerable<Line> GetLines()
        {
            var size = Size() / 2f;
            var points = new Line[12];

            var matrix4X4 = Matrix();
            var vertice1 = matrix4X4.MultiplyPoint3x4(new Vector3(-size.x, -size.y, -size.z));
            var vertice2 = matrix4X4.MultiplyPoint3x4(new Vector3(size.x, -size.y, -size.z));
            var vertice3 = matrix4X4.MultiplyPoint3x4(new Vector3(size.x, -size.y, size.z));
            var vertice4 = matrix4X4.MultiplyPoint3x4(new Vector3(-size.x, -size.y, size.z));
            var vertice5 = matrix4X4.MultiplyPoint3x4(new Vector3(-size.x, size.y, -size.z));
            var vertice6 = matrix4X4.MultiplyPoint3x4(new Vector3(size.x, size.y, -size.z));
            var vertice7 = matrix4X4.MultiplyPoint3x4(new Vector3(size.x, size.y, size.z));
            var vertice8 = matrix4X4.MultiplyPoint3x4(new Vector3(-size.x, size.y, size.z));
            
            points[0] = new Line(vertice1, vertice2); //Back Bottom Edge
            points[1] = new Line(vertice2, vertice3); //Right Bottom Edge
            points[2] = new Line(vertice3, vertice4); //Forward Bottom Edge
            points[3] = new Line(vertice4, vertice1); //Left Bottom Edge
            
            points[4] = new Line(vertice5, vertice6); //Back Top Edge
            points[5] = new Line(vertice6, vertice7); //Right Top Edge
            points[6] = new Line(vertice7, vertice8); //Forward Top Edge
            points[7] = new Line(vertice8, vertice5); //Left Top Edge
            
            
            points[8] = new Line(vertice5, vertice1); //Left Back Edge
            points[9] = new Line(vertice6, vertice2); //Right Back Edge
            points[10] = new Line(vertice7, vertice3); //Right Forward Edge
            points[11] = new Line(vertice8, vertice4); //Left Forward Edge
            
            return points;
        }
    }
}