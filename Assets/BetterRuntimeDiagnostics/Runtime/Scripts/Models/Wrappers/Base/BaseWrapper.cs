using System.Collections.Generic;
using Better.Diagnostics.Runtime.Interfaces;
using UnityEngine;

namespace Better.Diagnostics.Runtime.Models
{
    public abstract class BaseWrapper<T> : IRendererWrapper
    {
        private protected List<Line> _lines;
        private protected readonly ITrackableData<T> _data;

        protected BaseWrapper(ITrackableData<T> data)
        {
            _data = data;
        }

        public void Initialize()
        {
            _lines = GenerateBaseLines();
        }

        private protected abstract List<Line> GenerateBaseLines();

        public abstract IEnumerable<Line> GetLines();
    }
}