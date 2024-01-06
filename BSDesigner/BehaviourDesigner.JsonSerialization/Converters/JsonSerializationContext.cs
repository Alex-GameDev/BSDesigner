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
        /// All the blackboard fields.
        /// </summary>
        public Dictionary<string, BlackboardField> BlackboardFields = new Dictionary<string, BlackboardField>();

        /// <summary>
        /// Store the relation between a subsystem and the engine that references.
        /// </summary>
        public Dictionary<Parameter<BehaviourEngine>, int> SubsystemMap { get; set; } = new Dictionary<Parameter<BehaviourEngine>, int>();

        /// <summary>
        /// Store the parameters that are bound to a blackboard field to set the value just after deserialization
        /// </summary>
        public Dictionary<Parameter, string> BoundParameterMap { get; set; } = new Dictionary<Parameter, string>();
    }
}