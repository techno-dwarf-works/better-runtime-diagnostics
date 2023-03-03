using System;
using System.Collections;
using System.Reflection;
using Better.Extensions.Runtime;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Better.Diagnostics.EditorAddons.NodeEditor
{
    public class BakedFieldData
    {
        public TextAreaAttribute TextAreaAttribute { get; }
        public RangeAttribute RangeAttribute { get; }
        public FlagsAttribute FlagsAttribute { get; }
        public MinAttribute MinAttribute { get; }

        public BakedFieldData(FieldInfo fieldInfo)
        {
            TextAreaAttribute = GetCustomAttribute<TextAreaAttribute>(fieldInfo);
            RangeAttribute = GetCustomAttribute<RangeAttribute>(fieldInfo);
            FlagsAttribute = GetCustomAttribute<FlagsAttribute>(fieldInfo);
            MinAttribute = GetCustomAttribute<MinAttribute>(fieldInfo);
        }

        private T GetCustomAttribute<T>(FieldInfo fieldInfo) where T : Attribute
        {
            return fieldInfo.GetCustomAttribute<T>();
        }
    }

    public class FieldObject
    {
        private readonly FieldInfo _fieldInfo;
        private readonly object _obj;
        private readonly bool _allowSceneObjects;
        private readonly Type _fieldType;
        private readonly bool _isList;

        private readonly BakedFieldData _fieldData;

        public GUIContent FieldName { get; }
        public Vector2 Size { get; }

        public FieldObject(FieldInfo fieldInfo, object obj, bool allowSceneObjects)
        {
            _fieldInfo = fieldInfo;
            _fieldData = new BakedFieldData(fieldInfo);
            _fieldType = fieldInfo.FieldType;
            FieldName = new GUIContent(_fieldInfo.Name.PrettyCamelCase());
            var style = EditorStyles.label;
            Size = style.CalcSize(FieldName);
            _obj = obj;
            _allowSceneObjects = allowSceneObjects;

            if (GetValue() is IList)
            {
                _isList = true;
            }
        }

        private void SetValue(object value)
        {
            _fieldInfo.SetValue(_obj, value);
        }

        private object GetValue()
        {
            return _fieldInfo.GetValue(_obj);
        }

        public float GetHeight()
        {
            var textArea = _fieldData.TextAreaAttribute;

            float singleLine;

            if (_isList)
            {
                var count = Count();
                singleLine = NodeStyles.SingleLine * count + NodeStyles.SingleLine + NodeStyles.ListItemSpacing * (count - 1);
            }
            else
            {
                singleLine = NodeStyles.SingleLine;
            }

            if (textArea != null)
            {
                return singleLine * textArea.maxLines;
            }

            return singleLine;
        }

        public float GetExternalHeight()
        {
            return GetHeight() + (_isList ? NodeStyles.SingleLine / 2f : 0);
        }

        private int Count()
        {
            var count = ((IList)GetValue()).Count;
            count = count == 0 ? 1 : count;
            return count;
        }

        public bool DrawField(Rect fieldRect)
        {
            (bool, object) fieldInternal = new(false, null);
            if (_isList)
            {
                var list = (IList)GetValue();
                for (var index = 0; index < list.Count; index++)
                {
                    var value = list[index];
                    if (index > 0)
                    {
                        fieldRect.position += Vector2.up * (NodeStyles.SingleLine + NodeStyles.ListItemSpacing);
                    }

                    var item = DrawFieldInternal(fieldRect, FieldTypeGenericTypeArgument(), value);
                    if (!item.Item1) continue;
                    fieldInternal = (true, list);
                    list[index] = item.Item2;
                }

                var isChanged = DrawListButtons(fieldRect);
                if (isChanged)
                {
                    fieldInternal = new(true, list);
                }
            }
            else
            {
                fieldInternal = DrawFieldInternal(fieldRect, _fieldType, GetValue());
            }

            if (fieldInternal.Item1)
            {
                SetValue(fieldInternal.Item2);
            }

            return fieldInternal.Item1;
        }

        private bool DrawListButtons(Rect fieldRect)
        {
            var isChanged = false;
            fieldRect.height = NodeStyles.SingleLine;
            fieldRect.position += Vector2.up * (NodeStyles.SingleLine + NodeStyles.ListItemSpacing);
            fieldRect.width /= 2f;
            if (GUI.Button(fieldRect, "+"))
            {
                var list = (IList)GetValue();
                if (list == null)
                {
                    list = (IList)Activator.CreateInstance(_fieldType);
                    SetValue(list);
                }

                list.Add(FieldTypeGenericTypeArgument().GetDefaultValue());
                isChanged = true;
            }

            fieldRect.position += Vector2.right * fieldRect.width;
            if (GUI.Button(fieldRect, "-"))
            {
                var list = (IList)GetValue();
                if (list.Count > 0)
                {
                    list.RemoveAt(list.Count - 1);
                }

                isChanged = true;
            }

            return isChanged;
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
                    var range = _fieldData.RangeAttribute;
                    var t = range == null ? EditorGUI.FloatField(fieldRect, floatValue) : EditorGUI.Slider(fieldRect, floatValue, range.min, range.max);

                    if (check.changed)
                    {
                        value = ValidateMin(t);
                    }
                }
                else if (fieldType == typeof(string) && value.Cast<string>(out var stringValue))
                {
                    var textArea = _fieldData.TextAreaAttribute;
                    string t;
                    if (textArea == null)
                    {
                        t = EditorGUI.TextField(fieldRect, stringValue);
                    }
                    else
                    {
                        fieldRect.height = textArea.maxLines;
                        t = EditorGUI.TextArea(fieldRect, stringValue);
                    }

                    if (check.changed)
                    {
                        value = t;
                    }
                }
                else if (fieldType.IsSubclassOf(typeof(Object)) && value.Cast<Object>(out var obj))
                {
                    var newObj = EditorGUI.ObjectField(fieldRect, obj, fieldType, _allowSceneObjects);
                    if (check.changed)
                    {
                        value = newObj;
                    }
                }
                else if (fieldType.IsSubclassOf(typeof(Enum)) && value.Cast<Enum>(out var eEnum))
                {
                    var flagsAttribute = _fieldData.FlagsAttribute;
                    var newValue = flagsAttribute == null ? EditorGUI.EnumPopup(fieldRect, eEnum) : EditorGUI.EnumFlagsField(fieldRect, eEnum);
                    if (check.changed)
                    {
                        value = newValue;
                    }
                }

                return check.changed ? (true, value) : (false, null);
            }
        }

        private Type FieldTypeGenericTypeArgument()
        {
            return _fieldType.GenericTypeArguments[0];
        }


        private float ValidateMin(float t)
        {
            var attribute =_fieldData.MinAttribute;
            if (attribute != null)
                t = Mathf.Max(attribute.min, t);
            return t;
        }
    }
}