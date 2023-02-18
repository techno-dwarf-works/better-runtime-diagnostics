using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Better.Extensions.Runtime;
using UnityEditor;
using UnityEngine;

namespace Better.Diagnostics.EditorAddons.SettingsEditor
{
    public class FieldsDrawer
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

            public float GetHeight()
            {
                if (FieldType == typeof(Rect))
                {
                    return 0f;
                }

                var textArea = GetCustomAttribute<TextAreaAttribute>();
                if (textArea != null)
                {
                    return EditorGUIUtility.singleLineHeight * textArea.maxLines;
                }

                return EditorGUIUtility.singleLineHeight;
            }
        }

        private readonly FieldObject[] _fieldInfos;

        private static HashSet<Type> BaseTypes = new HashSet<Type>()
        {
            typeof(int), typeof(float), typeof(string)
        };

        public event Action OnChanged;

        private FieldObject[] GetFields(object obj)
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

        public void Draw(Rect rect, GUIStyle nodeStyle)
        {
            var singleLine = EditorGUIUtility.singleLineHeight;

            var style = EditorStyles.label;
            var labelRect = new Rect(rect);
            labelRect.height = singleLine;

            var stylePadding = nodeStyle.padding;
            var styleMargin = nodeStyle.margin;
            var left = stylePadding.left + styleMargin.left;
            var right = stylePadding.right + styleMargin.right;
            var line = rect.size.x - singleLine - left - right;
            labelRect.position += new Vector2(left, stylePadding.top + styleMargin.top + singleLine);
            foreach (var fieldInfo in _fieldInfos)
            {
                if (fieldInfo.FieldType == typeof(Rect)) continue;
                var fieldName = new GUIContent(fieldInfo.FieldName);
                EditorGUI.LabelField(labelRect, fieldName);

                var size = style.CalcSize(fieldName);

                var fieldRect = new Rect(labelRect);
                fieldRect.position += new Vector2(singleLine + size.x, 0);
                fieldRect.width = line - size.x;

                var rectHeight = fieldInfo.GetHeight();
                fieldRect.height = rectHeight;
                DrawField(fieldInfo, fieldRect);

                labelRect.position += Vector2.up * (singleLine / 2f) + new Vector2(0, rectHeight);
            }
        }

        private void DrawField(FieldObject obj, Rect fieldRect)
        {
            var value = obj.GetValue();
            using (var check = new EditorGUI.ChangeCheckScope())
            {
                if (value.Cast<int>(out var intValue))
                {
                    var t = EditorGUI.IntField(fieldRect, intValue);
                    if (check.changed)
                    {
                        value = Mathf.RoundToInt(ValidateMin(obj, t));
                    }
                }
                else if (value.Cast<float>(out var floatValue))
                {
                    var range = obj.GetCustomAttribute<RangeAttribute>();
                    var t = range == null ? EditorGUI.FloatField(fieldRect, floatValue) : EditorGUI.Slider(fieldRect, floatValue, range.min, range.max);

                    if (check.changed)
                    {
                        value = ValidateMin(obj, t);
                    }
                }
                else if (value.Cast<string>(out var stringValue))
                {
                    var textArea = obj.GetCustomAttribute<TextAreaAttribute>();
                    var t = textArea == null ? EditorGUI.TextField(fieldRect, stringValue) : EditorGUI.TextArea(fieldRect, stringValue);

                    if (check.changed)
                    {
                        value = t;
                    }
                }

                if (!check.changed) return;
                obj.SetValue(value);
                OnChanged?.Invoke();
            }
        }

        private static float ValidateMin(FieldObject fieldObject, float t)
        {
            var attribute = fieldObject.GetCustomAttribute<MinAttribute>();
            if (attribute != null)
                t = Mathf.Max(attribute.min, t);
            return t;
        }

        public FieldsDrawer(object obj)
        {
            _fieldInfos = GetFields(obj);
        }

        public float GetHeight()
        {
            var height = _fieldInfos.Sum(x => x.GetHeight());
            return height;
        }
    }
}