using UnityEngine;

namespace Better.Diagnostics.Runtime.Models
{
    public class BoxColliderRenderer : BaseBoxRenderer
    {
        private readonly BoxCollider _boxCollider;

        public BoxColliderRenderer(BoxCollider collider)
        {
            _boxCollider = collider;
        }

        private protected override Vector3 Size()
        {
            return _boxCollider.size;
        }

        private protected override Matrix4x4 Matrix()
        {
            var transform = _boxCollider.transform;
            return Matrix4x4.TRS(transform.localPosition + _boxCollider.center, transform.localRotation, transform.localScale);
        }
    }
}