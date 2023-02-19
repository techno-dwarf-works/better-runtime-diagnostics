using System;
using System.Reflection;
using Better.Extensions.Runtime;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Better.Diagnostics.EditorAddons.NodeEditor
{
    public class FieldObject
    {
        private readonly FieldInfo _fieldInfo;
        private readonly object _obj;
        private readonly bool _allowSceneObjects;
        private readonly Type _fieldType;

        public GUIContent FieldName { get; }
        public Vector2 Size { get; }

        public FieldObject(FieldInfo fieldInfo, object obj, bool allowSceneObjects)
        {
            _fieldInfo = fieldInfo;
            _fieldType = fieldInfo.FieldType;
            FieldName = new GUIContent(_fieldInfo.Name.PrettyCamelCase());
            var style = EditorStyles.label;
            Size = style.CalcSize(FieldName);
            _obj = obj;
            _allowSceneObjects = allowSceneObjects;
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
            var textArea = GetCustomAttribute<TextAreaAttribute>();
            if (textArea != null)
            {
                return EditorGUIUtility.singleLineHeight * textArea.maxLines;
            }

            return EditorGUIUtility.singleLineHeight;
        }

        public bool DrawField(Rect fieldRect)
        {
            var value = GetValue();
            using (var check = new EditorGUI.ChangeCheckScope())
            {
                if (_fieldType == typeof(int) && value.Cast<int>(out var intValue))
                {
                    var t = EditorGUI.IntField(fieldRect, intValue);
                    if (check.changed)
                    {
                        value = Mathf.RoundToInt(ValidateMin(t));
                    }
                }
                else if (_fieldType == typeof(float) && value.Cast<float>(out var floatValue))
                {
                    var range = GetCustomAttribute<RangeAttribute>();
                    var t = range == null ? EditorGUI.FloatField(fieldRect, floatValue) : EditorGUI.Slider(fieldRect, floatValue, range.min, range.max);

                    if (check.changed)
                    {
                        value = ValidateMin(t);
                    }
                }
                else if (_fieldType == typeof(string) && value.Cast<string>(out var stringValue))
                {
                    var textArea = GetCustomAttribute<TextAreaAttribute>();
                    var t = textArea == null ? EditorGUI.TextField(fieldRect, stringValue) : EditorGUI.TextArea(fieldRect, stringValue);

                    if (check.changed)
                    {
                        value = t;
                    }
                }
                else if (_fieldType.IsSubclassOf(typeof(Object)) && value.Cast<Object>(out var obj))
                {
                    var newObj = EditorGUI.ObjectField(fieldRect, obj, _fieldInfo.FieldType, _allowSceneObjects);
                    if (check.changed)
                    {
                        value = newObj;
                    }
                }

                if (!check.changed) return false;
                SetValue(value);
                return true;
            }
        }


        private float ValidateMin(float t)
        {
            var attribute = GetCustomAttribute<MinAttribute>();
            if (attribute != null)
                t = Mathf.Max(attribute.min, t);
            return t;
        }
    }
}