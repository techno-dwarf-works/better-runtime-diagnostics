using System;
using Better.Diagnostics.Runtime.Interfaces;
using UnityEngine;

namespace Better.Diagnostics.Runtime.Models
{
    public class GenericRenderer : BaseRenderer
    {
        public GenericRenderer(Material material, IRendererWrapper wrapper) : base(material, wrapper)
        {
        }
    }
}