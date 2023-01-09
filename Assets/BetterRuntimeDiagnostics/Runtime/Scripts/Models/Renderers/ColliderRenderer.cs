using System;
using UnityEngine;

namespace Better.Diagnostics.Runtime.Models
{
    
    public class ColliderRenderer : BaseRenderer
    {
        public ColliderRenderer(Material material, BoxCollider collider) : base(material, new BoxColliderWrapper(collider))
        {
        }

        public ColliderRenderer(Material material, SphereCollider collider) : base(material, new SphereColliderWrapper(collider))
        {
        }
        
        public ColliderRenderer(Material material, CapsuleCollider collider) : base(material, new CapsuleColliderWrapper(collider))
        {
        }
    }
}