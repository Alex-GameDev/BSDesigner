using BSDesigner.Core;
using BSDesigner.Unity.VisualTool.Editor.Assets.VisualTool.Editor.Graphs;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using Node = BSDesigner.Core.Node;

namespace BSDesigner.Unity.VisualTool.Editor.Graphs
{
    public class GraphView : UnityEditor.Experimental.GraphView.GraphView
    {
        #region Properties

        /// <summary>
        /// The current rendered graph
        /// </summary>
        public BehaviourGraph Graph { get; private set; }

        #endregion

        #region Events

        /// <summary>
        /// Event called when any node variable changed.
        /// </summary>
        public event Action DataChanged;

        /// <summary>
        /// Event called when the node selection changes.
        /// </summary>
        public event Action<IEnumerable<Node>> NodeSelectionChanged;

        #endregion

        private IEdgeConnectorListener connector;

        private Dictionary<Node, NodeView> nodeViewMap = new Dictionary<Node, NodeView>();

        /// <summary>
        /// Create a new graphView
        /// </summary>
        public GraphView()
        {
            GridBackground background = new GridBackground();
            background.StretchToParentSize();
            Insert(0, background);

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(new ClickSelector());

            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            this.nodeCreationRequest = HandleNodeCreationRequest;
            this.graphViewChanged = HandleMoveOrDeleteGraphElement;
            this.connector = new CustomEdgeConnectorListener<EdgeView>(HandleCreateConnection, HandleCreateConnectionWithoutTargetNode);
        }

        public void Update(BehaviourEngine behaviourEngine)
        {
            Graph = behaviourEngine as BehaviourGraph;

            ClearView();
            if(Graph != null)
            {
                UpdateView();
            }
        }

        #region Override methods

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            var validPorts = new List<Port>();
            var startNodeView = startPort.node as NodeView;

            if (startNodeView == null) return validPorts;

            var bannedNodes = new HashSet<Node>();

            if (!Graph.CanCreateLoops)
            {
                bannedNodes = startPort.direction == Direction.Input ?
                    startNodeView.Node.Children.ToHashSet() :
                    startNodeView.Node.Parents.ToHashSet();
            }

            foreach (var port in ports)
            {
                if (startPort.direction == port.direction) continue;
                if (startPort.node == port.node) continue;

                var otherNodeView = port.node as NodeView;

                if (bannedNodes.Contains(otherNodeView?.Node)) continue;
                if (startPort.direction == Direction.Input && !port.portType.IsAssignableFrom(startPort.portType)) continue;
                if (startPort.direction == Direction.Output && !startPort.portType.IsAssignableFrom(port.portType)) continue;

                validPorts.Add(port);
            }
            return validPorts;
        }

        /// <summary>
        /// Called when a new element is added to the selection.
        /// Invoke the selection change method.
        /// </summary>
        /// <param name="selectable">The selected element.</param>
        public override void AddToSelection(ISelectable selectable)
        {
            base.AddToSelection(selectable);
            NodeSelectionChanged?.Invoke(GetSelection());
        }

        /// <summary>
        /// Called when a new element is removed from the selection
        /// Invoke the selection change method.
        /// </summary>
        /// <param name="selectable">The unselected element.</param>
        public override void RemoveFromSelection(ISelectable selectable)
        {
            base.RemoveFromSelection(selectable);
            NodeSelectionChanged?.Invoke(GetSelection());
        }

        /// <summary>
        /// Called when the selection is clear.
        /// Invoke the selection change method.
        /// </summary>
        public override void ClearSelection()
        {
            base.ClearSelection();
            NodeSelectionChanged?.Invoke(GetSelection());
        }

        private IEnumerable<Node> GetSelection() => selection.OfType<NodeView>().Select(n => n.Node);

        #endregion

        #region Render data

        private void UpdateView()
        {
            foreach (var node in Graph.Nodes)
            {
                DrawNode(node);
            }

            foreach (var connection in Graph.GetConnections())
            {
                DrawConnection(connection);
            }
        }

