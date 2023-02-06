using System;
using Better.Diagnostics.Runtime.DrawingModule.Interfaces;
using UnityEngine;

namespace Better.Diagnostics.Runtime.DrawingModule
{
    public abstract class BaseRenderer : IDiagnosticsRenderer, ISettable<IRendererWrapper, IDiagnosticsRenderer>
    {
        private protected IRendererWrapper _wrapper;

        public bool IsMarkedForRemove => _wrapper.IsMarkedForRemove;

        public IDiagnosticsRenderer Set(IRendererWrapper wrapper)
        {
            _wrapper = wrapper;
            _wrapper.Initialize();
            return this;
        }

        public void MarkForRemove()
        {
            _wrapper.MarkForRemove();
        }

        public abstract void OnRemoved();

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

            var enumerable = _wrapper.GetLines();
            try
            {
                foreach (var line in enumerable)
                {
                    line.Draw();
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
                
                
                foreach (var line in enumerable)
                {
                    line.MarkForRemove();
                    line.OnRemoved();
                }
            }
        }
    }
}