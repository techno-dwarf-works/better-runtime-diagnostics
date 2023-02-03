using System;
using Better.Diagnostics.Runtime.DrawingModule.Interfaces;
using UnityEngine;

namespace Better.Diagnostics.Runtime.DrawingModule
{
    public abstract class BaseRenderer : IDiagnosticsRenderer
    {
        private protected readonly IRendererWrapper _wrapper;

        public bool IsMarkedForRemove => _wrapper.IsMarkedForRemove;

        protected BaseRenderer(IRendererWrapper wrapper)
        {
            _wrapper = wrapper;
            _wrapper.Initialize();
        }

        public void MarkForRemove()
        {
            _wrapper.MarkForRemove();
        }

        public void Draw(Material material, Camera camera)
        {
            if (!material)
            {
                Debug.LogError("Please Assign a material on the inspector");
                return;
            }

            GL.PushMatrix();
            GL.LoadProjectionMatrix(camera.projectionMatrix);
            GL.modelview = camera.worldToCameraMatrix;
            material.SetPass(0);

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
            finally
            {
                GL.End();
                GL.PopMatrix();
            }
        }
    }
}