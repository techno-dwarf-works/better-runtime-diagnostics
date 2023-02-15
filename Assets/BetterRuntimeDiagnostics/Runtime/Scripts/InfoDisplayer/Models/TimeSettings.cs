using Better.Diagnostics.Runtime.InfoDisplayer.Interfaces;
using UnityEditor;
using UnityEngine;

namespace Better.Diagnostics.Runtime.InfoDisplayer.Models
{
    public class TimeSettings : IDebugInfo
    {
        private readonly Rect _position;
        private readonly GUIContent _content;
        private readonly GUIContent _label;

        public TimeSettings(Rect position)
        {
            _position = position;
            _content = new GUIContent();
            _label = new GUIContent("Time scale:");
        }

        public void Initialize()
        {
        }

        public void OnGUI()
        {
            var size = GUI.skin.label.CalcSize(_label);
            GUI.Label(_position, _label);

            var timeScale = Time.timeScale;
            _content.text = timeScale.ToString("F");
            var rect = new Rect(new Vector2(_position.x + size.x + 9f, _position.y), size);
            GUI.Label(rect, _content);

            var sliderRect = new Rect(new Vector2(_position.x, _position.y + size.y + 9f), size);
            Time.timeScale = GUI.HorizontalSlider(sliderRect, timeScale, 0, 1);
        }

        public void Deconstruct()
        {
        }
    }
}