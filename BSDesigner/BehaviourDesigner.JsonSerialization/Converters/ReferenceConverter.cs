using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace BSDesigner.JsonSerialization
{
    /// <summary>
    /// Custom converter that serializes all instances of <typeparamref name="T"/>
    /// </summary>
    /// <typeparam name="T">The type of the references.</typeparam>
    public class ReferenceConverter<T> : JsonConverter<T> where T : class
    {
        public List<T> referencedObjects = new List<T>();

        public override void WriteJson(JsonWriter writer, T? value, JsonSerializer serializer)
        {
            if (value != null)
            {
                writer.WriteValue(referencedObjects.Count);
                referencedObjects.Add(value);
            }
            else
            {
                writer.WriteNull();
            }
        }

        public override T? ReadJson(JsonReader reader, Type objectType, T? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Integer)
            {
                var referenceIndex = reader.ReadAsInt32() ?? -1;
                if (referenceIndex < 0 || referenceIndex >= referencedObjects.Count) return null;
                return referencedObjects[referenceIndex];
            }
            return null;
        }
    }
}