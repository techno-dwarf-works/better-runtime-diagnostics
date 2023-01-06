using UnityEngine;

namespace Better.Diagnostics.Runtime
{
    public class SphereColliderRenderer : BaseSphereRenderer
    {
        private SphereCollider _boxCollider;

        public SphereColliderRenderer(SphereCollider collider)
        {
            _boxCollider = collider;
        }

        private protected override float Radius()
        {
            return _boxCollider.radius;
        }

        private protected override Matrix4x4 Matrix()
        {
            var transform = _boxCollider.transform;
            return Matrix4x4.TRS(transform.localPosition + _boxCollider.center, transform.localRotation, transform.localScale);
        }
    }
}