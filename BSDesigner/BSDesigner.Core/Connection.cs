namespace BSDesigner.Core
{
    /// <summary>
    /// Element that represents a connection between two nodes in a graph.
    /// </summary>
    public struct Connection
    {
        public Connection(Node source, Node target)
        {
            this.Source = source;
            this.Target = target;
        }

        /// <summary>
        /// The source node.
        /// </summary>
        public Node Source { get; set; }

        /// <summary>
        /// The target node.
        /// </summary>
        public Node Target { get; set; }
    }
}
