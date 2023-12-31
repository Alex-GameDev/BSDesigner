using System;
using System.Collections.Generic;
using BSDesigner.Core;

namespace BSDesigner.JsonSerialization
{
    /// <summary>
    /// Class that allows json converters to read and write data during the serialization / deserialization.
    /// </summary>
    public class JsonSerializationContext
    {
        /// <summary>
        /// The serialized engines
        /// </summary>
        public List<BehaviourEngine> Engines { get; set; } = new List<BehaviourEngine>();

        /// <summary>
        /// Store the relation between a subsystem and the engine that references.
        /// </summary>
        public Dictionary<Subsystem, int> SubsystemMap { get; set; } = new Dictionary<Subsystem, int>();
    }
}