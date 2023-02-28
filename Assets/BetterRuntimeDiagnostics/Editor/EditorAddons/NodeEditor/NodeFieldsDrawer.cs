using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Better.Diagnostics.EditorAddons.NodeEditor
{
    public class NodeFieldsDrawer
    {
        private readonly FieldObject[] _fieldInfos;

        public event Action OnChanged;

        private static readonly HashSet<Type> BaseTypes = new HashSet<Type>()
        {
            typeof(int), typeof(float), typeof(string), typeof(Object), typeof(Enum), typeof(IList)
        };

        private FieldObject[] GetFields(Type type, object obj, bool allowSceneObjects)
        {
            if (obj == null) return new FieldObject[0];
            var t = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                .Where(x => x.GetCustomAttribute<SerializeField>() != null && x.FieldType != typeof(Rect)).ToArray();

            var bases = t.Where(fieldInfo => BaseTypes.Any(baseType => baseType.IsAssignableFrom(fieldInfo))).ToArray();

            var remaining = t.Except(bases);

            var next = remaining.SelectMany(x => GetFields(x.FieldType, x.GetValue(obj), allowSceneObjects));
            var toReturn = !bases.Any() ? next : bases.Select(x => new FieldObject(x, obj, allowSceneObjects)).Concat(next);
            return toReturn.ToArray();
        }

        public void Draw(Rect rect, GUIStyle nodeStyle)
        {
            var singleLine = EditorGUIUtility.singleLineHeight;

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
                var fieldName = fieldInfo.FieldName;
                EditorGUI.LabelField(labelRect, fieldName);

                var size = fieldInfo.Size;

                var fieldRect = new Rect(labelRect);
                fieldRect.position += new Vector2(singleLine + size.x, 0);
                fieldRect.width = line - size.x;

                var rectHeight = fieldInfo.GetHeight();
                fieldRect.height = rectHeight;
                var changed = fieldInfo.DrawField(fieldRect);
                if (changed)
                    OnChanged?.Invoke();

                labelRect.position += Vector2.up * (singleLine / 2f) + new Vector2(0, rectHeight);
            }
        }

        public NodeFieldsDrawer(Type type, object obj, bool allowSceneObjects)
        {
            _fieldInfos = GetFields(type, obj, allowSceneObjects);
        }

        public float GetHeight()
        {
            var height = _fieldInfos.Sum(x => x.GetHeight());
            return height;
        }
    }
}