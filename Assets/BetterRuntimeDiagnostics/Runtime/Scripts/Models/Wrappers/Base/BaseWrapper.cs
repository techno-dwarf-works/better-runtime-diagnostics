using System.Collections.Generic;
using Better.Diagnostics.Runtime.Interfaces;
using UnityEngine;

namespace Better.Diagnostics.Runtime.Models
{
    public abstract class Base : IRendererWrapper
    {
        private List<Line> _lines;

        public abstract bool IsMarkedForRemove { get; }

        public abstract void MarkForRemove();

        public void Initialize()
        {
            _lines = GenerateBaseLines(GetColor());
        }

        protected abstract Color GetColor();

        protected List<Line> GetBaseLines()
        {
            return _lines;
        }

        private protected abstract List<Line> GenerateBaseLines(Color color);

        public abstract IEnumerable<Line> GetLines();
    }
    
    public abstract class BaseWrapper<T> : Base
    {
        private protected readonly ITrackableData<T> _data;
        public override bool IsMarkedForRemove => _data.IsMarkedForRemove;

        protected BaseWrapper(ITrackableData<T> data)
        {
            _data = data;
        }

        protected override Color GetColor()
        {
            return _data.Color;
        }

        public override void MarkForRemove()
        {
            _data.MarkForRemove();
        }
    }
}