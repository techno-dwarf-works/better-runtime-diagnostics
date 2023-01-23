using Better.Diagnostics.Runtime.Models.Datas;
using UnityEngine;

namespace Better.Diagnostics.Runtime.Models
{
    public class ColliderRenderer : BaseRenderer
    {
        public ColliderRenderer(Material material, BoxCollider collider) : base(material, new BoxWrapper(new BoxColliderData(collider)))
        {
        }

        public ColliderRenderer(Material material, SphereCollider collider) : base(material, new SphereWrapper(new SphereColliderData(collider)))
        {
        }
        
        public ColliderRenderer(Material material, CapsuleCollider collider) : base(material, new CapsuleWrapper(new CapsuleColliderData(collider)))
        {
        }
    }
}