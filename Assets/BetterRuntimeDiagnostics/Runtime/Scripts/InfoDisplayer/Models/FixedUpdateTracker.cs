using Better.Diagnostics.Runtime.InfoDisplayer.Interfaces;
using UnityEngine;
using UnityEngine.UIElements;

namespace Better.Diagnostics.Runtime.InfoDisplayer.Models
{
    public class FixedUpdateTracker : IDebugInfo, IFixedUpdateableInfo
    {
        private readonly Texture2D _whiteTexture;
        private readonly Texture2D _blackTexture;
        private bool _wasShown;
        private readonly Image _image;

        public FixedUpdateTracker(Rect position, int trackerSize)
        {
            _image = VisualElementsFactory.CreateElement<Image>(position, Color.white);
            _whiteTexture = new Texture2D(trackerSize, trackerSize, TextureFormat.RGB24, false);
            _whiteTexture.SetColor(Color.white);

            _blackTexture = new Texture2D(trackerSize, trackerSize, TextureFormat.RGB24, false);
            _blackTexture.SetColor(Color.black);
        }

        public void Initialize(UIDocument uiDocument)
        {
            uiDocument.rootVisualElement.Add(_image);
        }

        public void Deconstruct()
        {
        }

        public void FixedUpdate()
        {
            _image.image = _wasShown ? _whiteTexture : _blackTexture;
            _wasShown = !_wasShown;
        }
    }
}