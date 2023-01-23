using System;
using Better.Diagnostics.Runtime.Interfaces;
using UnityEngine;

namespace Better.Diagnostics.Runtime.Models.Datas
{
    public class BoxColliderData : ITrackableData<Vector3>
    {
        private readonly BoxCollider _collider;

        public BoxColliderData(BoxCollider collider)
        {
            _collider = collider;
        }

        public Vector3 Size => _collider.size;
        public float OptionalSize { get; }

        public Matrix4x4 Matrix4X4
        {
            get
            {
                var transform = _collider.transform;
                return Matrix4x4.TRS(transform.localPosition + _collider.center, transform.localRotation, transform.localScale);
            }
        }
    }
}