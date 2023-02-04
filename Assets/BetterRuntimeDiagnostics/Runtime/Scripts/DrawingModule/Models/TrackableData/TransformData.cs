using Better.Diagnostics.Runtime.DrawingModule.Interfaces;
using UnityEngine;

namespace Better.Diagnostics.Runtime.DrawingModule.TrackableData
{
    public class TransformData : ITrackableData<float>, ISettable<Transform, ITrackableData<float>>
    {
        private Transform _transform;
        private bool _isNeedRemove;

        public  ITrackableData<float> Set(Transform transform)
        {
            _transform = transform;
            return this;
        }

        public float Size { get; }
        public float OptionalSize { get; }
        public bool IsMarkedForRemove => _isNeedRemove || _transform.IsNullOrDestroyed();
        public Color Color { get; } = Color.white;
        public Matrix4x4 Matrix4X4 => Matrix4x4.TRS(_transform.localPosition, _transform.localRotation, _transform.localScale);

        public void MarkForRemove()
        {
            _isNeedRemove = true;
        }

        public void OnRemoved()
        {
            RemovablePool.Instance.Add(this);
        }
    }
}