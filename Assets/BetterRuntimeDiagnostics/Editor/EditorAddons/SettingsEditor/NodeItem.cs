using System;
using UnityEngine;

namespace Better.Diagnostics.EditorAddons.SettingsEditor
{
    public class NodeItem
    {
        private readonly Action<Vector2> _rectChanged;
        public object InnerObject { get; }
        public Vector2 Position { get; private set; }

        public void SetPosition(Vector2 rect)
        {
            Position = rect;
            _rectChanged?.Invoke(rect);
        }

        public NodeItem(object innerObject, Vector2 position, Action<Vector2> rectChanged)
        {
            _rectChanged = rectChanged;
            InnerObject = innerObject;
            Position = position;
        }
    }
}