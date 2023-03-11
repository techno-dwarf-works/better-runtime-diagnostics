using System;
using Better.Diagnostics.Runtime.InfoDisplayer.Interfaces;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Better.Diagnostics.Runtime.InfoDisplayer.Models
{
    public class TimeSettings : IDebugInfo, IUpdateableInfo
    {
        private readonly Label _label;
        private readonly Slider _slider;
        private readonly VisualElement _panel;
        private float _currentTimeScale;

        public TimeSettings(Rect position)
        {
            _panel = VisualElementsFactory.CreateElement<VisualElement>(position, Color.white);
            _panel.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
            _panel.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Column);
            _panel.style.alignContent = new StyleEnum<Align>(Align.FlexStart);
            _panel.style.alignItems = new StyleEnum<Align>(Align.FlexStart);
            _label = VisualElementsFactory.CreateElement<Label>(Color.white);
            
            SetCurrentScale(Time.timeScale);
            _panel.Add(_label);
            
            _slider = VisualElementsFactory.CreateElement<Slider>(Color.white);
            _slider.highValue = 1f;
            _slider.lowValue = 0f;
            _slider.value = _currentTimeScale;
            _slider.RegisterValueChangedCallback(OnChanged);
            _panel.Add(_slider);
            
        }

        private void SetCurrentScale(float currentTimeScale)
        {
            _currentTimeScale = currentTimeScale;
            _label.text = $"Time scale: {currentTimeScale}";
        }

        private void OnChanged(ChangeEvent<float> eventData)
        {
            var timeScale = eventData.newValue;
            Time.timeScale = timeScale;
            SetCurrentScale(timeScale);
        }

        public void Initialize(UIDocument uiDocument)
        {
            uiDocument.rootVisualElement.Add(_panel);
        }

        public void Update()
        {
            if (Math.Abs(_currentTimeScale - Time.timeScale) > float.Epsilon)
            {
                SetCurrentScale(Time.timeScale);
                _slider.SetValueWithoutNotify(_currentTimeScale);
            }
        }

        public void Deconstruct()
        {
        }
    }
}