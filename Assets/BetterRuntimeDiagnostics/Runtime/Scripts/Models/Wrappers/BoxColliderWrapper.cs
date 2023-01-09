using System;
using UnityEngine;

namespace Better.Diagnostics.Runtime.Models
{
    public class BoxColliderWrapper : BoxWrapper
    {
        private readonly BoxCollider _collider;

        public BoxColliderWrapper(BoxCollider collider)
        {
            _collider = collider;
        }

        private protected override Vector3 Size()
        {
            return _collider.size;
        }

        private protected override Matrix4x4 Matrix()
        {
            var transform = _collider.transform;
            return Matrix4x4.TRS(transform.localPosition + _collider.center, transform.localRotation, transform.localScale);
        }
    }
}