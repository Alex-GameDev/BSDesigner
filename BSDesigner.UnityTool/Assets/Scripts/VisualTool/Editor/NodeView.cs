using BSDesigner.Core;
using UnityEngine;
using UnityEngine.UIElements;
using GVNode = UnityEditor.Experimental.GraphView.Node;

namespace BSDesigner.Unity.VisualTool.Editor
{
    public class NodeView : GVNode
    {
        #region Fields

        private Node m_Node;

        private BSGraphView m_GraphView;

        #endregion

        #region Constructor

        public NodeView(Node node, BSGraphView gView)
        {
            m_Node = node;
            m_GraphView = gView;
            SetPosition(new Rect(new Vector2(node.Position.X, node.Position.Y), Vector2.zero));
        }

        #endregion

        #region Override methods

        public override void OnSelected()
        {
            base.OnSelected();
        }

        public override void OnUnselected()
        {
            base.OnUnselected();
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {

        }

        #endregion

    }
}