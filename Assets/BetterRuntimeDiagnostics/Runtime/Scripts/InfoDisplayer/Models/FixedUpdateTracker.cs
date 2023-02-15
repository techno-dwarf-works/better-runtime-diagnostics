using Better.Diagnostics.Runtime.InfoDisplayer.Interfaces;
using UnityEngine;

namespace Better.Diagnostics.Runtime.InfoDisplayer.Models
{
    public class FixedUpdateTracker : IDebugInfo, IFixedUpdateableInfo
    {
        private readonly Rect _position;
        private readonly GUIContent _content;
        private readonly Texture2D _whiteTexture;
        private readonly Texture2D _blackTexture;
        private bool _wasShown;

        public FixedUpdateTracker(Rect position, int trackerSize)
        {
            _position = position;
            _content = new GUIContent();
            _whiteTexture = new Texture2D(trackerSize, trackerSize, TextureFormat.RGB24, false);
            _whiteTexture.SetColor(Color.white);

            _blackTexture = new Texture2D(trackerSize, trackerSize, TextureFormat.RGB24, false);
            _blackTexture.SetColor(Color.black);
        }

        public void Initialize()
        {
        }

        public void OnGUI()
        {
            _content.image = _wasShown ? _whiteTexture : _blackTexture;
            GUI.Label(_position, _content);
        }

        public void Deconstruct()
        {
        }

        public void FixedUpdate()
        {
            _wasShown = !_wasShown;
        }
    }
}