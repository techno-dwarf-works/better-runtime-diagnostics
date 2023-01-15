using System;
using Better.Diagnostics.Runtime.Interfaces;
using UnityEngine;

namespace Better.Diagnostics.Runtime.Models
{
    public abstract class BaseRenderer : IDiagnosticsRenderer
    {
        private protected readonly Material _mat;
        private protected readonly IRendererWrapper _wrapper;

        protected BaseRenderer(Material material, IRendererWrapper wrapper)
        {
            _mat = material;
            _wrapper = wrapper;
            _wrapper.Initialize();
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

            try
            {
                foreach (var corner in _wrapper.GetLines())
                {
                    corner.Draw();
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }

            GL.End();

            GL.PopMatrix();
        }
    }
}