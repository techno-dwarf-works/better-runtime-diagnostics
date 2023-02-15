using System;
using UnityEngine;

namespace Better.Diagnostics.EditorAddons.SettingsEditor
{
    public class NodeItem
    {
        private readonly Action<Rect> _rectChanged;
        public object InnerObject { get; }
        public Rect Position { get; private set; }

        public void SetPosition(Rect rect)
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
                position = new Rect(Vector2.zero, NodeWindow.DefaultSize);
                _rectChanged?.Invoke(position);
            }

            Position = position;
        }
    }
}