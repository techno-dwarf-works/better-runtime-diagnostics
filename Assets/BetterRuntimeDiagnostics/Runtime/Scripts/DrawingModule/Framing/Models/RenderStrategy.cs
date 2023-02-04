using System;
using System.Collections.Generic;
using System.Linq;
using Better.Diagnostics.Runtime.DrawingModule.Interfaces;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Better.Diagnostics.Runtime.DrawingModule.Framing.Models
{
    internal abstract class RenderStrategy
    {
        private List<IDiagnosticsRenderer> _renderers;
        private Material _material;
        private readonly Func<IDiagnosticsRenderer, bool> _predicate = x => x.IsMarkedForRemove;

        public virtual void Initialize()
        {
            _material = new Material(Shader.Find("Hidden/Diagnostics/Unlit"));
            _renderers = new List<IDiagnosticsRenderer>();
        }

        private protected void RemoveMarked()
        {
            var list = new List<IDiagnosticsRenderer>(_renderers.Where(_predicate));
            foreach (var diagnosticsRenderer in list)
            {
                diagnosticsRenderer.OnRemoved();
                _renderers.Remove(diagnosticsRenderer);
            }
        }

        private protected void OnRender(Camera camera)
        {
            if (!Application.isPlaying)
            {
                Deconstruct();
                return;
            }

            foreach (var diagnosticsRenderer in _renderers)
            {
                diagnosticsRenderer.Draw(_material, camera);
            }
        }

        public void AddRenderer(IDiagnosticsRenderer diagnosticsRenderer)
        {
            _renderers.Add(diagnosticsRenderer);
        }

        public void AddRangeRenderer(IEnumerable<IDiagnosticsRenderer> diagnosticsRenderer)
        {
            _renderers.AddRange(diagnosticsRenderer);
        }

        public virtual void Deconstruct()
        {
#if UNITY_EDITOR
            Object.DestroyImmediate(_material);
#else
            Object.Destroy(_material);
#endif
        }
    }
}