using System;
using System.Collections.Generic;
using BSDesigner.BehaviourTrees;
using BSDesigner.Core;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Profiling.Memory.Experimental;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;
using Node = BSDesigner.Core.Node;

namespace BSDesigner.Unity.VisualTool.Editor
{
    public class BSGraphView : GraphView
    {
        private static readonly float k_MinZoomScale = 0.5f;
        private static readonly float k_MaxZoomScale = 5f;

        private BehaviourGraph m_CurrentGraph;

        private static readonly string k_StylePath = "GridBackground.uss";

        #region Fields

        private readonly EditorWindow m_EditorWindow;

        private readonly CustomEdgeConnectorListener<EdgeView> m_ConnectorListener;
        #endregion

        #region public Properties

        public IEdgeConnectorListener Connector => m_ConnectorListener;

        #endregion

        #region Events

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
            this.styleSheets.Add(styleSheet);

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(new ClickSelector());

            SetupZoom(ContentZoomer.DefaultMinScale * k_MinZoomScale, ContentZoomer.DefaultMaxScale * k_MaxZoomScale);

            this.nodeCreationRequest = HandleNodeCreationCall;

            m_ConnectorListener = new CustomEdgeConnectorListener<EdgeView>(OnEdgeCreated, OnEdgeCreatedOutsidePort);
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
            else
            {
                
            }
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
            DrawNode(node);
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
            CreateConnection(edge);
        }

        private void CreateConnection(EdgeView edge)
        {
            var source = edge.output.node as NodeView;
            var target = edge.input.node as NodeView;

            m_CurrentGraph.ConnectNodes(source?.Node, target?.Node);
            DataChanged?.Invoke();
        }

        private void RemoveConnection(EdgeView edge)
        {
            var source = edge.output.node as NodeView;
            var target = edge.input.node as NodeView;

            m_CurrentGraph.Disconnect(source?.Node, target?.Node);
            DataChanged?.Invoke();
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