using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Better.Diagnostics.Runtime
{
    public class TestGL : MonoBehaviour
    {
        [SerializeField] private Camera cam;
        private Material _material;
        public static List<IDiagnosticsRenderer> _renderers = new List<IDiagnosticsRenderer>();

        private void Start()
        {
            _material = new Material(Shader.Find("Hidden/Diagnostics/Unlit"));
            _renderers.AddRange(FindObjectsOfType<BoxCollider>().Select(x => new ColliderRenderer(_material, x)));
            _renderers.AddRange(FindObjectsOfType<SphereCollider>().Select(x => new ColliderRenderer(_material, x)));
        }

        private void OnPostRender()
        {
            foreach (var diagnosticsRenderer in _renderers)
            {
                diagnosticsRenderer.Draw(cam);
            }
        }
    }
}
