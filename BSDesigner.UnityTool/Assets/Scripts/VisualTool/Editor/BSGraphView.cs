using System;
using System.Collections.Generic;
using System.Linq;
using BSDesigner.Core;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using Node = BSDesigner.Core.Node;
using UTILS = BSDesigner.Unity.VisualTool.Editor.EditorUtilities;

namespace BSDesigner.Unity.VisualTool.Editor
{
    /// <summary>
    /// Visual representation of a Behaviour graph.
    /// </summary>
    public class BSGraphView : GraphView
    {
        private static readonly float k_MinZoomScale = 0.5f;
        private static readonly float k_MaxZoomScale = 5f;
        private static readonly string k_StylePath = "GridBackground.uss";

        #region Fields

        private BehaviourGraph m_CurrentGraph;

        private readonly EditorWindow m_EditorWindow;

        private readonly CustomEdgeConnectorListener<EdgeView> m_ConnectorListener;
        
        #endregion

        #region public Properties

        public BehaviourGraph Graph => m_CurrentGraph;
        public IEdgeConnectorListener Connector => m_ConnectorListener;

        #endregion

        #region Events

        /// <summary>
        /// Event called when any node variable changed.
        /// </summary>
        public event Action DataChanged;

        #endregion

        #region Constructor

        public BSGraphView(EditorWindow window)
        {
            m_EditorWindow = window;

            var bg = new GridBackground { name = "Grid" };
            bg.StretchToParentSize();
            Insert(0, bg);

            var stylePath = VisualToolSettings.instance.EditorStylesPath + k_StylePath;
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(stylePath);
            styleSheets.Add(styleSheet);

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(new ClickSelector());

            SetupZoom(ContentZoomer.DefaultMinScale * k_MinZoomScale, ContentZoomer.DefaultMaxScale * k_MaxZoomScale);

            nodeCreationRequest = HandleNodeCreationCall;
            graphViewChanged = OnGraphViewChanged;
            m_ConnectorListener = new CustomEdgeConnectorListener<EdgeView>(OnEdgeCreated, OnEdgeCreatedOutsidePort);
        }

        #endregion

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
                    startNodeView.Node.GetConnectedNodesAsChild() :
                    startNodeView.Node.GetConnectedNodesAsParent();
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

        #endregion

        #region Public method

        public void LoadGraph(BehaviourGraph graph)
        {
            graphElements.ForEach(RemoveElement);

            m_CurrentGraph = graph;

            if (m_CurrentGraph != null)
            {
                DrawGraph();
            }
            UTILS.LOG($"GV - Load graph ({graph}).");
        }

        #endregion

        #region Private methods

        private void HandleNodeCreationCall(NodeCreationContext ctx)
        {
            var nodeCreationProvider = NodeTypeSearchWindow.Create(m_CurrentGraph.NodeType, (_, t) => CreateNode(t, ctx));
            SearchWindow.Open(new SearchWindowContext(ctx.screenMousePosition), nodeCreationProvider);
        }

        private void CreateNode(Type nodeType, NodeCreationContext ctx)
        {
            var pos = contentViewContainer.WorldToLocal(ctx.screenMousePosition - m_EditorWindow.position.position);
            var node = (Node)Activator.CreateInstance(nodeType);
            node.Position = new System.Numerics.Vector2(pos.x, pos.y);
            m_CurrentGraph.AddNode(node);

            DrawNode(node);

            UTILS.LOG($"GV - Create new node ({node}) at position ({pos}).");
            DataChanged?.Invoke();
        }

        private NodeView DrawNode(Node node)
        {
            var nodeView = new NodeView(node, this);
            this.AddElement(nodeView);
            return nodeView;
        }

        private void DrawGraph()
        {
            var nodeViewMap = new Dictionary<Node, NodeView>();

            foreach (var node in m_CurrentGraph.Nodes)
            {
                var view = DrawNode(node);
                nodeViewMap[node] = view;
            }

            foreach (var node in m_CurrentGraph.Nodes)
            {
                var source = nodeViewMap[node];
                foreach (var child in node.Children)
                {
                    var target = nodeViewMap[child];
                    var sourcePort = source.GetValidPort(target, Direction.Output);
                    var targetPort = target.GetValidPort(source, Direction.Input);
                    var edge = sourcePort.ConnectTo<EdgeView>(targetPort);
                    AddElement(edge);
                }
            }

            foreach (var view in nodeViewMap.Values)
            {
                view.OnCreated();
            }
        }

