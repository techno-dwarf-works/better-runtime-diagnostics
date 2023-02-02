using Better.Diagnostics.Runtime.Models.Datas;
using UnityEngine;

namespace Better.Diagnostics.Runtime.Models
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