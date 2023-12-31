using System;
using BSDesigner.BehaviourTrees;
using BSDesigner.Core;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
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

        #region Fields

        private EditorWindow m_EditorWindow;

        #endregion

        public BSGraphView(EditorWindow window)
        {
            m_EditorWindow = window;

            var bg = new GridBackground { name = "Grid" };
            bg.StretchToParentSize();
            Insert(0, bg);

            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/VisualTool/Editor/uss/GridBackground.uss");
            this.styleSheets.Add(styleSheet);

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(new ClickSelector());

            SetupZoom(ContentZoomer.DefaultMinScale * k_MinZoomScale, ContentZoomer.DefaultMaxScale * k_MaxZoomScale);

            this.nodeCreationRequest = HandleNodeCreationCall;
        }


        #region Private methods

        private void HandleNodeCreationCall(NodeCreationContext ctx)
        {
            var nodeCreationProvider = NodeTypeSearchWindow.Create(typeof(BtNode), (_, t) => CreateNode(t, ctx));
            SearchWindow.Open(new SearchWindowContext(ctx.screenMousePosition), nodeCreationProvider);
        }

        private void CreateNode(Type nodeType, NodeCreationContext ctx)
        {
            var pos = contentViewContainer.WorldToLocal(ctx.screenMousePosition - m_EditorWindow.position.position);
            var node = (Node)Activator.CreateInstance(nodeType);
            node.Position = new System.Numerics.Vector2(pos.x, pos.y);
            DrawNode(node);
            Debug.Log(nodeType);
        }

        private NodeView DrawNode(Node node)
        {
            var nodeView = new NodeView(node, this);
            this.AddElement(nodeView);
            return nodeView;
        }

        #endregion
    }


}