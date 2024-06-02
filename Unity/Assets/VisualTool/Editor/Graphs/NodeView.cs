using BSDesigner.Core;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using Node = BSDesigner.Core.Node;

namespace BSDesigner.Unity.VisualTool.Editor.Graphs
{
    public class NodeView : UnityEditor.Experimental.GraphView.Node
    {
        private static readonly string BORDER_ID = "node-border";
        private static readonly string NAME_FIELD_ID = "node-id-lbl";

        private static readonly Color SELECTED_COLOR = new Color(0.3f, 0.8f, 1f, 1f);

        #region Properties

        public Node Node { get; }

        public event Action DataChanged;

        public event EventHandler<ContextualMenuPopulateEvent> OnBuildContextualMenu;

        #endregion

        #region Private fields

        private readonly IEdgeConnectorListener connector;

        private readonly NodeRenderer renderer;

        private readonly List<PortView> inputPorts = new List<PortView>();

        private readonly List<PortView> outputPorts = new List<PortView>();

        private VisualElement border;

        private Label nameLabel;

        #endregion


        public NodeView(Node node, IEdgeConnectorListener connector) : base($"{ToolSettings.instance.EditorToolPath}/Editor/Graphs/UI/node.uxml")
        {
            this.Node = node;
            this.connector = connector;
            this.renderer = NodeRenderer.Create(this);

            this.border = this.Q(BORDER_ID);
            this.nameLabel = this.Q<Label>(NAME_FIELD_ID);

            this.nameLabel.text = string.IsNullOrEmpty(node.Name) ? node.GetType().Name : node.Name;

            SetPosition(new Rect(new Vector2(node.Position.X, node.Position.Y), Vector2.zero));

            this.renderer?.SetUp();
            this.renderer?.OnUIEvent(GraphUIEvent.Added);
        }

        #region Public methods

        public void UpdatePosition()
        {
            var pos = GetPosition().position;
            this.Node.Position = new System.Numerics.Vector2(pos.x, pos.y);
        }

        internal PortView InstantiatePort(Direction direction, Vector2 connectionDirection)
        {
            if (direction == Direction.Input ? Node.MaxInputConnections == 0 : Node.MaxOutputConnections == 0) return null;

            var capacity = direction == Direction.Input ?
                Node.MaxInputConnections == -1 ? Port.Capacity.Multi : Port.Capacity.Single :
                Node.MaxOutputConnections == -1 ? Port.Capacity.Multi : Port.Capacity.Single;

            var type = direction == Direction.Input ? Node.GetType() : Node.ChildType;
            var port = new PortView(Orientation.Horizontal, direction, capacity, type, connector);
            port.ConnectionDirection = connectionDirection;

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
            this.border.style.backgroundColor = SELECTED_COLOR;
            renderer?.OnUIEvent(GraphUIEvent.Selected);
        }

        public override void OnUnselected()
        {
            base.OnUnselected();
            this.border.style.backgroundColor = new Color(0, 0, 0, 0);
            renderer?.OnUIEvent(GraphUIEvent.Unselected);
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            renderer?.BuildContextualMenu(evt);
            this.OnBuildContextualMenu(this, evt);
            evt.StopPropagation();
        }

        public PortView GetPort(NodeView target, Direction input) => this.renderer.GetPort(target, input);

        public void RefreshView()
        {
            var pos = this.Node.Position;
            var unityPos = new Vector2(pos.X, pos.Y);
            SetPosition(new Rect(unityPos, Vector2.zero));
        }

        #endregion
    }
}