using System.Collections.Generic;
using Better.Diagnostics.Runtime.Interfaces;
using UnityEngine;

namespace Better.Diagnostics.Runtime.Models
{
    public abstract class PrismaWrapper : IRendererWrapper
    {
        private IList<Line> _baseCircle;
        private IList<Line> _lines;

        public void Initialize()
        {
            _baseCircle = FillUpBaseLines();
            _lines = FillUpSideLines();
        }

        protected abstract IList<Line> FillUpSideLines();

        public abstract IList<Line> FillUpBaseLines();

        protected abstract float GetHeight();

        protected abstract float GetBaseSize();

        protected abstract Matrix4x4 GetMatrix();

        public IEnumerable<Line> GetLines()
        {
            var count = _lines.Count + _baseCircle.Count;
            var list = new Line[count];
            var h = GetHeight();
            var baseSize = GetBaseSize();
            var matrix4X4 = GetMatrix();

            var height = Vector3.down * h;
            var t = 0;
            for (var index = 0; index < _lines.Count; index++)
            {
                var line = _lines[index];
                list[t] = line.MoveEndBy(baseSize).MoveEnd(height) * matrix4X4;
                t++;
            }

            for (var index = 0; index < _baseCircle.Count; index++)
            {
                list[t] = ((_baseCircle[index] * baseSize) + height) * matrix4X4;
                t++;
            }

            return list;
        }
    }
}