using System;
using System.Collections.Generic;
using Better.Diagnostics.EditorAddons.NodeEditor.Models;
using UnityEngine;

namespace Better.Diagnostics.EditorAddons.NodeEditor
{
    public class NodeGroup
    {
        private Rect _rect;
        
        private readonly List<Node> _nodes;

        private readonly GUIStyle _drawingStyle;
        private readonly string _name;
        
        public event Action<NodeItem> OnRemove;
        public event Action DataChanged;

        public NodeGroup(string name, Rect rect, GUIStyle drawingStyle)
        {
            _name = name;
            _rect = rect;
            _nodes = new List<Node>();
            _drawingStyle = drawingStyle;
        }
        
        internal void Attach(Node node)
        {
            _nodes.Add(node);
            node.OnChanged += OnDataChanged;
            node.OnRemoveNode += OnClickRemoveNode;
        }

        internal void Detach(Node node)
        {
            _nodes.Remove(node);
            node.OnChanged -= OnDataChanged;
            node.OnRemoveNode -= OnClickRemoveNode;
        }

        public void SetNodeItems(IEnumerable<NodeItem> items)
        {
            foreach (var item in items)
            {
                var node = new Node(item, NodeStyles.NodeStyle, NodeStyles.SelectedNodeStyle);
                Attach(node);
            }
        }

        private void OnClickRemoveNode(BaseNode node)
        {
            node.OnChanged -= OnDataChanged;
            OnRemove?.Invoke(node.Object);
            _nodes.Remove(node as Node);
            OnDataChanged();
        }

        private void OnDataChanged()
        {
            DataChanged?.Invoke();
        }

        public void Drag(Vector2 delta)
        {
            _rect.position += delta;
            for (var i = 0; i < _nodes.Count; i++)
            {
                _nodes[i].Drag(delta);
            }
        }

        public void SetRect(Rect rect)
        {
            _rect = rect;
        }

        private bool ProcessNodeEvents(Event e)
        {
            var isChanged = false;
            for (var i = _nodes.Count - 1; i >= 0; i--)
            {
                var guiChanged = _nodes[i].ProcessEvents(e);

                if (guiChanged)
                {
                    GUI.changed = true;
                    isChanged = true;
                }
            }

            return isChanged;
        }

        public void Draw()
        {
            GUI.Box(_rect, "", _drawingStyle);

            for (var i = 0; i < _nodes.Count; i++)
            {
                _nodes[i].Draw();
            }
        }

        public bool ProcessEvents(Event e)
        {
            return ProcessNodeEvents(e);
        }

        public bool Contains(Vector2 mousePosition)
        {
            return _rect.Contains(mousePosition);
        }
    }
}