        private void OnEdgeCreatedOutsidePort(EdgeView edge, Vector2 pos)
        {
            if (edge.output != null)
            {
                //var parentView = edge.output.node as NodeView;
                //var nodeCreationProvider = ElementSearchWindowProvider.Create(m_Metadata, parentView.NodeData.ChildType, (t, pos) => CreateAndConnect(t, pos, edge, parentView));
                //SearchWindow.Open(new SearchWindowContext(pos), nodeCreationProvider);
            }
        }

        private void OnEdgeCreated(EdgeView edge)
        {
            var edgesToDelete = new HashSet<Edge>();

            foreach (var connection in edge.input.connections)
            {
                if (edge.input.capacity == Port.Capacity.Single || connection.output == edge.output)
                {
                    if (connection != edge) edgesToDelete.Add(connection);
                }
            }

            foreach (var connection in edge.output.connections)
            {
                if (edge.output.capacity == Port.Capacity.Single || connection.input == edge.input)
                {
                    if (connection != edge) edgesToDelete.Add(connection);
                }
            }

            if (edgesToDelete.Count > 0) DeleteElements(edgesToDelete);

            edge.input.Connect(edge);
            edge.output.Connect(edge);
            this.AddElement(edge);
            CreateConnection(edge);
        }

        private void CreateConnection(Edge edge)
        {
            var source = (edge.output.node as NodeView)?.Node;
            var target = (edge.input.node as NodeView)?.Node;

            if(source == null || target == null) return;

            UTILS.LOG($"GV - Connect {source} with {target}");
            m_CurrentGraph.ConnectNodes(source, target);
            DataChanged?.Invoke();
        }

        private GraphViewChange OnGraphViewChanged(GraphViewChange change)
        {
            if (change.elementsToRemove != null) OnDeleteGraphElements(change.elementsToRemove);
            if (change.movedElements != null) OnMoveGraphElements(change.movedElements);
            DataChanged?.Invoke();
            return change;
        }

        private void OnDeleteGraphElements(List<GraphElement> elementsToRemove)
        {
            var removedEdges = elementsToRemove.OfType<EdgeView>();
            var removedNodes = elementsToRemove.OfType<NodeView>();
            
            foreach (var removedEdge in removedEdges)
            {
                var source = (NodeView)removedEdge.output.node;
                var target = (NodeView)removedEdge.input.node;
                m_CurrentGraph.Disconnect(source.Node, target.Node);
            }

            foreach (var removedNode in removedNodes)
            {
                var node = removedNode.Node;
                m_CurrentGraph.RemoveNode(node);
            }
            UTILS.LOG("GV - Remove elements");
        }

        private void OnMoveGraphElements(List<GraphElement> elementsToMove)
        {
            var movedNodes = elementsToMove.OfType<NodeView>();

            foreach (var movedNode in movedNodes)
            {
                movedNode.OnMoved();
            }
            UTILS.LOG("GV - Move elements");
        }

        #endregion

        /// <summary>
        /// Generic Edge connector listener class.
        /// </summary>
        /// <typeparam name="TEdge">The type of the edges that t</typeparam>
        private class CustomEdgeConnectorListener<TEdge> : IEdgeConnectorListener where TEdge : Edge
        {
            private readonly System.Action<TEdge> m_callback = null;
            private readonly System.Action<TEdge, Vector2> m_callbackOutsidePort = null;

            public CustomEdgeConnectorListener(System.Action<TEdge> callback, System.Action<TEdge, Vector2> callbackOutsidePort = null)
            {
                m_callback = callback;
                m_callbackOutsidePort = callbackOutsidePort;
            }

            public void OnDrop(GraphView graphView, Edge edge) => m_callback?.Invoke(edge as TEdge);

            public void OnDropOutsidePort(Edge edge, Vector2 position) => m_callbackOutsidePort?.Invoke(edge as TEdge, position);
        }
    }


}