using UnityEngine;

namespace Better.Diagnostics.Runtime
{
    public static class ValidateExtension
    {
        public static bool IsNullOrDestroyed(this Object obj) {
 
            if (ReferenceEquals(obj, null)) return true;
 
            return obj == null;
        }
    }

    public static class TextureExtensions
    {
        public static void SetColor(this Texture2D texture2D, Color color)
        {
            for (int x = 0; x < texture2D.width; x++)
            {
                for (int y = 0; y < texture2D.height; y++)
                {
                    texture2D.SetPixel(x, y, color);
                }
            }
            texture2D.Apply();
        }
    }
}