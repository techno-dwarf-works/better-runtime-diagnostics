using UnityEngine;

namespace Better.Diagnostics.Runtime
{
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
        
        public static void SetColor(this Texture2D texture2D, Color32 color)
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