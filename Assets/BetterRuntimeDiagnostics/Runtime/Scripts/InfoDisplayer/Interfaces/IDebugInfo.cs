using UnityEngine.UIElements;

namespace Better.Diagnostics.Runtime.InfoDisplayer.Interfaces
{
    public interface IDebugInfo
    {
        public void Initialize(UIDocument uiDocument);
        public void Deconstruct();
    }
}