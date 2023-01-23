using System.Collections.Generic;
using System.Linq;
using Better.Diagnostics.Runtime.Interfaces;
using Better.Diagnostics.Runtime.Models;
using Better.Diagnostics.Runtime.Models.Datas;
using UnityEngine;

namespace Better.Diagnostics.Runtime
{
    public class TestGL : MonoBehaviour
    {
        [SerializeField] private Camera cam;
        [SerializeField] private DefaultTrackable defaultTrackable;
        private Material _material;
        private List<IDiagnosticsRenderer> _renderers = new List<IDiagnosticsRenderer>();

        private void Start()
        {
            _material = new Material(Shader.Find("Hidden/Diagnostics/Unlit"));
            _renderers.AddRange(FindObjectsOfType<BoxCollider>().Select(x => new ColliderRenderer(_material, x)));
            _renderers.AddRange(FindObjectsOfType<SphereCollider>().Select(x => new ColliderRenderer(_material, x)));
            _renderers.AddRange(FindObjectsOfType<CapsuleCollider>().Select(x => new ColliderRenderer(_material, x)));
            _renderers.AddRange(FindObjectsOfType<Transform>().Select(x => new GenericRenderer(_material, new AxisWrapper(new TransformData(x)))));
            _renderers.Add(new GenericRenderer(_material, new ConeWrapper(defaultTrackable)));
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
