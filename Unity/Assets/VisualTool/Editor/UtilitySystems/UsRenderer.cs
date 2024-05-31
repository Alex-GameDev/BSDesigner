using BSDesigner.LayoutProcessing;
using BSDesigner.Unity.VisualTool.Editor.Graphs;
using BSDesigner.UtilitySystems;

namespace BSDesigner.Unity.VisualTool.Editor
{
    [GraphRendererOfType(typeof(UtilitySystem))]
    public class UsRenderer : GraphRenderer
    {
        public override LayoutProcessor GetLayoutHandler()
        {
            return new LayeredLayoutProcessor(new System.Numerics.Vector2(200, 200));
        }
    }
}
