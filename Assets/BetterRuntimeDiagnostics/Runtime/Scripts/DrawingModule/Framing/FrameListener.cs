using System;
using System.Collections.Generic;
using Better.Diagnostics.Runtime.DrawingModule.Framing.Models;
using Better.Diagnostics.Runtime.DrawingModule.Interfaces;
using UnityEngine;
using UnityEngine.Rendering;

namespace Better.Diagnostics.Runtime.DrawingModule.Framing
{
    internal static class FrameListener
    {
        private static RenderStrategy _renderStrategy;

        [RuntimeInitializeOnLoadMethod]
        private static void Initialize()
        {
            InitializeInternal();
            AppDomain.CurrentDomain.ProcessExit += GLDrawerDeconstruct;
            RenderPipelineManager.activeRenderPipelineTypeChanged += OnRenderPipelineTypeChanged;
        }

        private static void InitializeInternal()
        {
#if USING_URP || USING_HDRP || USING_CUSTOM_PIPELINE
            _renderStrategy = new CustomPipelineStrategy();
#else
            if (RenderPipelineManager.currentPipeline == null)
            {
                _renderStrategy = new BuildInStrategy();
            }
            else
            {
                _renderStrategy = new CustomPipelineStrategy();
            }
#endif
            _renderStrategy.Initialize();
        }

        private static void OnRenderPipelineTypeChanged()
        {
            _renderStrategy?.Deconstruct();
            InitializeInternal();
        }

        private static void GLDrawerDeconstruct(object sender, EventArgs e)
        {
            Deconstruct();
        }

        private static void Deconstruct()
        {
            RenderPipelineManager.activeRenderPipelineTypeChanged -= OnRenderPipelineTypeChanged;
            AppDomain.CurrentDomain.ProcessExit -= GLDrawerDeconstruct;
            _renderStrategy?.Deconstruct();
            _renderStrategy = null;
        }


        public static void AddRenderer(IDiagnosticsRenderer diagnosticsRenderer)
        {
            _renderStrategy?.AddRenderer(diagnosticsRenderer);
        }

        public static void AddWrapper(IRendererWrapper wrapper)
        {
            _renderStrategy?.AddRenderer(RemovablePool.Instance.Get<GenericRenderer>().Set(wrapper));
        }

        public static void AddRangeRenderer(IEnumerable<IDiagnosticsRenderer> diagnosticsRenderer)
        {
            _renderStrategy?.AddRangeRenderer(diagnosticsRenderer);
        }
    }
}