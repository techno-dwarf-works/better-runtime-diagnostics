using System;
using Better.Diagnostics.Runtime.Interfaces;
using UnityEngine;

namespace Better.Diagnostics.Runtime.Models.Datas
{
    [Serializable]
    public class CapsuleColliderData : ITrackableData<float>
    {
        private readonly CapsuleCollider _collider;


        private static readonly Quaternion[] DirectionRotation = new Quaternion[]
        {
            Quaternion.LookRotation(Vector3.forward, Vector3.left),
            Quaternion.LookRotation(Vector3.back, Vector3.up),
            Quaternion.LookRotation(Vector3.up, Vector3.forward)
        };

        public CapsuleColliderData(CapsuleCollider collider)
        {
            _collider = collider;
        }

        public float Size => _collider.radius;
        public float OptionalSize => _collider.height;

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
    }
}