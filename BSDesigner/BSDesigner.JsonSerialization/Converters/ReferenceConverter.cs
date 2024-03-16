using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace BSDesigner.JsonSerialization.Converters
{
    /// <summary>
    /// Json converter that use a list to store the references and serialize them using the index in the list.
    /// </summary>
    /// <typeparam name="T">The type of the elements stored in the reference list.</typeparam>
    public class ReferenceConverter<T> : JsonConverter<T> where T : class
    {
        private readonly List<T> _referenceList;

        // Same Id for repeated references?
        // Serialize invalid references as null or as -1.
        // When is an invalid reference?

        public ReferenceConverter(List<T> references)
        {
            _referenceList = references;
        }

        public override void WriteJson(JsonWriter writer, T? value, JsonSerializer serializer)
        {
            if (value != null)
            {
                var index = SetReference(value);
                writer.WriteValue(index);

            }
            else
            {
                writer.WriteNull();
            }
        }

        public override T? ReadJson(JsonReader reader, Type objectType, T? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.Integer) return null;

            var referenceIndex = reader.ReadAsInt32() ?? -1;
            return GetReference(referenceIndex);
        }

        private T? GetReference(int id)
        {
            if (id > 0 && id < _referenceList.Count)
            {
                return _referenceList[id];
            }
            return null;
        }

        private int SetReference(T element)
        {
            var index = _referenceList.Count;
            _referenceList.Add(element);
            return index;
        }
    }
}