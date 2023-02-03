using Better.Diagnostics.Runtime.DrawingModule.Interfaces;
using UnityEngine;

namespace Better.Diagnostics.Runtime.DrawingModule.TrackableData
{
    public class BoxColliderData : ITrackableData<Vector3>
    {
        private readonly BoxCollider _collider;
        private bool _isNeedRemove;

        public BoxColliderData(BoxCollider collider)
        {
            _collider = collider;
        }

        public Vector3 Size => _collider.size;
        public float OptionalSize { get; }

        public bool IsMarkedForRemove => _isNeedRemove || _collider.IsNullOrDestroyed();
        public Color Color => _collider.isTrigger ? Color.yellow : Color.green;

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