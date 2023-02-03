using Better.Diagnostics.Runtime.DrawingModule.Interfaces;

namespace Better.Diagnostics.Runtime.DrawingModule
{
    public class GenericRenderer : BaseRenderer
    {
        public GenericRenderer(IRendererWrapper wrapper) : base(wrapper)
        {
        }
    }
}