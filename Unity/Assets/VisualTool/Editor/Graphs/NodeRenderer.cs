using BSDesigner.Unity.VisualTool.Editor.BehaviourTrees;
using System;
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
        protected readonly NodeView view;

        protected NodeRenderer(NodeView view)
        {
            this.view = view;
        }

        public static NodeRenderer Create(NodeView nodeView)
        {
            return new BtNodeRenderer(nodeView);
        }

        public abstract void SetUp();

        public abstract void OnUIEvent(GraphUIEvent evt);

        public abstract void OnConnect(EdgeView edge);

        public abstract void OnDisconnect(EdgeView edge);

        public abstract void BuildContextualMenu(ContextualMenuPopulateEvent evt);

        public abstract PortView GetPort(NodeView target, Direction input);
    }
}
