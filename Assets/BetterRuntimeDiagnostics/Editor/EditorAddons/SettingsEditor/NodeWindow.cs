using System;
using System.Collections.Generic;
using System.Reflection;
using Better.Extensions.Runtime;
using UnityEditor;
using UnityEngine;

namespace Better.Diagnostics.EditorAddons.SettingsEditor
{
    public class NodeWindow : EditorWindow
    {
        private List<Node> _nodes;

        private GUIStyle _nodeStyle;
        private GUIStyle _selectedNodeStyle;
        private Vector2 _drag;
        private Vector2 _offset;
        private Type[] _menuList;
        private Action<NodeItem> _onCreate;
        private Action<NodeItem> _onRemove;

        public static readonly Vector2 DefaultSize = new Vector2(300, 50);
        private List<NodeGroup> _groups;

        public static NodeWindow OpenWindow()
        {
            var window = GetWindow<NodeWindow>();
            window.titleContent = new GUIContent("Node Based Editor");
            window._menuList = null;
            window._nodes = null;
            return window;
        }

        private void Awake()
        {
            _nodeStyle = new GUIStyle(EditorStyles.helpBox);
            _nodeStyle.normal.textColor = Color.white;
            _nodeStyle.hover.textColor = Color.white;
            _nodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1.png") as Texture2D;
            _nodeStyle.border = new RectOffset(12, 12, 12, 12);
            _nodeStyle.padding = new RectOffset(12, 12, 10, 10);

            _selectedNodeStyle = new GUIStyle(EditorStyles.helpBox);
            _selectedNodeStyle.normal.textColor = Color.white;
            _selectedNodeStyle.hover.textColor = Color.white;
            _selectedNodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1 on.png") as Texture2D;
            _selectedNodeStyle.border = new RectOffset(12, 12, 12, 12);
            _selectedNodeStyle.padding = new RectOffset(12, 12, 10, 10);
        }

        private void OnGUI()
        {
            DrawGrid(20, 0.2f, Color.gray);
            DrawGrid(100, 0.4f, Color.gray);
            
            DrawGroups();
            DrawNodes();

            ProcessNodeEvents(Event.current);
            ProcessEvents(Event.current);

            if (GUI.changed) Repaint();
        }

        private void DrawGrid(float gridSpacing, float gridOpacity, Color gridColor)
        {
            var widthDivs = Mathf.CeilToInt(position.width / gridSpacing);
            var heightDivs = Mathf.CeilToInt(position.height / gridSpacing);

            Handles.BeginGUI();
            Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);

            _offset += _drag * 0.5f;
            var newOffset = new Vector3(_offset.x % gridSpacing, _offset.y % gridSpacing, 0);

            for (var i = 0; i < widthDivs; i++)
            {
                Handles.DrawLine(new Vector3(gridSpacing * i, -gridSpacing, 0) + newOffset, new Vector3(gridSpacing * i, position.height, 0f) + newOffset);
            }

            for (var j = 0; j < heightDivs; j++)
            {
                Handles.DrawLine(new Vector3(-gridSpacing, gridSpacing * j, 0) + newOffset, new Vector3(position.width, gridSpacing * j, 0f) + newOffset);
            }

            Handles.color = Color.white;
            Handles.EndGUI();
        }

        private void DrawNodes()
        {
            if (_nodes == null) return;
            for (var i = 0; i < _nodes.Count; i++)
            {
                _nodes[i].Draw();
            }
        }

        private void DrawGroups()
        {
            if (_groups == null) return;
            for (var i = 0; i < _groups.Count; i++)
            {
                _groups[i].Draw();
            }
        }

        private void ProcessEvents(Event e)
        {
            _drag = Vector2.zero;
            switch (e.type)
            {
                case EventType.MouseDown:
                    if (e.button == 1)
                    {
                        ProcessContextMenu(e.mousePosition);
                    }

                    break;

                case EventType.MouseDrag:
                    if (e.button == 0)
                    {
                        OnDrag(e.delta);
                        _drag = e.delta;
                    }

                    break;
            }
        }

        private void OnDrag(Vector2 delta)
        {
            if (_nodes != null)
            {
                for (var i = 0; i < _nodes.Count; i++)
                {
                    _nodes[i].Drag(delta);
                }
            }

            if (_groups != null)
            {
                for (var i = 0; i < _groups.Count; i++)
                {
                    _groups[i].Drag(delta);
                }
            }

            GUI.changed = true;
        }

        private void ProcessNodeEvents(Event e)
        {
            if (_nodes != null)
            {
                Node changed = null;
                for (var i = _nodes.Count - 1; i >= 0; i--)
                {
                    var guiChanged = _nodes[i].ProcessEvents(e);

                    if (guiChanged)
                    {
                        changed = _nodes[i];
                        GUI.changed = true;
                        break;
                    }
                }

                if (changed == null) return;
                _nodes.Remove(changed);
                _nodes.Add(changed);
            }
        }

        private void ProcessContextMenu(Vector2 mousePosition)
        {
            var genericMenu = new GenericMenu();
            if (_menuList == null) return;
            for (var i = 0; i < _menuList.Length; i++)
            {
                var menu = _menuList[i];
                genericMenu.AddItem(new GUIContent($"Add {menu.Name.PrettyCamelCase()}"), false, () => OnClickAddNode(menu, mousePosition));
            }

            genericMenu.ShowAsContext();
        }

        private void OnClickAddNode(Type nodeType, Vector2 mousePosition)
        {
            if (_nodes == null)
            {
                _nodes = new List<Node>();
            }

            var instance = Activator.CreateInstance(nodeType);

            var method = nodeType.GetMethod("SetPosition", BindingFlags.Public | BindingFlags.Instance,
                null,
                CallingConventions.Any,
                new Type[] { typeof(Vector2) },
                null);
            Action<Vector2> action = method == null ? null : rect => method.Invoke(instance, new object[] { rect });
            var nodeItem = new NodeItem(instance, Vector2.zero, action);

            _onCreate?.Invoke(nodeItem);
            _nodes.Add(new Node(mousePosition, nodeItem, _nodeStyle, _selectedNodeStyle, OnClickRemoveNode));
        }

        public void SetInstancedList(NodeItem[] items)
        {
            if (_nodes == null)
            {
                _nodes = new List<Node>();
            }

            for (var i = 0; i < items.Length; i++)
            {
                var item = items[i];
                _nodes.Add(new Node(item.Position, item, _nodeStyle, _selectedNodeStyle, OnClickRemoveNode));
            }
        }

        public void SetActions(Action<NodeItem> onCreate, Action<NodeItem> onRemove)
        {
            _onRemove = onRemove;
            _onCreate = onCreate;
        }

        public void SetMenuList(Type[] items)
        {
            _menuList = items;
        }

        public void AddGroup(NodeGroup group)
        {
            if (_groups == null)
            {
                _groups = new List<NodeGroup>();
            }

            _groups.Add(group);
        }

        private void OnClickRemoveNode(Node node)
        {
            _onRemove?.Invoke(node.Object);
            _nodes.Remove(node);
        }

        public void SetOffset(Vector2 offset)
        {
            if (_nodes != null)
            {
                for (var i = 0; i < _nodes.Count; i++)
                {
                    _nodes[i].Drag(offset);
                }
            }

            if (_groups != null)
            {
                for (var i = 0; i < _groups.Count; i++)
                {
                    _groups[i].Drag(offset);
                }
            }
        }
    }
}