using System.Collections.Generic;
using BSDesigner.Core;

namespace BSDesigner.JsonSerialization
{
    /// <summary>
    /// Serializable representation a behaviour engine.
    /// </summary>
    public class BehaviourEngineDto
    {
        /// <summary>
        /// The local blackboard of the engine
        /// </summary>
        public List<BlackboardField>? Blackboard;

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
        public List<ConnectionDto>? Connections;
    }
}