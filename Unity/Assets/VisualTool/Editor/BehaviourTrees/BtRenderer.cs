using BSDesigner.BehaviourTrees;
using BSDesigner.LayoutProcessing;
using BSDesigner.Unity.VisualTool.Editor.Graphs;
using System;
using UnityEngine.UIElements;

namespace BSDesigner.Unity.VisualTool.Editor.BehaviourTrees
{
    [GraphRendererOfType(typeof(BehaviourTree))]
    public class BtRenderer : GraphRenderer
    {
        public override void BuildNodeContextualMenu(NodeView nodeView, ContextualMenuPopulateEvent evt)
        {
            evt.menu.AppendAction("Convert to root node", _ => ConvertToRootNode(nodeView));
        }

        public override LayoutProcessor GetLayoutHandler()
        {
            return new TreeLayoutProcessor(new System.Numerics.Vector2(200, 200));
        }

        private void ConvertToRootNode(NodeView nodeView)
        {
            
        }

    }
}
