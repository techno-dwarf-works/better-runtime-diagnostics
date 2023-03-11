using Better.Diagnostics.Runtime.InfoDisplayer.Interfaces;
using Better.Diagnostics.Runtime.InfoDisplayer.Utils;
using UnityEngine;
using UnityEngine.UIElements;

namespace Better.Diagnostics.Runtime.InfoDisplayer.Models
{
    public abstract class BaseFrameCounter : IDebugInfo, IUpdateableInfo
    {
        private float _deltaTime;
        private float _lastFPS;
        private float _nextUpdate;
        private readonly UpdateTimer _updateTimer;
        private readonly Label _label;

        public BaseFrameCounter(Rect position, UpdateInterval updateInterval)
        {
            _label = VisualElementsFactory.CreateElement<Label>(position, Color.white);
            _updateTimer = new UpdateTimer(updateInterval, OnUpdate);
        }

        public virtual void Initialize(UIDocument uiDocument)
        {
            uiDocument.rootVisualElement.Add(_label);
        }

        public virtual void Update()
        {
            _updateTimer.Update();
        }

        private void OnUpdate()
        {
            _lastFPS = DisplayFPS();
            _label.text = ContentText(_lastFPS);
        }

        private protected abstract string ContentText(float fps);

        private protected abstract float DisplayFPS();

        public virtual void Deconstruct()
        {
        }
    }
}