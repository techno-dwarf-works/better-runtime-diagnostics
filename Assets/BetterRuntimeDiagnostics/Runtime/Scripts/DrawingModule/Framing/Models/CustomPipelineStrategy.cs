using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Better.Diagnostics.Runtime.DrawingModule.Framing.Models
{
    internal class CustomPipelineStrategy : RenderStrategy
    {
        public override void Initialize()
        {
            base.Initialize();
            RenderPipelineManager.endContextRendering += OnEndFrameRendering;
            //RenderPipelineManager.beginFrameRendering
        }
        private void OnEndFrameRendering(ScriptableRenderContext context, List<Camera> list)
        {
            RemoveMarked();
            foreach (var camera in list)
            {
                OnRender(camera);
            }
        }

        public override void Deconstruct()
        {
            base.Deconstruct();
            RenderPipelineManager.endContextRendering -= OnEndFrameRendering;
        }
    }
}