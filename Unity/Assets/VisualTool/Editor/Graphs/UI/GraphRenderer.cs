using BSDesigner.LayoutProcessing;
using UnityEngine.UIElements;

namespace BSDesigner.Unity.VisualTool.Editor.Graphs
{
    public abstract class GraphRenderer
    {
        /// <summary>
        /// Compute the position of the nodes of the graph
        /// </summary>
        public abstract LayoutProcessor GetLayoutHandler();

        /// <summary>
        /// Add options to context menu in the graph nodes.
        /// </summary>
        /// <param name="nodeView">The node</param>
        /// <param name="evt">The context menu event</param>
        public abstract void BuildNodeContextualMenu(NodeView nodeView, ContextualMenuPopulateEvent evt);
    }
}
