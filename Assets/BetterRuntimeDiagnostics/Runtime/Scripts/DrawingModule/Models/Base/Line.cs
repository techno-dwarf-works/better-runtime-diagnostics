using Better.Diagnostics.Runtime.DrawingModule.Interfaces;
using UnityEngine;

namespace Better.Diagnostics.Runtime.DrawingModule
{
    public class Line : ISettable<Vector3, Vector3, Color, Line>, IRemovable
    {
        private Vector3 _start;
        private Vector3 _end;
        private Color _color;

        public bool IsMarkedForRemove { get; private set; }

        public Line Set(Vector3 start, Vector3 end, Color color)
        {
            IsMarkedForRemove = false;
            _start = start;
            _end = end;
            _color = color;
            return this;
        }

        public static Line operator *(Line line, float f)
        {
            return line.Set(line._start * f, line._end * f, line._color);
        }

        public static Line operator *(Line line, Vector3 f)
        {
            return line.Set(Vector3.Scale(line._start, f), Vector3.Scale(line._end, f), line._color);
        }

        public static Line operator +(Line line, Vector3 f)
        {
            return line.Set(line._start + f, line._end + f, line._color);
        }

        public static Line operator -(Line line, Vector3 f)
        {
            return line.Set(line._start - f, line._end - f, line._color);
        }

        public static Line operator *(Line line, Matrix4x4 matrix)
        {
            return line.Set(matrix.MultiplyPoint3x4(line._start), matrix.MultiplyPoint3x4(line._end), line._color);
        }

        public Line Multiply(float x, float y, float z)
        {
            var start = new Vector3(_start.x * x, _start.y * y, _start.z * z);
            var end = new Vector3(_end.x * x, _end.y * y, _end.z * z);
            return Set(start, end, _color);
        }

        public Line MoveEndBy(float addition)
        {
            return Set(_start, _end * addition, _color);
        }

        public Line MoveStartBy(float addition)
        {
            return Set(_start * addition, _end, _color);
        }

        public Line MoveEnd(Vector3 addition)
        {
            return Set(_start, _end + addition, _color);
        }

        public Line MoveStart(Vector3 addition)
        {
            return Set(_start + addition, _end, _color);
        }

        public void Draw()
        {
            GL.Color(_color);
            GL.Vertex(_start);
            GL.Vertex(_end);
        }

        public void MarkForRemove()
        {
            IsMarkedForRemove = true;
        }

        public void OnRemoved()
        {
            RemovablePool.Instance.Add(this);
        }

        public Line Copy()
        {
            return RemovablePool.Instance.Get<Line, Vector3, Vector3, Color>(_start, _end, _color);
        }
    }
}