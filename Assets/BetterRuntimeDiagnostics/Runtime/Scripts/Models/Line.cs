using UnityEngine;

namespace Better.Diagnostics.Runtime
{
    public readonly struct Line
    {
        private readonly Vector3 _start;
        private readonly Vector3 _end;

        public Line(Vector3 start, Vector3 end)
        {
            _start = start;
            _end = end;
        }
        
        public void Draw(Camera camera)
        {
            GL.Vertex(_start);
            GL.Vertex(_end);
        }
    }
}