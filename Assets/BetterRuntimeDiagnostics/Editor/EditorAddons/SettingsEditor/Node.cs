using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Better.Extensions.Runtime;
using UnityEditor;
using UnityEngine;

namespace Better.Diagnostics.EditorAddons.SettingsEditor
{
    public class Node
    {
        private class FieldObject
        {
            private readonly FieldInfo _fieldInfo;
            private readonly object _obj;
            private readonly string _fieldName;

            public string FieldName => _fieldName;

            public Type FieldType => _fieldInfo.FieldType;

            public FieldObject(FieldInfo fieldInfo, object obj)
            {
                _fieldInfo = fieldInfo;
                _fieldName = _fieldInfo.Name.PrettyCamelCase();
                _obj = obj;
            }

            public void SetValue(object value)
            {
                _fieldInfo.SetValue(_obj, value);
            }

            public object GetValue()
            {
                return _fieldInfo.GetValue(_obj);
            }

            public T GetCustomAttribute<T>() where T : Attribute
            {
                return _fieldInfo.GetCustomAttribute<T>();
            }
        }

        private readonly string _title;
        private readonly GUIStyle _defaultNodeStyle;
        private readonly GUIStyle _selectedNodeStyle;

        private readonly Action<Node> _onRemoveNode;

        private readonly NodeItem _nodeItem;

        private readonly FieldObject[] _fieldInfos;

        public NodeItem Object => _nodeItem;

        private Rect _rect;
        private Rect _relativeDrag;
        private bool _isDragged;
        private bool _isSelected;

        private GUIStyle _style;

        public Node(Vector2 position, NodeItem nodeItem, GUIStyle nodeStyle, GUIStyle selectedStyle, Action<Node> onClickRemoveNode)
        {
            _nodeItem = nodeItem;
            var type = _nodeItem.InnerObject.GetType();
            _title = type.Name.PrettyCamelCase();
            _fieldInfos = GetFields(_nodeItem.InnerObject);
            _rect = new Rect(position, NodeWindow.DefaultSize);
            _style = nodeStyle;
            _defaultNodeStyle = nodeStyle;
            _selectedNodeStyle = selectedStyle;
            _onRemoveNode = onClickRemoveNode;
        }


        private static HashSet<Type> BaseTypes = new HashSet<Type>()
        {
            typeof(int), typeof(float)
        };

        private static FieldObject[] GetFields(object obj)
        {
            var type = obj.GetType();
            var t = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                .Where(x => x.GetCustomAttribute<SerializeField>() != null).ToArray();

            var bases = t.Where(x => BaseTypes.Contains(x.FieldType)).ToArray();

            var remaining = t.Except(bases);

            var next = remaining.SelectMany(x => GetFields(x.GetValue(obj)));
            var toReturn = !bases.Any() ? next : bases.Select(x => new FieldObject(x, obj)).Concat(next);
            return toReturn.ToArray();
        }

        private void DrawByReflection(Rect rect)
        {
            var style = EditorStyles.label;
            var labelRect = new Rect(rect);
            var stylePadding = _style.padding;
            var styleMargin = _style.margin;
            var singleLine = EditorGUIUtility.singleLineHeight;
            var left = stylePadding.left + styleMargin.left;
            var right = stylePadding.right + styleMargin.right;
            var line = rect.size.x - singleLine - left - right;
            labelRect.position += new Vector2(left, stylePadding.top + styleMargin.top);

            foreach (var fieldInfo in _fieldInfos)
            {
                if (fieldInfo.FieldType == typeof(Rect)) continue;
                var fieldName = new GUIContent(fieldInfo.FieldName);
                EditorGUI.LabelField(labelRect, fieldName);

                var size = style.CalcSize(fieldName);

                var fieldRect = new Rect(labelRect);
                fieldRect.position += new Vector2(singleLine + size.x, singleLine);
                fieldRect.height = singleLine;
                fieldRect.width = line - size.x;

                DrawField(fieldInfo, fieldRect);

                labelRect.position += Vector2.up * singleLine;
            }
        }

        private void DrawField(FieldObject obj, Rect fieldRect)
        {
            var value = obj.GetValue();
            using (var check = new EditorGUI.ChangeCheckScope())
            {
                if (value is int intValue)
                {
                    var t = EditorGUI.IntField(fieldRect, intValue);
                    if (check.changed)
                    {
                        value = Mathf.RoundToInt(ValidateMin(obj, t));
                    }
                }
                else if (value is float floatValue)
                {
                    var range = obj.GetCustomAttribute<RangeAttribute>();
                    var t = range == null ? EditorGUI.FloatField(fieldRect, floatValue) : EditorGUI.Slider(fieldRect, floatValue, range.min, range.max);

                    if (check.changed)
                    {
                        value = ValidateMin(obj, t);
                    }
                }

                if (check.changed)
                {
                    obj.SetValue(value);
                }
            }
        }

        private static float ValidateMin(FieldObject fieldObject, float t)
        {
            var attribute = fieldObject.GetCustomAttribute<MinAttribute>();
            if (attribute != null)
                t = Mathf.Max(attribute.min, t);
            return t;
        }

        private void DragInternal(Vector2 delta)
        {
            _rect.position += delta;
            _nodeItem.SetPosition(_rect.position);
        }

        public void Drag(Vector2 delta)
        {
            _relativeDrag.position += delta;
        }

        public void Draw()
        {
            var absoluteRect = new Rect(_rect.position + _relativeDrag.position, _rect.size + _relativeDrag.size);

            if (_isDragged || _isSelected)
            {
                var positionRect = new Rect(absoluteRect);
                positionRect.position += Vector2.down * (EditorGUIUtility.singleLineHeight * 2);
                GUI.Label(positionRect, $"(x:{_rect.x}, y:{_rect.y})");
            }
            
            var copyRect = new Rect(absoluteRect);
            copyRect.height += _fieldInfos.Count(x => x.GetType() != typeof(Rect)) * EditorGUIUtility.singleLineHeight;
            GUI.Box(copyRect, _title, _style);
            DrawByReflection(absoluteRect);
        }

        public bool ProcessEvents(Event e)
        {
            var absoluteRect = new Rect(_rect.position + _relativeDrag.position, _rect.size + _relativeDrag.size);
            switch (e.type)
            {
                case EventType.MouseDown:
                    if (e.button == 0)
                    {
                        if (absoluteRect.Contains(e.mousePosition))
                        {
                            _isDragged = true;
                            GUI.changed = true;
                            _isSelected = true;
                            _style = _selectedNodeStyle;
                            return true;
                        }
                        else
                        {
                            GUI.changed = true;
                            _isSelected = false;
                            _style = _defaultNodeStyle;
                        }
                    }
                    else if (e.button == 1 && _isSelected && absoluteRect.Contains(e.mousePosition))
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
            }

            return false;
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
    }
}