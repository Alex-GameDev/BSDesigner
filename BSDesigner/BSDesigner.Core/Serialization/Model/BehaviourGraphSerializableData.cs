using BSDesigner.Core.Graphs;
using System;
using System.Collections.Generic;
using System.Text;

namespace BSDesigner.Core.Serialization.Model
{
    /// <summary>
    /// Serializable representation of a behaviour graph.
    /// </summary>
    internal class BehaviourGraphSerializableData
    {
        /// <summary>
        /// The behaviour engine
        /// </summary>
        public BehaviourGraph Graph;

        /// <summary>
        /// The list of nodes
        /// </summary>
        public IEnumerable<Node> Nodes;

        /// <summary>
        /// The list of connections
        /// </summary>
        public IEnumerable<ConnectionSerializableData> Connections;
    }
}
