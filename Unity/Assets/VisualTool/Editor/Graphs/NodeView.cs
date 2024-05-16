using BSDesigner.Core;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using Node = BSDesigner.Core.Node;

namespace BSDesigner.Unity.VisualTool.Editor.Graphs
{
    public class NodeView : UnityEditor.Experimental.GraphView.Node
    {
        private static readonly string BORDER_ID = "node-border";
        private static readonly string NAME_FIELD_ID = "node-name-lbl";


        #region Private fields

        private readonly Node node;

        private readonly IEdgeConnectorListener connector;

        private readonly NodeRenderer renderer;

        private readonly List<PortView> inputPorts = new List<PortView>();

        private readonly List<PortView> outputPorts = new List<PortView>();

        private VisualElement border;

        private Label nameLabel;

        #endregion


        public NodeView(Node node, IEdgeConnectorListener connector) : base($"{ToolSettings.instance.EditorToolPath}/Graphs/UI/node.uxml")
        {
            this.node = node;
            this.connector = connector;
            this.renderer = NodeRenderer.Create(this);

            this.border = this.Q(BORDER_ID);
            this.nameLabel = this.Q<Label>(NAME_FIELD_ID);

            this.nameLabel.text = node.Name;

            SetPosition(new Rect(new Vector2(node.Position.X, node.Position.Y), Vector2.zero));

            this.renderer.OnUIEvent(GraphUIEvent.Added);
        }

        #region Public methods

        public void UpdatePosition()
        {
            var pos = GetPosition().position;
            node.Position = new System.Numerics.Vector2(pos.x, pos.y);
        }

        internal PortView InstantiatePort(Direction direction, Vector2 connectionDirection)
        {
            if (direction == Direction.Input ? node.MaxInputConnections == 0 : node.MaxOutputConnections == 0) return null;

            var capacity = direction == Direction.Input ?
                node.MaxInputConnections == -1 ? Port.Capacity.Multi : Port.Capacity.Single :
                node.MaxOutputConnections == -1 ? Port.Capacity.Multi : Port.Capacity.Single;

            var type = direction == Direction.Input ? node.GetType() : node.ChildType;
            var port = new PortView(Orientation.Horizontal, direction, capacity, type, connector);

            if (direction == Direction.Input)
            {
                inputContainer.Add(port);
                inputPorts.Add(port);
            }
            else
            {
                outputContainer.Add(port);
                outputPorts.Add(port);
            }

            return port;
        }

        #endregion

        #region Override methods

        public override void OnSelected()
        {
            base.OnSelected();
            //m_BorderElement.AddToClassList("border-selected");
            //m_BorderElement.RemoveFromClassList("border-unselected");
            renderer.OnUIEvent(GraphUIEvent.Selected);
        }

        public override void OnUnselected()
        {
            base.OnUnselected();
            //m_BorderElement.RemoveFromClassList("border-selected");
            //m_BorderElement.AddToClassList("border-unselected");
            renderer.OnUIEvent(GraphUIEvent.Unselected);
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            renderer.BuildContextualMenu(evt);
            evt.StopPropagation();
        }

        #endregion
    }
}