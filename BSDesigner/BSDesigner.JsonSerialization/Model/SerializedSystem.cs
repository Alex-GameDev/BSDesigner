using System.Collections.Generic;
using BSDesigner.Core;

namespace BSDesigner.JsonSerialization.Model
{
    /// <summary>
    /// A serializable representation of a behaviour system
    /// </summary>
    public class SerializedSystem
    {
        /// <summary>
        /// A common bloackboard for all the engines in the system
        /// </summary>
        public Blackboard? Blackboard;

        /// <summary>
        /// A list of behaviour engines included in the system
        /// </summary>
        public List<SerializedEngine>? Engines;
    }
}