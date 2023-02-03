using Better.Diagnostics.Runtime.DrawingModule.TrackableData;
using UnityEngine;

namespace Better.Diagnostics.Runtime.DrawingModule
{
    public class ColliderRenderer : BaseRenderer
    {
        public ColliderRenderer(BoxCollider collider) : base(new BoxWrapper(new BoxColliderData(collider)))
        {
        }

        public ColliderRenderer(SphereCollider collider) : base(new SphereWrapper(new SphereColliderData(collider)))
        {
        }
        
        public ColliderRenderer(CapsuleCollider collider) : base(new CapsuleWrapper(new CapsuleColliderData(collider)))
        {
        }
    }
}