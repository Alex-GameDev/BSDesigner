using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace BSDesigner.Unity.VisualTool.Editor.Graphs
{
    public enum GraphUIEvent
    {
        Moved,
        Added,
        Removed,
        Selected,
        Unselected,
        Repainted,
        DataChanged,
        Destroyed
    }

    public abstract class NodeRenderer
    {
        public static NodeRenderer Create(NodeView nodeView)
        {
            return null;
        }

        public abstract void SetUp();

        public abstract void OnUIEvent(GraphUIEvent evt);

        public abstract void OnConnect(Edge edge);

        public abstract void OnDisconnect(Edge edge);

        public abstract void BuildContextualMenu(ContextualMenuPopulateEvent evt);
    }
}
