using System;
using Better.Extensions.Runtime;
using UnityEditor;
using UnityEngine;

namespace Better.Diagnostics.EditorAddons.SettingsEditor
{
    public class Node
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
        private readonly FieldsDrawer _drawer;
        private float _resizeThickness = 10f;
        private bool _onEdge;
        private bool _horizontal;
        private bool _vertical;

        public event Action OnChanged;

        public Node(NodeItem nodeItem, GUIStyle nodeStyle, GUIStyle selectedStyle, Action<Node> onClickRemoveNode)
        {
            _nodeItem = nodeItem;
            var type = _nodeItem.InnerObject.GetType();
            _title = type.Name.PrettyCamelCase();
            _drawer = new FieldsDrawer(nodeItem.InnerObject);
            _drawer.OnChanged += OnDataChanged;
            _rect = nodeItem.Position;
            _style = nodeStyle;
            _defaultNodeStyle = nodeStyle;
            _selectedNodeStyle = selectedStyle;
            _onRemoveNode = onClickRemoveNode;
        }

        private void OnDataChanged()
        {
            OnChanged?.Invoke();
        }

        private void DragInternal(Vector2 delta)
        {
            _rect.position += delta;
            _nodeItem.SetPosition(_rect);
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
                var positionRect = new Rect(absoluteRect);
                positionRect.position += Vector2.down * (EditorGUIUtility.singleLineHeight * 2f);
                GUI.Label(positionRect, $"(x:{_rect.x}, y:{_rect.y})");
            }

            var copyRect = new Rect(absoluteRect);
            copyRect.height += _drawer.GetHeight();
            GUI.Box(copyRect, _title, _style);
            _drawer.Draw(absoluteRect, _style);

            if (_onEdge)
            {
                var cursorType = MouseCursor.Orbit;
                if (_horizontal && _vertical)
                {
                    cursorType = MouseCursor.ScaleArrow;
                }
                else if (_horizontal)
                {
                    cursorType = MouseCursor.ResizeHorizontal;
                }
                else if (_vertical)
                {
                    cursorType = MouseCursor.ResizeVertical;
                }

                EditorGUIUtility.AddCursorRect(copyRect, cursorType);
            }
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

                    break;

                case EventType.MouseUp:
                    _isDragged = false;
                    break;

                case EventType.MouseDrag:
                    if (e.button == 0 && _isDragged)
                    {
                        DragInternal(e.delta);
                        e.Use();
                        return true;
                    }

                    break;
                case EventType.MouseMove:


                    var maxRect = Rect.MinMaxRect(absoluteRect.xMin - _resizeThickness, absoluteRect.yMin - _resizeThickness,
                        absoluteRect.xMax + _resizeThickness, absoluteRect.yMax + _resizeThickness);
                    var minRect = Rect.MinMaxRect(absoluteRect.xMin + _resizeThickness, absoluteRect.yMin + _resizeThickness,
                        absoluteRect.xMax - _resizeThickness, absoluteRect.yMax - _resizeThickness);

                    var leftEdge = Rect.MinMaxRect(maxRect.xMin, maxRect.yMin, minRect.xMin, maxRect.yMax);
                    var rightEdge = Rect.MinMaxRect(minRect.xMax, maxRect.yMin, maxRect.xMax, maxRect.yMax);
                    var topEdge = Rect.MinMaxRect(maxRect.xMin, maxRect.yMin, maxRect.xMax, minRect.yMin);
                    var botEdge = Rect.MinMaxRect(maxRect.xMin, minRect.yMax, maxRect.xMax, maxRect.yMax);

                    _onEdge = maxRect.Contains(mousePosition) && !minRect.Contains(mousePosition);
                    _horizontal = leftEdge.Contains(mousePosition) || rightEdge.Contains(mousePosition);
                    _vertical = topEdge.Contains(mousePosition) || botEdge.Contains(mousePosition);
                    return true;
            }

            return false;
        }
    }
}