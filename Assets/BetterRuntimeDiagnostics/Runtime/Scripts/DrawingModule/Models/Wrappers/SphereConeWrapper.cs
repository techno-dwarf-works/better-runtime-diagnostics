using System.Collections.Generic;
using Better.Diagnostics.Runtime.Calculations;
using Better.Diagnostics.Runtime.DrawingModule.Interfaces;
using UnityEngine;

namespace Better.Diagnostics.Runtime.DrawingModule
{
    public class SphereConeWrapper : PrismaWrapper
    {
        private SphereCalculator _calculator;
        private readonly ITrackableData<float> _data;

        public SphereConeWrapper(ITrackableData<float> data)
        {
            _data = data;
        }

        public override bool IsMarkedForRemove => _data.IsMarkedForRemove;

        public override void MarkForRemove()
        {
            _data.MarkForRemove();
        }

        protected override IList<Line> FillUpSideLines()
        {
            var lines = new Line[4];
            lines[0] = new Line(Vector3.zero, Vector3.forward);
            lines[1] = new Line(Vector3.zero, Vector3.back);
            lines[2] = new Line(Vector3.zero, Vector3.right);
            lines[3] = new Line(Vector3.zero, Vector3.left);
            return lines;
        }

        public override IList<Line> FillUpBaseLines()
        {
            _calculator = new SphereCalculator();
            return _calculator.PrepareHorizontalCircle();
        }

        protected override float GetHeight()
        {
            return _data.OptionalSize;
        }

        protected override float GetBaseSize()
        {
            return _data.Size;
        }

        protected override Matrix4x4 GetMatrix()
        {
            return _data.Matrix4X4;
        }
    }
}