using Better.Diagnostics.Runtime.Models;
using UnityEngine;

namespace Better.Diagnostics.Runtime
{
    public class ColliderRenderer : IDiagnosticsRenderer
    {
        private readonly Material _mat;
        private readonly IRendererWrapper _collider;

        public ColliderRenderer(Material material, BoxCollider collider)
        {
            _mat = material;
            _collider = new BoxColliderRenderer(collider);
        }
        
        public ColliderRenderer(Material material, SphereCollider collider)
        {
            _mat = material;
            _collider = new SphereColliderRenderer(collider);
        }

        public void Draw(Camera camera)
        {
            if (!_mat)
            {
                Debug.LogError("Please Assign a material on the inspector");
                return;
            }

            GL.PushMatrix();
            GL.LoadProjectionMatrix(camera.projectionMatrix);
            GL.modelview = camera.worldToCameraMatrix;
            _mat.SetPass(0);

            GL.Begin(GL.LINES);
            GL.Color(Color.red);

            foreach (var corner in _collider.GetLines())
            {
                corner.Draw(camera);
            }

            GL.End();

            GL.PopMatrix();
        }
    }
}