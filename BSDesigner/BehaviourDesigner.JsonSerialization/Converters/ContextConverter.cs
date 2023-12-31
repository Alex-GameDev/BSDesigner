using Newtonsoft.Json;

namespace BSDesigner.JsonSerialization
{
    /// <summary>
    /// Custom json converter that can read and write variables in the context
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ContextConverter<T> : JsonConverter<T>
    {
        /// <summary>
        /// The serialization context
        /// </summary>
        public JsonSerializationContext Context { get; set; }
    }
}