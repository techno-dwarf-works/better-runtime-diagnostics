using UnityEngine;
using UnityEngine.UIElements;

namespace Better.Diagnostics.Runtime
{
    public static class VisualElementsFactory
    {
        public static T CreateElement<T>(Rect position, Color color) where T : VisualElement, new()
        {
            var element = new T();
            var dpi = Screen.dpi / 100f;
            element.style.height = new StyleLength(position.height / dpi);
            element.style.width = new StyleLength(position.width / dpi);
            element.style.position = new StyleEnum<Position>(Position.Absolute);
            element.transform.position = position.position;
            element.style.color = new StyleColor(color);
            element.style.fontSize = new StyleLength(11);
            return element;
        }

        public static T CreateElement<T>(Color color) where T : VisualElement, new()
        {
            var element = new T();
            element.style.position = new StyleEnum<Position>(Position.Relative);
            element.style.width = new StyleLength(StyleKeyword.Auto);
            element.style.flexGrow = new StyleFloat(1);
            element.style.color = new StyleColor(color);
            element.style.fontSize = new StyleLength(11);
            return element;
        }
    }
}