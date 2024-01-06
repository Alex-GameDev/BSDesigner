using System.Collections.Generic;
using Newtonsoft.Json;

namespace BSDesigner.JsonSerialization
{
    /// <summary>
    /// Class used to configure the serialization outside the json assembly.
    /// </summary>
    public class JsonSettings
    {
        private List<JsonConverter> converters = new List<JsonConverter>();

        /// <summary>
        /// Add a new reference converter that uses <paramref name="references"/>
        /// to write or read the serialized/deserialized elements.
        /// <para>In serialization, references is an empty list that is populated after the process.</para>
        /// <para>In deserialization, references is populated before the process.</para>
        /// </summary>
        /// <typeparam name="T">The type of the elements stored.</typeparam>
        /// <param name="references">The reference list.</param>
        public void AddReferenceConverter<T>(ref List<T> references) where T : class
        {
            var converter = new ReferenceConverter<T>();
            converter.referencedObjects = references;
        }
    }
}