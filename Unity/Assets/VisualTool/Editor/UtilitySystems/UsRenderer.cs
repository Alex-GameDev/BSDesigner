using BSDesigner.LayoutProcessing;
using BSDesigner.Unity.VisualTool.Editor.Graphs;
using BSDesigner.UtilitySystems;
using UnityEngine.UIElements;

namespace BSDesigner.Unity.VisualTool.Editor
{
    [GraphRendererOfType(typeof(UtilitySystem))]
    public class UsRenderer : GraphRenderer
    {
        public override void BuildNodeContextualMenu(NodeView nodeView, ContextualMenuPopulateEvent evt)
        {
            
        }

        public override LayoutProcessor GetLayoutHandler()
        {
            return new LayeredLayoutProcessor(new System.Numerics.Vector2(200, 200));
        }
    }
}
