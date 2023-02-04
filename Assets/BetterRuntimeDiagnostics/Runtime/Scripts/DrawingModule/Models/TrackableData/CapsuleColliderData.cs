using System;
using Better.Diagnostics.Runtime.DrawingModule.Interfaces;
using UnityEngine;

namespace Better.Diagnostics.Runtime.DrawingModule.TrackableData
{
    [Serializable]
    public class CapsuleColliderData : ITrackableData<float>, ISettable<CapsuleCollider, ITrackableData<float>>
    {
        private CapsuleCollider _collider;
        
        private bool _isNeedRemove;

        private static readonly Quaternion[] DirectionRotation = new Quaternion[]
        {
            Quaternion.LookRotation(Vector3.forward, Vector3.left),
            Quaternion.LookRotation(Vector3.back, Vector3.up),
            Quaternion.LookRotation(Vector3.up, Vector3.forward)
        };

        public float Size => _collider.radius;
        public float OptionalSize => _collider.height;
        public bool IsMarkedForRemove => _isNeedRemove || _collider.IsNullOrDestroyed();
        
        
        public ITrackableData<float> Set(CapsuleCollider collider)
        {
            _collider = collider;
            return this;
        }
        public Color Color => _collider.isTrigger ? Color.yellow : Color.green;

        public Matrix4x4 Matrix4X4
        {
            get
            {
                var transform = _collider.transform;
                return Matrix4x4.TRS(transform.localPosition + _collider.center,
                    transform.localRotation * DirectionRotation[_collider.direction],
                    transform.localScale);
            }
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