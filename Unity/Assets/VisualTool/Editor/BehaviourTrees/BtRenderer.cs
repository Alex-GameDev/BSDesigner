using BSDesigner.BehaviourTrees;
using BSDesigner.LayoutProcessing;
using BSDesigner.Unity.VisualTool.Editor.Graphs;

namespace BSDesigner.Unity.VisualTool.Editor.BehaviourTrees
{
    [GraphRendererOfType(typeof(BehaviourTree))]
    public class BtRenderer : GraphRenderer
    {
        public override LayoutProcessor GetLayoutHandler()
        {
            return new TreeLayoutProcessor(new System.Numerics.Vector2(200, 200));
        }
    }
}
