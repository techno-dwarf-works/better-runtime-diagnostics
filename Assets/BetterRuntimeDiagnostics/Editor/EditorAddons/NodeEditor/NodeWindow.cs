using System;
using System.Collections.Generic;
using Better.Diagnostics.EditorAddons.NodeEditor.Models;
using Better.Diagnostics.Runtime.NodeModule;
using Better.Extensions.Runtime;
using UnityEditor;
using UnityEngine;

namespace Better.Diagnostics.EditorAddons.NodeEditor
{
    public class NodeWindow : EditorWindow
    {
        private Vector2 _lastWindowSize;

        private Vector2 _drag;
        private Vector2 _offset;
        private Type[] _menuList;
        private Action<object> _onCreate;
        private Action<object> _onRemove;
        private List<NodeGroup> _groups;
        public event Action OnClosed;
        public event Action OnSave;
        public event Action OnDiscard;
        public event Action OnChanged;

        private NodeGroup _generalGroup;
        private List<BaseNode> _sideNodes;

        public static NodeWindow OpenWindow()
        {
            var window = GetWindow<NodeWindow>();
            window.titleContent = new GUIContent("Node Based Editor");
            window._menuList = null;
            window._onCreate = null;
            window._onRemove = null;
            window.saveChangesMessage = "This window has unsaved changes. Would you like to save?";
            window.OnSave = null;
            window.OnDiscard = null;
            window.OnChanged = null;
            window._sideNodes = null;
            window.wantsMouseMove = true;
            window._generalGroup = new NodeGroup("General Group", new Rect(Vector2.zero, window.position.size), GUIStyle.none);
            window._generalGroup.OnRemove += window.OnRemove;
            window._generalGroup.DataChanged += window.OnDataChanged;
            window._groups = null;
            return window;
        }

        public override void DiscardChanges()
        {
            base.DiscardChanges();
            OnDiscard?.Invoke();
        }

        private void OnDataChanged()
        {
            hasUnsavedChanges = true;
            OnChanged?.Invoke();
        }

        public override void SaveChanges()
        {
            base.SaveChanges();
            OnSave?.Invoke();
        }

