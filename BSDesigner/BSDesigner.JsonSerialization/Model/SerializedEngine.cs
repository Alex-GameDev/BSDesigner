using BSDesigner.Core;
using System.Collections.Generic;
using System.Linq;

namespace BSDesigner.JsonSerialization.Model
{
    /// <summary>
    /// Serializable representation of a behaviour engine
    /// </summary>
    public class SerializedEngine
    {
        /// <summary>
        /// The behaviour engine
        /// </summary>
        public BehaviourEngine? Engine;

        /// <summary>
        /// The list of nodes (Behaviour graph only)
        /// </summary>
        public List<Node>? Nodes;

        /// <summary>
        /// The list of connections (Behaviour graph only)
        /// </summary>
        public List<SerializedConnection>? Connections;
    }
}