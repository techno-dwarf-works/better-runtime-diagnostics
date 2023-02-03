using System;
using System.Collections.Generic;
using Better.Diagnostics.Runtime.DrawingModule.Framing.Models;
using Better.Diagnostics.Runtime.DrawingModule.Interfaces;
using UnityEngine;

namespace Better.Diagnostics.Runtime.DrawingModule.Framing
{
    internal static class FrameListener
    {
        private static RenderStrategy _renderStrategy;

        [RuntimeInitializeOnLoadMethod]
        private static void Initialize()
        {
#if USING_URP || USING_HDRP || USING_CUSTOM_PIPELINE
            _renderStrategy = new CustomPipelineStrategy();
#else
            _renderStrategy = new BuildInStrategy();
#endif
            _renderStrategy.Initialize();
            AppDomain.CurrentDomain.ProcessExit += GLDrawerDeconstruct;
        }

        private static void GLDrawerDeconstruct(object sender, EventArgs e)
        {
            Deconstruct();
        }

        private static void Deconstruct()
        {
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
            _renderStrategy?.AddRenderer(new GenericRenderer(wrapper));
        }

        public static void AddRangeRenderer(IEnumerable<IDiagnosticsRenderer> diagnosticsRenderer)
        {
            _renderStrategy?.AddRangeRenderer(diagnosticsRenderer);
        }
    }
}