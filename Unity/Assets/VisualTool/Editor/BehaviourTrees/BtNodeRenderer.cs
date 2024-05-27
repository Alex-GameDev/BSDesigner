using BSDesigner.BehaviourTrees;
using BSDesigner.Unity.VisualTool.Editor.Assets.VisualTool.Editor.GraphView;
using BSDesigner.Unity.VisualTool.Editor.Graphs;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace BSDesigner.Unity.VisualTool.Editor.BehaviourTrees
{
    [NodeRenderer(typeof(BehaviourTree))]
    public class BtNodeRenderer : NodeRenderer
    {
        private PortView inputPort, outputPort;

        public BtNodeRenderer(NodeView view) : base(view)
        {
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {

        }

        public override PortView GetPort(NodeView target, Direction input)
        {
            return input == Direction.Input ? inputPort : outputPort;
        }

        public override void OnConnect(EdgeView edge)
        {
            return;
        }

        public override void OnDisconnect(EdgeView edge)
        {
            return;
        }

        public override void OnUIEvent(GraphUIEvent evt)
        {
            switch (evt)
            {
                case GraphUIEvent.Moved:
                    break;
                case GraphUIEvent.Added:
                    break;
                case GraphUIEvent.Removed:
                    break;
                case GraphUIEvent.Selected:
                    break;
                case GraphUIEvent.Unselected:
                    break;
                case GraphUIEvent.Repainted:
                    break;
                case GraphUIEvent.DataChanged:
                    break;
                case GraphUIEvent.Destroyed:
                    break;
            }
        }

        public override void SetUp()
        {
            var node = view.Node;
            if (node == null || node.MaxInputConnections != 0)
            {
                this.inputPort = view.InstantiatePort(Direction.Input, Vector2.up);
            }
            else
            {
                view.inputContainer.style.display = DisplayStyle.None;
            }

            if (node == null || node.MaxOutputConnections != 0)
            {
                this.outputPort = view.InstantiatePort(Direction.Output, Vector2.down);
            }
            else
            {
                view.outputContainer.style.display = DisplayStyle.None;
            }
        }
    }
}
