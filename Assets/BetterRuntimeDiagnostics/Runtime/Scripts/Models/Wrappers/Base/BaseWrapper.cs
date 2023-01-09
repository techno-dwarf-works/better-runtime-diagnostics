using System.Collections.Generic;
using Better.Diagnostics.Runtime.Interfaces;
using UnityEngine;

namespace Better.Diagnostics.Runtime.Models
{
    public abstract class BaseWrapper : IRendererWrapper
    {
        private protected List<Line> _lines;
        private protected abstract Matrix4x4 Matrix();

        public void Initialize()
        {
            _lines = GenerateBaseLines();
        }

        private protected abstract List<Line> GenerateBaseLines();

        public abstract IEnumerable<Line> GetLines();
    }
}