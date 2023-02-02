using System.Collections.Generic;
using Better.Diagnostics.Runtime.Interfaces;
using UnityEngine;

namespace Better.Diagnostics.Runtime.Framing.Models
{
    internal class BuildInStrategy : RenderStrategy
    {
        private List<IDiagnosticsRenderer> _renderers;
        private Material _material;
        private HashSet<Camera> _cameras;

        public override void Initialize()
        {
            base.Initialize();
            _cameras= new HashSet<Camera>();
            Camera.onPostRender += OnPostRender;
        }

        private void OnPostRender(Camera camera)
        {
            if (_cameras.Contains(camera))
            {
                _cameras.Clear();
                RemoveMarked();
            }
            _cameras.Add(camera);
            OnRender(camera);
        }

        public override void Deconstruct()
        {
            Camera.onPostRender -= OnPostRender;
#if UNITY_EDITOR
            Object.DestroyImmediate(_material);
#else
            Object.Destroy(_material);
#endif
        }
    }
}