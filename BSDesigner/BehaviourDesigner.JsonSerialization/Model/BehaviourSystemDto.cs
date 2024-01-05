using System.Collections.Generic;
using BSDesigner.Core;

namespace BehaviourDesigner.JsonSerialization.Model
{
    /// <summary>
    /// A serializable representation of a behaviour system
    /// </summary>
    public class BehaviourSystemDto
    {
        /// <summary>
        /// The local blackboard of the engine
        /// </summary>
        public Blackboard? Blackboard;

        /// <summary>
        /// A list of behaviour engines included in the system
        /// </summary>
        public List<BehaviourEngineDto> Engines = new List<BehaviourEngineDto>();
    }
}