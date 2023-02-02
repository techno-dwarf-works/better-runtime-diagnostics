using UnityEngine;

namespace Better.Diagnostics.Runtime.Interfaces
{
    public interface IDiagnosticsRenderer : IRemovable
    {
        public void Draw(Material material, Camera camera);
    }
}