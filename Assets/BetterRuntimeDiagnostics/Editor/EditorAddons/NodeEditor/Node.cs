using Better.Diagnostics.EditorAddons.NodeEditor.Models;
using UnityEditor;
using UnityEngine;

namespace Better.Diagnostics.EditorAddons.NodeEditor
{
    internal class Node : BaseNode
    {
        private Rect _relativeDrag;
        private bool _isDragged;

        private GUIStyle _style;
        private float _resizeThickness = 10f;
        private NodeResizeDrawer _nodeResizeDrawer;
        private readonly GUIContent _content;
        private bool _onEdge;

        private void DragInternal(Vector2 delta)
        {
            var rect = base.AbsoluteRect();
            rect.position += delta;
            _nodeItem.SetRect(rect);
            SetRect(rect);
            OnDataChanged();
        }

        private void ResizeInternal(Vector2 delta)
        {
            var baseRect = base.AbsoluteRect();
            _nodeResizeDrawer?.RestrictResize(ref baseRect, delta, GetHeight());
            _nodeItem.SetRect(baseRect);
            SetRect(baseRect);
            OnDataChanged();
        }

        public void Drag(Vector2 delta)
        {
            _relativeDrag.position += delta;
        }

        public override void Draw()
        {
            var absoluteRect = AbsoluteRect();

            if (_isDragged || _isSelected)
            {
                var baseRect = base.AbsoluteRect();
                _content.text = $"(x:{baseRect.x}, y:{baseRect.y})";
                var size = EditorStyles.label.CalcSize(_content);
                size.x += EditorGUIUtility.singleLineHeight;
                var positionRect = new Rect(absoluteRect)
                {
                    size = size
                };
                positionRect.position += Vector2.down * EditorGUIUtility.singleLineHeight;
                GUI.Label(positionRect, _content);
            }

            base.Draw();
            
            _nodeResizeDrawer?.DrawCursor(absoluteRect);
        }

        public override Rect AbsoluteRect()
        {
            var rect = base.AbsoluteRect();
            return new Rect(rect.position + _relativeDrag.position, rect.size + _relativeDrag.size);
        }

        private protected override bool ProcessOthers(Event e, Rect absoluteRect, Vector2 mousePosition)
        {
            switch (e.type)
            {
                case EventType.MouseUp:
                    _isDragged = false;
                    break;

                case EventType.MouseDrag:
                    return MouseDragProcessEvents(e);

                case EventType.MouseMove:
                    return ResizeProcessEvents(absoluteRect, mousePosition);
            }

            return false;
        }

        private protected override void OnSelect()
        {
            _isDragged = true;
            base.OnSelect();
        }
        
        private bool MouseDragProcessEvents(Event e)
        {
            if (e.button == 0)
            {
                if (_isDragged)
                {
                    if (_onEdge)
                    {
                        ResizeInternal(e.delta);
                    }
                    else
                    {
                        DragInternal(e.delta);
                    }

                    e.Use();
                    return true;
                }
            }

            return false;
        }

        private bool ResizeProcessEvents(Rect absoluteRect, Vector2 mousePosition)
        {
            var maxRect = Rect.MinMaxRect(absoluteRect.xMin - _resizeThickness, absoluteRect.yMin - _resizeThickness,
                absoluteRect.xMax + _resizeThickness, absoluteRect.yMax + _resizeThickness);
            var minRect = Rect.MinMaxRect(absoluteRect.xMin + _resizeThickness, absoluteRect.yMin + _resizeThickness,
                absoluteRect.xMax - _resizeThickness, absoluteRect.yMax - _resizeThickness);

            var leftEdge = Rect.MinMaxRect(maxRect.xMin, maxRect.yMin, minRect.xMin, maxRect.yMax);
            var rightEdge = Rect.MinMaxRect(minRect.xMax, maxRect.yMin, maxRect.xMax, maxRect.yMax);
            var topEdge = Rect.MinMaxRect(maxRect.xMin, maxRect.yMin, maxRect.xMax, minRect.yMin);
            var botEdge = Rect.MinMaxRect(maxRect.xMin, minRect.yMax, maxRect.xMax, maxRect.yMax);

            _onEdge = maxRect.Contains(mousePosition) && !minRect.Contains(mousePosition);
            if (!_onEdge)
            {
                if (_nodeResizeDrawer == null) return false;
                _nodeResizeDrawer = null;
                return true;
            }

            if (_nodeResizeDrawer == null)
            {
                _nodeResizeDrawer = new NodeResizeDrawer();
            }
            
            var left = leftEdge.Contains(mousePosition);
            var right = rightEdge.Contains(mousePosition);
            var top = topEdge.Contains(mousePosition);
            var bot = botEdge.Contains(mousePosition);

            _nodeResizeDrawer.Corners(left, right, top, bot);

            return true;
        }

        public Node(NodeItem nodeItem, GUIStyle nodeStyle, GUIStyle selectedStyle) : base(nodeItem, nodeStyle, selectedStyle)
        {
            _content = new GUIContent();
        }
    }
}