        private void ClearView()
        {
            this.graphElements.ForEach(RemoveElement);
        }

        private void DrawNode(Node node)
        {
            var nodeView = new NodeView(node, connector);
            this.AddElement(nodeView);
            this.nodeViewMap[node] = nodeView;
        }

        private void DrawConnection(Connection connection)
        {
            var source = this.nodeViewMap.GetValueOrDefault(connection.Source);
            var target = this.nodeViewMap.GetValueOrDefault(connection.Target);
        }

        #endregion

        #region UI Events

        private void HandleNodeCreationRequest(NodeCreationContext context)
        {
            //var nodeCreationProvider = NodeTypeSearchWindow.Create(m_CurrentGraph.NodeType, (_, t) => CreateNode(t, ctx));
            //SearchWindow.Open(new SearchWindowContext(ctx.screenMousePosition), nodeCreationProvider);
        }

        /// <summary>
        /// Method called when a connection is created.
        /// </summary>
        /// <param name="newEdge">The edge created.</param>
        private void HandleCreateConnection(EdgeView newEdge)
        {
            var edgesToDelete =
                newEdge.input.connections.Where(previousEdge => AreEdgesCompatible(newEdge, previousEdge))
                .Union(newEdge.output.connections.Where(previousEdge => AreEdgesCompatible(newEdge, previousEdge)));

            if (edgesToDelete.Count() > 0)
            {
                DeleteElements(edgesToDelete);
            }

            newEdge.input.Connect(newEdge);
            newEdge.output.Connect(newEdge);
            this.AddElement(newEdge);
            CreateConnectionFromEdge(newEdge);
        }

        /// <summary>
        /// Method called when a connection is created with no target node.
        /// </summary>
        /// <param name="newEdge">The edge created</param>
        /// <param name="dropPosition">The drop position in the view</param>
        private void HandleCreateConnectionWithoutTargetNode(Edge newEdge, Vector2 dropPosition)
        {
            //TODO
        }

        /// <summary>
        /// Method called when an element in the graphview is moved or deleted.
        /// </summary>
        private GraphViewChange HandleMoveOrDeleteGraphElement(GraphViewChange change)
        {
            if (change.elementsToRemove != null) DeleteGraphElements(change.elementsToRemove);
            if (change.movedElements != null) MoveGraphElements(change.movedElements);
            //DataChanged?.Invoke();
            return change;
        }

        private void DeleteGraphElements(IEnumerable<GraphElement> elementsToRemove)
        {
            var removedEdges = elementsToRemove.OfType<EdgeView>();
            var removedNodes = elementsToRemove.OfType<NodeView>();

            foreach (var removedEdge in removedEdges)
            {
                var source = (NodeView)removedEdge.output.node;
                var target = (NodeView)removedEdge.input.node;
                Graph.Disconnect(source.Node, target.Node);
            }

            foreach (var removedNode in removedNodes)
            {
                var node = removedNode.Node;
                Graph.RemoveNode(node);
            }
        }

        private void MoveGraphElements(List<GraphElement> elementsToMove)
        {
            var movedNodes = elementsToMove.OfType<NodeView>();

            foreach (var movedNode in movedNodes)
            {
                movedNode.UpdatePosition();
            }
        }

        private bool AreEdgesCompatible(Edge newEdge, Edge previousEdge) => newEdge != previousEdge &&
            (newEdge.input.capacity == Port.Capacity.Single || previousEdge.output == newEdge.output);


        private void CreateConnectionFromEdge(EdgeView edge)
        {
            var sourceView = edge.output.node as NodeView;
            var targetView = edge.input.node as NodeView;
            var source = sourceView?.Node;
            var target = targetView?.Node;

            if (source == null || target == null) return;

            this.Graph.ConnectNodes(source, target);

            //sourceView.OnConnected(edge);
            //targetView.OnConnected(edge);

            //DataChanged?.Invoke();
        }

        #endregion
    }
}
