using UnityEngine;

namespace Better.Diagnostics.Runtime.Models
{
    public class CapsuleColliderWrapper : CapsuleWrapper
    {
        private readonly CapsuleCollider _collider;

        private static readonly Quaternion[] DirectionRotation = new Quaternion[]
        {
            Quaternion.LookRotation(Vector3.forward, Vector3.left),
            Quaternion.LookRotation(Vector3.back, Vector3.up),
            Quaternion.LookRotation(Vector3.up, Vector3.forward)
        };

        public CapsuleColliderWrapper(CapsuleCollider collider)
        {
            _collider = collider;
        }

        private protected override float Radius()
        {
            return _collider.radius;
        }

        private protected override float Height()
        {
            return _collider.height;
        }

        private protected override Matrix4x4 Matrix()
        {
            var transform = _collider.transform;
            return Matrix4x4.TRS(transform.localPosition + _collider.center, 
                transform.localRotation * DirectionRotation[_collider.direction],
                transform.localScale);
        }
    }
}