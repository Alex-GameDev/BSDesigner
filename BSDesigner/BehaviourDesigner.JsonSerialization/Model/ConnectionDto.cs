using System.ComponentModel;

namespace BSDesigner.JsonSerialization
{
    /// <summary>
    /// Represent a connection between two nodes in a graph
    /// </summary>
    public class ConnectionDto
    {
        /// <summary>
        /// The index of the source node in the graph node list
        /// </summary>
        [DefaultValue(-1)]
        public int SourceId;

        /// <summary>
        /// The index of the target node in the graph node list
        /// </summary>
        [DefaultValue(-1)]
        public int TargetId;
    }
}