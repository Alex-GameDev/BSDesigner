using System.Numerics;

namespace BSDesigner.Core.Graphs.Processors
{
    /// <summary>
    /// Graph processor that modify the position of the nodes to create a specific layout.
    /// </summary>
    public abstract class LayoutProcessor : IGraphProcessor
    {
        public Vector2 NodeOffset { get; set; }

        protected LayoutProcessor(Vector2 offset)
        {
            NodeOffset = offset;
        }
                   

        public void Apply(BehaviourGraph graph)
        {
            if(graph.Nodes.Count == 0)
            {
                return;
            }

            ComputeLayout(graph);
        }

        protected abstract void ComputeLayout(BehaviourGraph graph);
    }
}
