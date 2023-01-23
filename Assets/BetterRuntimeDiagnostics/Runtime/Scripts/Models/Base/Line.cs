using UnityEngine;

namespace Better.Diagnostics.Runtime.Models
{
    public readonly struct Line
    {
        private readonly Vector3 _start;
        private readonly Vector3 _end;
        private readonly Color _color;

        public Line(Vector3 start, Vector3 end)
        {
            _start = start;
            _end = end;
            _color = Color.red;
        }

        public Line(Vector3 start, Vector3 end, Color color)
        {
            _start = start;
            _end = end;
            _color = color;
        }

        public static Line operator *(Line line, float f)
        {
            return new Line(line._start * f, line._end * f, line._color);
        }

        public static Line operator *(Line line, Vector3 f)
        {
            return new Line(Vector3.Scale(line._start, f), Vector3.Scale(line._end, f), line._color);
        }

        public static Line operator +(Line line, Vector3 f)
        {
            return new Line(line._start + f, line._end + f, line._color);
        }

        public static Line operator -(Line line, Vector3 f)
        {
            return new Line(line._start - f, line._end - f, line._color);
        }

        public static Line operator *(Line line, Matrix4x4 matrix)
        {
            return new Line(matrix.MultiplyPoint3x4(line._start), matrix.MultiplyPoint3x4(line._end), line._color);
        }

        public static Line operator *(float f, Line line)
        {
            return line * f;
        }

        public Line Multiply(float x, float y, float z)
        {
            var start = new Vector3(_start.x * x, _start.y * y, _start.z * z);
            var end = new Vector3(_end.x * x, _end.y * y, _end.z * z);
            return new Line(start, end);
        }
        
        public Line MoveEndBy(float addition)
        {
            return new Line(_start, _end * addition, _color);
        }

        public Line MoveStartBy(float addition)
        {
            return new Line(_start * addition, _end, _color);
        }

        public Line MoveEnd(Vector3 addition)
        {
            return new Line(_start, _end + addition, _color);
        }
        public Line MoveStart(Vector3 addition)
        {
            return new Line(_start + addition, _end, _color);
        }

        public void Draw()
        {
            GL.Color(_color);
            GL.Vertex(_start);
            GL.Vertex(_end);
        }
    }
}