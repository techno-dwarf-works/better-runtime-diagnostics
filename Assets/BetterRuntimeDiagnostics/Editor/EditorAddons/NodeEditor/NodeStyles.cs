using Better.Diagnostics.Runtime;
using Better.EditorTools.Helpers;
using UnityEditor;
using UnityEngine;

namespace Better.Diagnostics.EditorAddons.NodeEditor
{
    public static class NodeStyles
    {
        public static readonly GUIStyle NodeStyle;
        public static readonly GUIStyle SelectedNodeStyle;
        public static readonly GUIStyle BoxStyle;
        public static readonly GUIStyle SideBoxStyle;
        public static readonly GUIStyle SelectedSideBoxStyle;

        public static readonly Vector2 DefaultSize = new Vector2(300, EditorGUIUtility.singleLineHeight * 3f);
        public static readonly Vector2 DefaultSideSize = new Vector2(200, EditorGUIUtility.singleLineHeight * 3f);
        public static readonly Vector2 MinSize = new Vector2(150, EditorGUIUtility.singleLineHeight * 3f);
        public static readonly Color32 BackgroundColor;
        public static float SingleLine { get; } = EditorGUIUtility.singleLineHeight;
        public static float ListItemSpacing { get; } = EditorGUIUtility.singleLineHeight / 4f;

        static NodeStyles()
        {
            BackgroundColor = EditorGUIUtility.isProSkin
                ? new Color32(56, 56, 56, 255)
                : new Color32(194, 194, 194, 255);
            
            NodeStyle = new GUIStyle(EditorStyles.helpBox);
            NodeStyle.normal.textColor = Color.white;
            NodeStyle.hover.textColor = Color.white;
            NodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1.png") as Texture2D;
            NodeStyle.border = new RectOffset(12, 12, 12, 12);
            NodeStyle.padding = new RectOffset(12, 12, 10, 10);

            SelectedNodeStyle = new GUIStyle(EditorStyles.helpBox);
            SelectedNodeStyle.normal.textColor = Color.white;
            SelectedNodeStyle.hover.textColor = Color.white;
            SelectedNodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1 on.png") as Texture2D;
            SelectedNodeStyle.border = new RectOffset(12, 12, 12, 12);
            SelectedNodeStyle.padding = new RectOffset(12, 12, 10, 10);

            BoxStyle = new GUIStyle(EditorStyles.helpBox);
            
            SideBoxStyle = new GUIStyle(EditorStyles.helpBox);
            SideBoxStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node0.png") as Texture2D;
            SideBoxStyle.border = new RectOffset(12, 12, 12, 12);
            SideBoxStyle.padding = new RectOffset(12, 12, 10, 10);

            SelectedSideBoxStyle = new GUIStyle(EditorStyles.helpBox);
            SelectedSideBoxStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node0 on.png") as Texture2D;
            SelectedSideBoxStyle.border = new RectOffset(12, 12, 12, 12);
            SelectedSideBoxStyle.padding = new RectOffset(12, 12, 10, 10);
        }

    }
}