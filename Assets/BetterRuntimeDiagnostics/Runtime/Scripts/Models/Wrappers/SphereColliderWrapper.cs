using System;
using UnityEngine;

namespace Better.Diagnostics.Runtime.Models
{
    public class SphereColliderWrapper : SphereWrapper
    {
        private readonly SphereCollider _collider;

        public SphereColliderWrapper(SphereCollider collider)
        {
            _collider = collider;
        }

        private protected override float Radius()
        {
            return _collider.radius;
        }

        private protected override Matrix4x4 Matrix()
        {
            var transform = _collider.transform;
            return Matrix4x4.TRS(transform.localPosition + _collider.center, transform.localRotation, transform.localScale);
        }
    }
}