        private void OnGUI()
        {
            if (_lastWindowSize != position.size)
            {
                _generalGroup?.SetRect(new Rect(Vector2.zero, position.size));
                _lastWindowSize = position.size;
            }

            DrawGrid(20, 0.2f, Color.gray);
            DrawGrid(100, 0.4f, Color.gray);

            Draw();

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

        private void Draw()
        {
            if (_groups != null)
            {
                for (var i = 0; i < _groups.Count; i++)
                {
                    _groups[i].Draw();
                }
            }

            _generalGroup?.Draw();

            if (_sideNodes != null)
            {
                for (var i = 0; i < _sideNodes.Count; i++)
                {
                    var node = _sideNodes[i];
                    var drawRect = node.AbsoluteRect();
                    if (i - 1 > 0)
                    {
                        var prevHeight = _sideNodes[i - 1].GetHeight();
                        drawRect.height = prevHeight;
                    }

                    node.SetRect(drawRect);
                    node.Draw();
                }
            }
        }

        private void ProcessEvents(Event e)
        {
            _generalGroup?.ProcessEvents(e);

            if (ProcessSideNotes(e)) return;

            if (ProcessGroups(e)) return;

            _drag = Vector2.zero;
            switch (e.type)
            {
                case EventType.ContextClick:
                    ProcessContextMenu(e);
                    break;
                case EventType.KeyDown:
                    ProcessKeyDown(e);
                    break;
                case EventType.MouseDrag:
                    ProcessDrag(e);
                    break;
                case EventType.ValidateCommand:
                    if (e.commandName.FastEquals("Duplicate"))
                    {
                    }

                    break;
            }
        }

        private void ProcessKeyDown(Event e)
        {
            if ((e.control || e.keyCode == KeyCode.LeftCommand) && e.keyCode == KeyCode.S)
            {
                SaveChanges();
            }
        }

        private bool ProcessGroups(Event e)
        {
            if (_groups != null)
            {
                for (var i = 0; i < _groups.Count; i++)
                {
                    var isChanged = _groups[i].ProcessEvents(e);
                    if (isChanged)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool ProcessSideNotes(Event e)
        {
            if (_sideNodes != null)
            {
                for (var i = 0; i < _sideNodes.Count; i++)
                {
                    var isChanged = _sideNodes[i].ProcessEvents(e);
                    if (isChanged)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private void ProcessDrag(Event e)
        {
            if (e.button != 0) return;
            if (_groups != null)
            {
                for (var i = 0; i < _groups.Count; i++)
                {
                    _groups[i].Drag(e.delta);
                }
            }

            _generalGroup?.Drag(e.delta);

            _drag = e.delta;
            e.Use();
            GUI.changed = true;
        }

        private void ProcessContextMenu(Event e)
        {
            var genericMenu = new GenericMenu();
            if (_menuList == null) return;
            for (var i = 0; i < _menuList.Length; i++)
            {
                var menu = _menuList[i];
                genericMenu.AddItem(new GUIContent($"Add {menu.Name.PrettyCamelCase()}"), false, () => OnClickAddNode(menu, e.mousePosition));
            }

            e.Use();
            genericMenu.ShowAsContext();
        }

        public void SetInstancedList(IEnumerable<NodeItem> items)
        {
            _generalGroup.SetNodeItems(items);
        }

        public void SetSideList(IEnumerable<NodeItem> items)
        {
            if (_sideNodes == null)
            {
                _sideNodes = new List<BaseNode>();
            }

            foreach (var nodeItem in items)
            {
                var size = NodeStyles.DefaultSideSize * _sideNodes.Count;
                var rect = new Rect(new Vector2(0, size.y), NodeStyles.DefaultSideSize);
                nodeItem.SetRect(rect);
                var node = new BaseNode(nodeItem, NodeStyles.SideBoxStyle, NodeStyles.SelectedSideBoxStyle);
                SetUpSideNode(node);
                _sideNodes.Add(node);
            }
        }

        private void SetUpSideNode(BaseNode node)
        {
            node.OnRemoveNode += OnRemoveSideNode;
            node.OnChanged += OnDataChanged;
        }

        public void SetActions(Action<object> onCreate, Action<object> onRemove)
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

            group.DataChanged += OnDataChanged;
            group.OnRemove += OnRemove;
            _groups.Add(group);
        }

        private void OnRemove(NodeItem obj)
        {
            _onRemove?.Invoke(obj.InnerObject);
        }

        private void OnClickAddNode(Type nodeType, Vector2 mousePosition)
        {
            var instance = Activator.CreateInstance(nodeType);

            if (instance is INodeRect nodeRect)
            {
                var rect = new Rect(mousePosition - _offset, NodeStyles.DefaultSize);
                nodeRect.SetRect(rect);
                var item = new Node(new NodeItem(nodeRect), NodeStyles.NodeStyle, NodeStyles.SelectedNodeStyle);
                item.Drag(_offset);
                var group = FindNodeGroup(mousePosition);
                group.Attach(item);
            }
            else
            {
                if (_sideNodes == null)
                {
                    _sideNodes = new List<BaseNode>();
                }

                var size = NodeStyles.DefaultSideSize * _sideNodes.Count;
                var rect = new Rect(new Vector2(0, size.y), NodeStyles.DefaultSideSize);
                var node = new BaseNode(new NodeItem(instance, rect, null), NodeStyles.SideBoxStyle, NodeStyles.SelectedSideBoxStyle);
                SetUpSideNode(node);
                _sideNodes.Add(node);
            }

            _onCreate?.Invoke(instance);

            OnDataChanged();
        }

        private NodeGroup FindNodeGroup(Vector2 mousePosition)
        {
            if (_groups == null)
            {
                return _generalGroup;
            }

            for (var i = 0; i < _groups.Count; i++)
            {
                var group = _groups[i];
                if (group.Contains(mousePosition))
                {
                    return group;
                }
            }

            return _generalGroup;
        }

        private void OnRemoveSideNode(BaseNode node)
        {
            node.OnRemoveNode -= OnRemoveSideNode;
            node.OnChanged -= OnDataChanged;
            _sideNodes.Remove(node);

            OnRemove(node.Object);
            OnDataChanged();
        }

        //TODO: Make offset works
        public void SetOffset(Vector2 offset)
        {
            if (_groups != null)
            {
                for (var i = 0; i < _groups.Count; i++)
                {
                    _groups[i].Drag(offset);
                }
            }
        }

        private void OnDestroy()
        {
            OnClosed?.Invoke();
        }
    }
}