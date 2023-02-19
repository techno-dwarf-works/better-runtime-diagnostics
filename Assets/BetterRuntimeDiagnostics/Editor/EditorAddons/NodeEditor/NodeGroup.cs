using UnityEngine;

namespace Better.Diagnostics.EditorAddons.NodeEditor
{
    public class NodeGroup
    {
        private Rect _rect;
        
        public NodeGroup(Rect rect)
        {
            _rect = rect;
        }

        public void SetPosition(Rect rect)
        {
            _rect = rect;
        }
        
        public void Drag(Vector2 delta)
        {
            _rect.position += delta;
        }

        public void Draw()
        {
            GUI.Box(_rect, "");
        }
    }
}