using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.Tilemaps.Tilemap;
using GVNode = UnityEditor.Experimental.GraphView.Node;
using Node = BSDesigner.Core.Node;
using Orientation = UnityEditor.Experimental.GraphView.Orientation;

namespace BSDesigner.Unity.VisualTool.Editor
{
    public class NodeView : GVNode
    {
        private static readonly string k_NodeLayoutPath = "node.uxml";

        #region Private fields

        private readonly Node m_Node;

        private readonly BSGraphView m_GraphView;

        private readonly NodeDrawer m_Drawer;

        private readonly List<PortView> m_InputPorts = new List<PortView>();
        
        private readonly List<PortView> m_OutputPorts = new List<PortView>();

        #endregion

        #region Public properties

        public Node Node => m_Node;

        public IReadOnlyList<PortView> InputPorts => m_InputPorts;
        public IReadOnlyList<PortView> OutputPorts => m_OutputPorts;

        #endregion

        #region Constructor

        public NodeView(Node node, BSGraphView gView) : base(VisualToolSettings.instance.EditorLayoutsPath + k_NodeLayoutPath)
        {
            m_Node = node;
            m_GraphView = gView;
            m_Drawer = NodeDrawer.CreateDrawer(this);

            SetPosition(new Rect(new Vector2(node.Position.X, node.Position.Y), Vector2.zero));

        }

        #endregion

        #region Override methods

        public override void OnSelected()
        {
            base.OnSelected();
            m_Drawer.OnSelected();
        }

        public override void OnUnselected()
        {
            base.OnUnselected();
            m_Drawer.OnUnselected();
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            m_Drawer.BuildContextualMenu(evt);
            evt.StopPropagation();
        }

        #endregion

        #region Public methods

        public void OnCreated()
        {
            m_Drawer.OnCreated();
        }

        public void OnMoved()
        {
            var pos = GetPosition().position;
            m_Node.Position = new System.Numerics.Vector2(pos.x, pos.y);
        }

        public void OnConnected(EdgeView edgeView)
        {
            //if (edgeView.output.node == this)
            //{
            //    var other = edgeView.input.node as NodeView;
            //    m_Node.children.Add(other.m_Node);
            //}
            //else
            //{
            //    var other = edgeView.output.node as NodeView;
            //    m_Node.parents.Add(other.m_Node);
            //}
            m_Drawer.OnConnected(edgeView);

            UpdateEdgeViews();
        }

        public void OnDisconnected(EdgeView edgeView)
        {
            //if (edgeView.output.node == this)
            //{
            //    var other = edgeView.input.node as NodeView;
            //    m_nodeData.children.Remove(other.NodeData);
            //}
            //else
            //{
            //    var other = edgeView.output.node as NodeView;
            //    m_nodeData.parents.Remove(other.NodeData);
            //}
            m_Drawer.OnDisconnected(edgeView);

            UpdateEdgeViews();
        }

        public PortView InstantiatePort(Direction direction, Vector2 connectionDirection)
        {
            if (direction == Direction.Input ? m_Node.MaxInputConnections == 0 : m_Node.MaxOutputConnections == 0) return null;

            var capacity = direction == Direction.Input ?
                m_Node.MaxInputConnections == -1 ? Port.Capacity.Multi : Port.Capacity.Single :
                m_Node.MaxOutputConnections == -1 ? Port.Capacity.Multi : Port.Capacity.Single;

            var type = direction == Direction.Input ? m_Node.GetType() : m_Node.ChildType;
            var port = new PortView(Orientation.Horizontal, direction, capacity, type, m_GraphView.Connector);

            if (direction == Direction.Input)
            {
                inputContainer.Add(port);
                m_InputPorts.Add(port);
            }
            else
            {
                outputContainer.Add(port);
                m_OutputPorts.Add(port);
            }

            return port;
        }

        public Port GetValidPort(NodeView other, Direction direction) => m_Drawer.GetValidPort(other, direction);

        #endregion

        private IEnumerable<EdgeView> GetInputEdges() => m_InputPorts.SelectMany(p => p.connections).Cast<EdgeView>();

        private IEnumerable<EdgeView> GetOutputEdges() => m_OutputPorts.SelectMany(p => p.connections).Cast<EdgeView>();

        #region Private methods

        private void UpdateEdgeViews()
        {

        }

        #endregion

    }
}