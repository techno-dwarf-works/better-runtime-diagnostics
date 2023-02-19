using System;
using Better.Diagnostics.EditorAddons.NodeEditor.Models;
using Better.Extensions.Runtime;
using UnityEditor;
using UnityEngine;

namespace Better.Diagnostics.EditorAddons.NodeEditor
{
    internal class Node
    {
        private readonly string _title;
        private readonly GUIStyle _defaultNodeStyle;
        private readonly GUIStyle _selectedNodeStyle;

        private readonly Action<Node> _onRemoveNode;

        private readonly NodeItem _nodeItem;

        public NodeItem Object => _nodeItem;

        private Rect _rect;
        private Rect _relativeDrag;
        private bool _isDragged;
        private bool _isSelected;

        private GUIStyle _style;
        private float _resizeThickness = 10f;
        private NodeResizeDrawer _nodeResizeDrawer;
        private readonly NodeFieldsDrawer _drawer;
        private readonly GUIContent _content;
        private bool _onEdge;

        public event Action OnChanged;

        public Node(NodeItem nodeItem, GUIStyle nodeStyle, GUIStyle selectedStyle, Action<Node> onClickRemoveNode)
        {
            _nodeItem = nodeItem;
            var type = _nodeItem.InnerObject.GetType();
            _title = type.Name.PrettyCamelCase();

            _drawer = new NodeFieldsDrawer(type, nodeItem.InnerObject, false);
            _drawer.OnChanged += OnDataChanged;
            _rect = nodeItem.Position;
            _style = nodeStyle;
            _defaultNodeStyle = nodeStyle;
            _selectedNodeStyle = selectedStyle;
            _onRemoveNode = onClickRemoveNode;
            _content = new GUIContent();
        }

        private void OnDataChanged()
        {
            OnChanged?.Invoke();
        }

        private void DragInternal(Vector2 delta)
        {
            _rect.position += delta;
            _nodeItem.SetRect(_rect);
            OnDataChanged();
        }

        private void ResizeInternal(Vector2 delta)
        {
            _nodeResizeDrawer?.RestrictResize(ref _rect, delta);
            _nodeItem.SetRect(_rect);
            OnDataChanged();
        }

        private void ProcessContextMenu()
        {
            var genericMenu = new GenericMenu();
            genericMenu.AddItem(new GUIContent("Remove node"), false, OnClickRemoveNode);
            genericMenu.ShowAsContext();
        }

        private void OnClickRemoveNode()
        {
            _onRemoveNode?.Invoke(this);
        }

        public void Drag(Vector2 delta)
        {
            _relativeDrag.position += delta;
        }

        public void Draw()
        {
            var absoluteRect = AbsoluteRect();

            if (_isDragged || _isSelected)
            {
                _content.text = ($"(x:{_rect.x}, y:{_rect.y})");
                var size = EditorStyles.label.CalcSize(_content);
                var positionRect = new Rect(absoluteRect)
                {
                    size = size
                };
                positionRect.position += Vector2.down * EditorGUIUtility.singleLineHeight;
                GUI.Label(positionRect, _content);
            }

            var copyRect = new Rect(absoluteRect);
            copyRect.height += _drawer.GetHeight();
            GUI.Box(copyRect, _title, _style);
            _drawer.Draw(absoluteRect, _style);

            _nodeResizeDrawer?.DrawCursor(copyRect);
        }

        private Rect AbsoluteRect()
        {
            return new Rect(_rect.position + _relativeDrag.position, _rect.size + _relativeDrag.size);
        }

        public bool ProcessEvents(Event e)
        {
            var absoluteRect = AbsoluteRect();
            absoluteRect.height += _drawer.GetHeight();
            var mousePosition = e.mousePosition;
            switch (e.type)
            {
                case EventType.MouseDown:
                    return MouseDownProcessEvents(e, absoluteRect, mousePosition);

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

        private bool MouseDownProcessEvents(Event e, Rect absoluteRect, Vector2 mousePosition)
        {
            if (e.button == 0)
            {
                if (absoluteRect.Contains(mousePosition))
                {
                    _isDragged = true;
                    GUI.changed = true;
                    _isSelected = true;
                    _style = _selectedNodeStyle;
                    return true;
                }

                GUI.changed = true;
                _isSelected = false;
                _style = _defaultNodeStyle;
            }
            else if (e.button == 1 && _isSelected && absoluteRect.Contains(mousePosition))
            {
                ProcessContextMenu();
                e.Use();
                return true;
            }

            return false;
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
    }
}