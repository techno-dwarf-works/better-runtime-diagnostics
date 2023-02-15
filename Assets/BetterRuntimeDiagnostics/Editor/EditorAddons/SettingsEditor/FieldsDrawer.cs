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
        }

        private readonly FieldObject[] _fieldInfos;

        private static HashSet<Type> BaseTypes = new HashSet<Type>()
        {
            typeof(int), typeof(float)
        };
        
        public event Action OnChanged;

        private FieldObject[] GetFields(object obj)
        {
            var type = obj.GetType();
            var t = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                .Where(x => CustomAttributeExtensions.GetCustomAttribute<SerializeField>((MemberInfo)x) != null).ToArray();

            var bases = t.Where(x => BaseTypes.Contains(x.FieldType)).ToArray();

            var remaining = t.Except(bases);

            var next = remaining.SelectMany(x => GetFields(x.GetValue(obj)));
            var toReturn = !bases.Any() ? next : bases.Select(x => new FieldObject(x, obj)).Concat(next);
            return toReturn.ToArray();
        }

        public void Draw(Rect rect, GUIStyle nodeStyle)
        {
            var style = EditorStyles.label;
            var labelRect = new Rect(rect);
            var stylePadding = nodeStyle.padding;
            var styleMargin = nodeStyle.margin;
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
                    OnChanged?.Invoke();
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

        public FieldsDrawer(object obj)
        {
            _fieldInfos = GetFields(obj);
        }

        public float GetHeight()
        {
            return _fieldInfos.Count(x => x.GetType() != typeof(Rect)) * EditorGUIUtility.singleLineHeight;
        }

    }
}