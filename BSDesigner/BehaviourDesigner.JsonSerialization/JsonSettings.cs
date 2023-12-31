using System.Collections.Generic;
using Newtonsoft.Json;

namespace BSDesigner.JsonSerialization
{
    public class JsonSettings
    {
        public IReadOnlyList<JsonConverter> Converters => converters;
        public bool Pretty { get; set; }

        private List<JsonConverter> converters = new List<JsonConverter>();

        public void AddReferenceConverter<T>(ref List<T> references) where T : class
        {
            var converter = new ReferenceConverter<T>();
            converter.referencedObjects = references;
        }
    }
}