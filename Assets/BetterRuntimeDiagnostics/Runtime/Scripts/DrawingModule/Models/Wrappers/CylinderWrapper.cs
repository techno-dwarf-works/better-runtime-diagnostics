using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Better.Diagnostics.Runtime.Calculations;
using Better.Diagnostics.Runtime.DrawingModule.Interfaces;
using UnityEngine;

namespace Better.Diagnostics.Runtime.DrawingModule
{
    public class CylinderWrapper : IRendererWrapper, ISettable<ITrackableData<float>, IRendererWrapper>
    {
        private Line[] _lines;
        private List<Line> _horizontalLines;
        private protected SphereCalculator _calculator;
        private protected ITrackableData<float> _data;

        public bool IsMarkedForRemove => _data.IsMarkedForRemove;

        public void OnRemoved()
        {
            RemovablePool.Instance.Add(this);
            _data.OnRemoved();
            foreach (var line in _lines)
            {
                line.OnRemoved();
            }
        }

        public IRendererWrapper Set(ITrackableData<float> data)
        {
            _data = data;
            return this;
        }

        public void MarkForRemove()
        {
            _data.MarkForRemove();
            foreach (var line in _lines)
            {
                line.MarkForRemove();
            }
        }

        public virtual void Initialize()
        {
            _calculator = new SphereCalculator();
            _horizontalLines = _calculator.PrepareHorizontalCircle(_data.Color);

            _lines = new Line[4];
            _lines[0] = RemovablePool.Instance.Get<Line, Vector3, Vector3, Color>(new Vector3(-1, 1, 0), new Vector3(-1, -1, 0), _data.Color);
            _lines[1] = RemovablePool.Instance.Get<Line, Vector3, Vector3, Color>(new Vector3(1, 1, 0), new Vector3(1, -1, 0), _data.Color);
            _lines[2] = RemovablePool.Instance.Get<Line, Vector3, Vector3, Color>(new Vector3(0, 1, 1), new Vector3(0, -1, 1), _data.Color);
            _lines[3] = RemovablePool.Instance.Get<Line, Vector3, Vector3, Color>(new Vector3(0, 1, -1), new Vector3(0, -1, -1), _data.Color);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private protected virtual int Lenght()
        {
            return _horizontalLines.Count * 2 + _lines.Length;
        }

        public virtual IEnumerable<Line> GetLines()
        {
            var r = _data.Size;
            var height = _data.OptionalSize;
            var matrix4X4 = _data.Matrix4X4;

            var count = Lenght();
            var lines = new Line[count];

            var b = height / 2f >= r;
            var size = b ? height / 2f - r : 0;
            var up = Vector3.up * size;

            var t = 0;
            TransformLines(lines, ref t, size, r, up, matrix4X4);

            return lines;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private protected virtual void TransformLines(Line[] lines, ref int index, float size, float radius, Vector3 up, Matrix4x4 matrix4X4)
        {
            for (var i = 0; i < _horizontalLines.Count; i++)
            {
                var upHorizontalLine = _horizontalLines[i].Copy();
                var downHorizontalLine = _horizontalLines[i].Copy();
                lines[index] = (upHorizontalLine * radius + up) * matrix4X4;
                index++;
                lines[index] = (downHorizontalLine * radius - up) * matrix4X4;
                index++;
            }

            for (var i = 0; i < _lines.Length; i++)
            {
                var line = _lines[i].Copy();
                lines[index] = line.Multiply(radius, size, radius) * matrix4X4;
                index++;
            }
        }
    }
}