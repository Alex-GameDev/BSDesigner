using System.Collections.Generic;

namespace BSDesigner.JsonSerialization.Model
{
    /// <summary>
    /// A serializable representation of a behaviour system
    /// </summary>
    public class SerializedSystem
    {
        /// <summary>
        /// A list of behaviour engines included in the system
        /// </summary>
        public List<SerializedEngine>? Engines;
    }
}