namespace BSDesigner.Core.Graphs
{
    /// <summary>
    /// Defines an element that can process a behaviour graph
    /// </summary>
    public interface IGraphProcessor
    {
        /// <summary>
        /// Process an specific graph
        /// </summary>
        /// <param name="graph">The graph processed</param>
        public void Apply(BehaviourGraph graph);
    }
}
