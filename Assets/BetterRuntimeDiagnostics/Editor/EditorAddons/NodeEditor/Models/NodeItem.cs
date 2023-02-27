using System;
using Better.Diagnostics.Runtime.NodeModule;
using UnityEngine;

namespace Better.Diagnostics.EditorAddons.NodeEditor.Models
{
    public class NodeItem
    {
        private readonly Action<Rect> _rectChanged;
        public object InnerObject { get; }
        public Rect Position { get; private set; }

        public bool IsNodeRect { get; } = false;

        public void SetRect(Rect rect)
        {
            Position = rect;
            _rectChanged?.Invoke(rect);
        }

        public NodeItem(object innerObject, Rect position, Action<Rect> rectChanged)
        {
            _rectChanged = rectChanged;
            InnerObject = innerObject;
            if (position.Equals(Rect.zero))
            {
                position = new Rect(Vector2.zero, NodeStyles.DefaultSize);
                _rectChanged?.Invoke(position);
            }

            Position = position;
        }

        public NodeItem(INodeRect innerObject) : this(innerObject, innerObject.Position, innerObject.SetRect)
        {
            IsNodeRect = true;
        }
    }
}