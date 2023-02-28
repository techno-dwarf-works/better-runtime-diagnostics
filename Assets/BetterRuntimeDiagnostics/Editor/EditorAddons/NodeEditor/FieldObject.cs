using System;
using System.Collections;
using System.Collections.Generic;
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
        private int _listLines;
        private bool _isList;

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

            if (GetValue() is IList list)
            {
                _isList = true;
                _listLines = list.Count + 2;
            } 
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
                return EditorGUIUtility.singleLineHeight * _listLines * textArea.maxLines;
            }

            return EditorGUIUtility.singleLineHeight * _listLines;
        }

        public bool DrawField(Rect fieldRect)
        {
            (bool, object) t = new(false, null);
            if (_isList)
            {
                var list = (IList)GetValue();
                for (var index = 0; index < list.Count; index++)
                {
                    var value = list[index];
                    var item = DrawFieldInternal(fieldRect, _fieldType.GenericTypeArguments[0], value);
                    if (!item.Item1) continue;
                    t = (true, list);
                    list[index] = item.Item2;
                }
            }
            else
            {
                t = DrawFieldInternal(fieldRect, _fieldType, GetValue());
            }

            if (t.Item1)
            {
                SetValue(t.Item2);
            }

            return t.Item1;
        }

        private (bool, object) DrawFieldInternal(Rect fieldRect, Type fieldType, object value)
        {
            using (var check = new EditorGUI.ChangeCheckScope())
            {
                if (fieldType == typeof(int) && value.Cast<int>(out var intValue))
                {
                    var t = EditorGUI.IntField(fieldRect, intValue);
                    if (check.changed)
                    {
                        value = Mathf.RoundToInt(ValidateMin(t));
                    }
                }
                else if (fieldType == typeof(float) && value.Cast<float>(out var floatValue))
                {
                    var range = GetCustomAttribute<RangeAttribute>();
                    var t = range == null ? EditorGUI.FloatField(fieldRect, floatValue) : EditorGUI.Slider(fieldRect, floatValue, range.min, range.max);

                    if (check.changed)
                    {
                        value = ValidateMin(t);
                    }
                }
                else if (fieldType == typeof(string) && value.Cast<string>(out var stringValue))
                {
                    var textArea = GetCustomAttribute<TextAreaAttribute>();
                    var t = textArea == null ? EditorGUI.TextField(fieldRect, stringValue) : EditorGUI.TextArea(fieldRect, stringValue);

                    if (check.changed)
                    {
                        value = t;
                    }
                }
                else if (fieldType.IsSubclassOf(typeof(Object)) && value.Cast<Object>(out var obj))
                {
                    var newObj = EditorGUI.ObjectField(fieldRect, obj, _fieldInfo.FieldType, _allowSceneObjects);
                    if (check.changed)
                    {
                        value = newObj;
                    }
                }
                else if (fieldType.IsSubclassOf(typeof(Enum)) && value.Cast<Enum>(out var eEnum))
                {
                    var flagsAttribute = GetCustomAttribute<FlagsAttribute>();
                    var newValue = flagsAttribute == null ? EditorGUI.EnumPopup(fieldRect, eEnum) : EditorGUI.EnumFlagsField(fieldRect, eEnum);
                    if (check.changed)
                    {
                        value = newValue;
                    }
                }

                return check.changed ? (true, value) : (false, null);
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