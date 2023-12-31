using System.Collections.Generic;

namespace BSDesigner.JsonSerialization
{
    /// <summary>
    /// A serializable representation of a behaviour system
    /// </summary>
    public class BehaviourSystemDto
    {
        /// <summary>
        /// A list of behaviour engines included in the system
        /// </summary>
        public List<BehaviourEngineDto> Engines = new List<BehaviourEngineDto>();
    }
}