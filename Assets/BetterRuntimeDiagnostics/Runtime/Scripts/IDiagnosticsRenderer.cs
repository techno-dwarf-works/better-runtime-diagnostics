using UnityEngine;

namespace Better.Diagnostics.Runtime
{
    public interface IDiagnosticsRenderer
    {
        public void Draw(Camera camera);
    }
}