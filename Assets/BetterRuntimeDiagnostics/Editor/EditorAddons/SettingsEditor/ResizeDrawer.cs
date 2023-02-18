using UnityEditor;
using UnityEngine;

namespace Better.Diagnostics.EditorAddons.SettingsEditor
{
    public class ResizeDrawer
    {
        private MouseCursor _cursor;
        private bool _left;
        private bool _right;
        private bool _top;
        private bool _bot;

        private void SetCursor(MouseCursor cursor)
        {
            _cursor = cursor;
        }

        public void RestrictResize(ref Rect rect, Vector2 delta)
        {
            ResizeInternal(ref rect, delta);
            var singleLineHeight = NodeWindow.DefaultSize.y;
            if (rect.height < singleLineHeight)
                rect.height = singleLineHeight;
        }

        private void ResizeInternal(ref Rect rect, Vector2 delta)
        {
            if (_bot && !(_right || _left))
            {
                rect.yMax += delta.y;
            }
            else if (_top && !(_right || _left))
            {
                rect.yMin += delta.y;
            }
            else if (_right && !(_bot || _top))
            {
                rect.xMax += delta.x;
            }
            else if (_left && !(_bot || _top))
            {
                rect.xMin += delta.x;
            }
            else if (_bot && _right)
            {
                rect.max += delta;
            }
            else if (_top && _left)
            {
                rect.min += delta;
            }
            else if (_bot && _left)
            {
                rect.yMax += delta.y;
                rect.xMin += delta.x;
            }
            else if (_top && _right)
            {
                rect.yMin += delta.y;
                rect.xMax += delta.x;
            }
        }

        public void DrawCursor(Rect rect)
        {
            EditorGUIUtility.AddCursorRect(rect, _cursor);
        }

        public void Corners(bool left, bool right, bool top, bool bot)
        {
            _bot = bot;
            _top = top;
            _right = right;
            _left = left;

            if ((bot || top) && (!right || !left))
            {
                SetCursor(MouseCursor.ResizeVertical);
            }

            if ((right || left) && (!bot || !top))
            {
                SetCursor(MouseCursor.ResizeHorizontal);
            }

            if ((bot && right) || (top && left))
            {
                SetCursor(MouseCursor.ResizeUpLeft);
            }

            if ((bot && left) || (top && right))
            {
                SetCursor(MouseCursor.ResizeUpRight);
            }
        }
    }
}