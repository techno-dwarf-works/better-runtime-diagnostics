using Better.Diagnostics.Runtime.Interfaces;
using UnityEngine;

namespace Better.Diagnostics.Runtime.Models.Datas
{
    public class SphereColliderData : ITrackableData<float>
    {
        private readonly SphereCollider _collider;
        private bool _isNeedRemove;

        public SphereColliderData(SphereCollider collider)
        {
            _collider = collider;
        }

        public float Size => _collider.radius;
        public float OptionalSize { get; }
        public Color Color => _collider.isTrigger ? Color.yellow : Color.green;
        public bool IsMarkedForRemove => _isNeedRemove || _collider.IsNullOrDestroyed();
        public Matrix4x4 Matrix4X4
        {
            get
            {
                var transform = _collider.transform;
                return Matrix4x4.TRS(transform.localPosition + _collider.center, transform.localRotation, transform.localScale);
            }
        }

        public void MarkForRemove()
        {
            _isNeedRemove = true;
        }
    }
}