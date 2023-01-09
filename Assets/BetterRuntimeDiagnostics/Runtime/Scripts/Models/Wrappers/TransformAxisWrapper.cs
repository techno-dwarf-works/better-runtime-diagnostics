using UnityEngine;

namespace Better.Diagnostics.Runtime.Models
{
    public class TransformAxisWrapper : AxisWrapper
    {
        private readonly Transform _transform;

        public TransformAxisWrapper(Transform transform)
        {
            _transform = transform;
        }
        
        private protected override Matrix4x4 Matrix()
        {
            return Matrix4x4.TRS(_transform.localPosition, _transform.localRotation, _transform.localScale);
        }
    }
}