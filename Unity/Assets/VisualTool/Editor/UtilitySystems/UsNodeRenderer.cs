using BSDesigner.Unity.VisualTool.Editor.Graphs;
using BSDesigner.UtilitySystems;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace BSDesigner.Unity.VisualTool.Editor
{
    [NodeRendererOfType(typeof(UtilityNode))]
    public class UsNodeRenderer : NodeRenderer
    {
        private PortView inputPort, outputPort;

        public UsNodeRenderer(NodeView view) : base(view)
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
            throw new System.NotImplementedException();
        }

        public override void OnDisconnect(EdgeView edge)
        {
            throw new System.NotImplementedException();
        }

        public override void OnUIEvent(GraphUIEvent evt)
        {
            throw new System.NotImplementedException();
        }

        public override void SetUp()
        {
            var node = view.Node;
            if (node == null || node.MaxInputConnections != 0)
            {
                this.inputPort = view.InstantiatePort(Direction.Input, Vector2.left);
            }
            else
            {
                view.inputContainer.style.display = DisplayStyle.None;
            }

            if (node == null || node.MaxOutputConnections != 0)
            {
                this.outputPort = view.InstantiatePort(Direction.Output, Vector2.right);
            }
            else
            {
                view.outputContainer.style.display = DisplayStyle.None;
            }
        }
    }
}
