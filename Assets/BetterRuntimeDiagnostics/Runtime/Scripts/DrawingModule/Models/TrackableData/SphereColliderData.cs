using Better.Diagnostics.Runtime.DrawingModule.Interfaces;
using UnityEngine;

namespace Better.Diagnostics.Runtime.DrawingModule.TrackableData
{
    public class SphereColliderData : ITrackableData<float>, ISettable<SphereCollider, ITrackableData<float>>
    {
        private SphereCollider _collider;
        private bool _isNeedRemove;

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
        
        public ITrackableData<float> Set(SphereCollider collider)
        {
            _collider = collider;
            return this;
        }

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