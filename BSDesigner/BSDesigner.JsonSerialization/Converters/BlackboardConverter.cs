using System;
using System.Linq;
using System.Runtime.Serialization;
using BSDesigner.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace BSDesigner.JsonSerialization.Converters
{
    /// <summary>
    /// Custom json converter for blackboards
    /// <para>Example1: { "id": "field_id", "fieldtype": "assembly, type", "value" : {}}</para>
    /// <para>Example2: { "id": "field_id", "fieldtype": "assembly, type"}</para>
    /// </summary>
    public class BlackboardConverter : ContextConverter<Blackboard>
    {
        private const string ID_TOKEN = "id";
        private const string TYPE_TOKEN = "fieldtype";
        private const string VALUE_TOKEN = "value";

        public BlackboardConverter(JsonConversionContext context) : base(context)
        {
        }

        public override void WriteJson(JsonWriter writer, Blackboard? value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
            }
            else
            {
                var list = value.GetAllFields();
                writer.WriteStartArray();
                foreach (var field in list)
                {
                    WriteField(field, writer, serializer);
                }
                writer.WriteEndArray();
            }
        }

        private void WriteField(BlackboardField field, JsonWriter writer, JsonSerializer serializer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName(ID_TOKEN);
            writer.WriteValue(field.Name);

            writer.WritePropertyName(TYPE_TOKEN);
            serializer.SerializationBinder.BindToName(field.Type, out var assemblyName, out var typeName);
            var typeValue = $"{typeName}, {assemblyName?.Split(",").FirstOrDefault()}";
            writer.WriteValue(typeValue);
            
            if (field.BaseValue != null)
            {
                writer.WritePropertyName(VALUE_TOKEN);
                serializer.Serialize(writer, field.BaseValue, typeof(object));
            }

            writer.WriteEndObject();
        }

        public override Blackboard? ReadJson(JsonReader reader, Type objectType, Blackboard? existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            var value = new Blackboard();

            reader.Read();

            while (reader.TokenType != JsonToken.EndArray)
            {
                ReadField(value, reader, serializer);

            }
            Context.GlobalBlackboard = value;
            return value;
        }

        private void ReadField(Blackboard blackboard, JsonReader reader, JsonSerializer serializer)
        {
            var jobject = JToken.ReadFrom(reader);
            reader.Read();

            var id = jobject[ID_TOKEN]?.Value<string>();
            var fieldTypeToken = jobject[TYPE_TOKEN]?.Value<string>();
            var valueToken = jobject[VALUE_TOKEN];
            if (id == null || fieldTypeToken == null)
            {
                return;
            }

            var fieldType = GetTypeFormSerializedString(fieldTypeToken, serializer.SerializationBinder); // Get the blackboard field type

            var concreteTypeToken = valueToken.HasValues ? valueToken?["$type"]?.Value<string>() : null;
            var concreteFieldType = concreteTypeToken != null ? GetTypeFormSerializedString(concreteTypeToken, serializer.SerializationBinder) : fieldType; // Get the value field type

            if (valueToken != null && concreteFieldType != null && fieldType != null)
            {
                var value = valueToken.ToObject(concreteFieldType);
                blackboard.CreateField(id, fieldType, value);
            }
        }

        private static Type? GetTypeFormSerializedString(string? str, ISerializationBinder binder)
        {
            if (str == null)
            {
                return null;
            }

            string[] fieldTypeTokens = str.Split(',');
            var typeName = fieldTypeTokens[0].Trim();
            var assemblyName = fieldTypeTokens[1].Trim();
            return binder.BindToType(assemblyName, typeName);
        }

    }
}