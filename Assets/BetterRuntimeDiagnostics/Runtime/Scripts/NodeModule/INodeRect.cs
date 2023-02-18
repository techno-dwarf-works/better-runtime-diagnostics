using UnityEngine;

namespace Better.Diagnostics.Runtime.NodeModule
{
    public interface INodeRect
    {
        public Rect Position { get; }
        public void SetRect(Rect rect);
    }
}