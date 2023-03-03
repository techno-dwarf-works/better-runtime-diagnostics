using System;
using Better.Diagnostics.EditorAddons.NodeEditor.Models;
using Better.Extensions.Runtime;
using UnityEditor;
using UnityEngine;

namespace Better.Diagnostics.EditorAddons.NodeEditor
{
    internal class BaseNode
    {
        private protected readonly string _title;
        private readonly GUIStyle _defaultNodeStyle;
        private readonly GUIStyle _selectedNodeStyle;

        public event Action<BaseNode> OnRemoveNode;

        private protected readonly NodeItem _nodeItem;

        public NodeItem Object => _nodeItem;

        private Rect _rect;

        private GUIStyle _style;
        private protected readonly NodeFieldsDrawer _drawer;
        private bool _onEdge;
        private protected bool _isSelected;

        public event Action OnChanged;

        public BaseNode(NodeItem nodeItem, GUIStyle nodeStyle, GUIStyle selectedStyle)
        {
            _nodeItem = nodeItem;
            var type = _nodeItem.InnerObject.GetType();
            _title = type.Name.PrettyCamelCase();

            _drawer = new NodeFieldsDrawer(type, nodeItem.InnerObject, false);
            _drawer.OnChanged += OnDataChanged;
            _rect = nodeItem.Position;
            
            var rectHeight = _drawer.GetHeight();
            var isHeightValid = NodeStyles.MinSize.y + rectHeight <= _rect.height;
            if (!isHeightValid)
            {
                _rect.height += rectHeight;
            }
            
            _style = nodeStyle;
            _defaultNodeStyle = nodeStyle;
            _selectedNodeStyle = selectedStyle;
        }
        
        private protected GUIStyle GetCurrentStyle()
        {
            return _style;
        }
        
        private protected void OnDataChanged()
        {
            ValidateRect();
            OnRectChanged();
        }

        private protected void OnRectChanged()
        {
            OnChanged?.Invoke();
        }

        public void SetRect(Rect rect)
        {
            _rect = rect;
        }

        private protected virtual void ValidateRect()
        {
            var heightDelta = NodeStyles.MinSize.y + _drawer.GetHeight() - _rect.height;
            
            _rect.height += heightDelta;
        }

        private protected virtual void ProcessContextMenu(Event e)
        {
            var genericMenu = new GenericMenu();
            genericMenu.AddItem(new GUIContent("Remove node"), false, OnClickRemoveNode);
            genericMenu.ShowAsContext();

            e.Use();
        }

        private void OnClickRemoveNode()
        {
            OnRemoveNode?.Invoke(this);
        }

        public virtual void Draw()
        {
            var absoluteRect = AbsoluteRect();

            GUI.Box(absoluteRect, _title, GetCurrentStyle());
            _drawer.Draw(absoluteRect, GetCurrentStyle());
        }

        public float GetHeight()
        {
            return _drawer.GetHeight();
        }

        public virtual Rect AbsoluteRect()
        {
            return _rect;
        }

        public bool ProcessEvents(Event e)
        {
            var absoluteRect = AbsoluteRect();
            var mousePosition = e.mousePosition;
            switch (e.type)
            {
                case EventType.MouseDown:
                    return MouseDownProcessEvents(e, absoluteRect, mousePosition);
                default:
                    return ProcessOthers(e, absoluteRect, mousePosition);
            }
        }

        private protected virtual bool ProcessOthers(Event e, Rect absoluteRect, Vector2 mousePosition)
        {
            return false;
        }

        private bool MouseDownProcessEvents(Event e, Rect absoluteRect, Vector2 mousePosition)
        {
            if (e.button == 0)
            {
                if (absoluteRect.Contains(mousePosition))
                {
                    OnSelect();
                    return true;
                }

                OnDeselect();
            }
            else if (e.button == 1 && _isSelected && absoluteRect.Contains(mousePosition))
            {
                ProcessContextMenu(e);
                return true;
            }

            return false;
        }

        private protected virtual void OnDeselect()
        {
            GUI.changed = true;
            _isSelected = false;
            _style = _defaultNodeStyle;
        }

        private protected virtual void OnSelect()
        {
            GUI.changed = true;
            _isSelected = true;
            _style = _selectedNodeStyle;
        }
    }
}