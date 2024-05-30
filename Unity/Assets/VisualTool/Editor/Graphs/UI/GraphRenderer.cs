using BSDesigner.LayoutProcessing;

namespace BSDesigner.Unity.VisualTool.Editor.Graphs
{
    public abstract class GraphRenderer
    {
        /// <summary>
        /// Compute the position of the nodes of the graph
        /// </summary>
        public abstract LayoutProcessor GetLayoutHandler();
    }
}
