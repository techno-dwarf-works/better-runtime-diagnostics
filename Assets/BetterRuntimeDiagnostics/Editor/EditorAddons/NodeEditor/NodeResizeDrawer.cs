using System;
using UnityEditor;
using UnityEngine;

namespace Better.Diagnostics.EditorAddons.NodeEditor
{
    public class NodeResizeDrawer
    {
        private MouseCursor _cursor;
        private ResizeDirection _direction;

        private void SetCursor(MouseCursor cursor)
        {
            _cursor = cursor;
        }

        public void RestrictResize(ref Rect rect, Vector2 delta)
        {
            ResizeInternal(ref rect, delta);
            var defaultSize = NodeWindow.MinSize;
            if (rect.height < defaultSize.y)
                rect.height = defaultSize.y;

            if (rect.width < defaultSize.x)
                rect.width = defaultSize.x;
        }

        private void ResizeInternal(ref Rect rect, Vector2 delta)
        {
            if (_direction.HasFlag(ResizeDirection.Bottom))
            {
                rect.yMax += delta.y;
            }

            if (_direction.HasFlag(ResizeDirection.Top))
            {
                rect.yMin += delta.y;
            }

            if (_direction.HasFlag(ResizeDirection.Right))
            {
                rect.xMax += delta.x;
            }

            if (_direction.HasFlag(ResizeDirection.Left))
            {
                rect.xMin += delta.x;
            }
        }

        [Flags]
        private enum ResizeDirection
        {
            None = 0,
            Top = 1,
            Bottom = 2,
            Left = 4,
            Right = 8
        }

        private ResizeDirection GetResizeDirection(bool left, bool right, bool top, bool bot)
        {
            var direction = ResizeDirection.None;

            if (top)
                direction |= ResizeDirection.Top;

            if (bot)
                direction |= ResizeDirection.Bottom;

            if (left)
                direction |= ResizeDirection.Left;

            if (right)
                direction |= ResizeDirection.Right;

            return direction;
        }

        public void DrawCursor(Rect rect)
        {
            EditorGUIUtility.AddCursorRect(rect, _cursor);
        }

        public void Corners(bool left, bool right, bool top, bool bot)
        {
            _direction = GetResizeDirection(left, right, top, bot);

            if (_direction == ResizeDirection.Bottom || _direction == ResizeDirection.Top)
            {
                SetCursor(MouseCursor.ResizeVertical);
            }

            if (_direction == ResizeDirection.Right || _direction == ResizeDirection.Left)
            {
                SetCursor(MouseCursor.ResizeHorizontal);
            }

            if (_direction == (ResizeDirection.Bottom | ResizeDirection.Right) || _direction == (ResizeDirection.Top | ResizeDirection.Left))
            {
                SetCursor(MouseCursor.ResizeUpLeft);
            }

            if (_direction == (ResizeDirection.Bottom | ResizeDirection.Left) || _direction == (ResizeDirection.Top | ResizeDirection.Right))
            {
                SetCursor(MouseCursor.ResizeUpRight);
            }
        }
    }
}