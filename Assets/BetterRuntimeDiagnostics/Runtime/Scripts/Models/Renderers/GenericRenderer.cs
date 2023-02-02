using Better.Diagnostics.Runtime.Interfaces;

namespace Better.Diagnostics.Runtime.Models
{
    public class GenericRenderer : BaseRenderer
    {
        public GenericRenderer(IRendererWrapper wrapper) : base(wrapper)
        {
        }
    }
}