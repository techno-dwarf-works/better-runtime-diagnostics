using System.Collections.Generic;
using Better.Diagnostics.Runtime.DrawingModule.Interfaces;
using UnityEngine;

namespace Better.Diagnostics.Runtime.DrawingModule
{
    public class SquareConeWrapper : PrismaWrapper, ISettable<ITrackableData<float>, IRendererWrapper>
    {
        private ITrackableData<float> _data;


        public override bool IsMarkedForRemove => _data.IsMarkedForRemove;


        public IRendererWrapper Set(ITrackableData<float> data)
        {
            _data = data;
            return this;
        }

        public override void OnRemoved()
        {
            RemovablePool.Instance.Add(this);
            _data.OnRemoved();
            base.OnRemoved();
        }

        public override void MarkForRemove()
        {
            _data.MarkForRemove();
            base.MarkForRemove();
        }

        protected override IList<Line> FillUpSideLines()
        {
            var lines = new Line[4];
            lines[0] = RemovablePool.Instance.Get<Line, Vector3, Vector3, Color>(Vector3.zero, Vector3.forward, _data.Color);
            lines[1] = RemovablePool.Instance.Get<Line, Vector3, Vector3, Color>(Vector3.zero, Vector3.back, _data.Color);
            lines[2] = RemovablePool.Instance.Get<Line, Vector3, Vector3, Color>(Vector3.zero, Vector3.right, _data.Color);
            lines[3] = RemovablePool.Instance.Get<Line, Vector3, Vector3, Color>(Vector3.zero, Vector3.left, _data.Color);
            return lines;
        }

        public override IList<Line> FillUpBaseLines()
        {
            var lines = new Line[4];
            lines[0] = RemovablePool.Instance.Get<Line, Vector3, Vector3, Color>(Vector3.right, Vector3.forward, _data.Color);
            lines[1] = RemovablePool.Instance.Get<Line, Vector3, Vector3, Color>(Vector3.right, Vector3.back, _data.Color);
            lines[2] = RemovablePool.Instance.Get<Line, Vector3, Vector3, Color>(Vector3.forward, Vector3.left, _data.Color);
            lines[3] = RemovablePool.Instance.Get<Line, Vector3, Vector3, Color>(Vector3.back, Vector3.left, _data.Color);
            return lines;
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