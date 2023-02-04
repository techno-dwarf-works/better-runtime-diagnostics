using Better.Diagnostics.Runtime.DrawingModule.Interfaces;

namespace Better.Diagnostics.Runtime.DrawingModule
{
    public class GenericRenderer : BaseRenderer
    {
        public override void OnRemoved()
        {
            RemovablePool.Instance.Add(this);
            _wrapper.OnRemoved();
        }
    }
}