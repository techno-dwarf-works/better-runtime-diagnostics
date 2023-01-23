using Better.Diagnostics.Runtime.Interfaces;
using UnityEngine;

namespace Better.Diagnostics.Runtime.Models.Datas
{
    public class TransformData : ITrackableData<float>
    {
        private readonly Transform _transform;

        public TransformData(Transform transform)
        {
            _transform = transform;
        }

        public float Size { get; }
        public float OptionalSize { get; }
        public Matrix4x4 Matrix4X4 => Matrix4x4.TRS(_transform.localPosition, _transform.localRotation, _transform.localScale);
    }
}