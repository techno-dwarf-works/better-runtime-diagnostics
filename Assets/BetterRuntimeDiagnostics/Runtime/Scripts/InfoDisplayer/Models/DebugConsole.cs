using System;
using Better.Diagnostics.Runtime.CommandConsoleModule;
using Better.Diagnostics.Runtime.InfoDisplayer.Interfaces;
using UnityEngine;
using UnityEngine.UIElements;

namespace Better.Diagnostics.Runtime.InfoDisplayer.Models
{
    public class Test1
    {
        [ConsoleCommand(CommandDefinition.DefaultCommandPrefix, "run")]
        private static int Test(int value)
        {
            return value * value;
        }
    }

    [Serializable]
    public class DebugConsole : IDebugInfo, IUpdateableInfo
    {
        private KeyCode _code;

        private bool _isShown;
        private readonly VisualElement _panel;
        private readonly ScrollView _scrollView;
        private readonly TextField _inputField;


        public DebugConsole(KeyCode code)
        {
            _code = code;
            _panel = VisualElementsFactory.CreateElement<VisualElement>(Color.black);
            _panel.style.width = new StyleLength(new Length(100, LengthUnit.Percent));
            _panel.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
            _panel.style.position = new StyleEnum<Position>(Position.Absolute);
            _panel.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Column);
            _panel.style.alignContent = new StyleEnum<Align>(Align.FlexStart);
            _panel.style.alignItems = new StyleEnum<Align>(Align.FlexStart);
            _panel.style.height = new StyleLength(new Length(35, LengthUnit.Percent));

            _scrollView = VisualElementsFactory.CreateElement<ScrollView>(Color.black);
            _scrollView.style.width = new StyleLength(new Length(100, LengthUnit.Percent));
            _scrollView.style.height = new StyleLength(new Length(70, LengthUnit.Percent));
            _scrollView.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
            _scrollView.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.ColumnReverse);
            _scrollView.mode = ScrollViewMode.Vertical;
            _inputField = VisualElementsFactory.CreateElement<TextField>(Color.black);
            _inputField.style.height = new StyleLength(new Length(30, LengthUnit.Percent));
            _inputField.isDelayed = true;
            _inputField.RegisterValueChangedCallback(OnCommandChanged);

            _panel.Add(_scrollView);
            _panel.Add(_inputField);
            _isShown = false;
            _panel.visible = _isShown;
        }

        private void OnCommandChanged(ChangeEvent<string> evt)
        {
            var result = CommandRegistry.RunCommand(evt.newValue);
            foreach (var valueTuple in result)
            {
                var label = VisualElementsFactory.CreateElement<Label>(Color.white);
                label.style.whiteSpace = new StyleEnum<WhiteSpace>(WhiteSpace.Normal);
                label.text = valueTuple.Item2;
                _scrollView.Add(label);
            }
        }

        public void Initialize(UIDocument uiDocument)
        {
            var defaultConsoleCommands = new ConsoleCommands(CommandDefinition.DefaultCommandPrefix);
            CommandRegistry.AddRegistry(defaultConsoleCommands);
            uiDocument.rootVisualElement.Add(_panel);
        }

        public void Update()
        {
            if (Input.GetKeyDown(_code))
            {
                _panel.BringToFront();
                _isShown = !_isShown;
                _panel.visible = _isShown;
            }
        }

        public void Deconstruct()
        {
            CommandRegistry.RemoveRegistry(CommandDefinition.DefaultCommandPrefix);
        }
    }
}