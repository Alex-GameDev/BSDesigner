using System.Collections.Generic;

namespace BehaviourDesigner.JsonSerialization.Model
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