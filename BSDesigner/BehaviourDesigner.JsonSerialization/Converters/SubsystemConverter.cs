using System;
using System.Collections.Generic;
using BSDesigner.Core;
using Newtonsoft.Json;

namespace BehaviourDesigner.JsonSerialization.Converters
{
    /// <summary>
    /// <para>SERIALIZATION: Find the subsystem identifier in a dictionary. </para>
    /// <para>DESERIALIZATION: Store the references. </para>
    /// </summary>
    public class SubsystemConverter : ContextConverter<Parameter<BehaviourEngine>>
    {
        public override void WriteJson(JsonWriter writer, Parameter<BehaviourEngine>? value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("$id");

            var subsystemId = -1;
            if (value != null && value.Value != null)
            {
                subsystemId = Context.Engines?.IndexOf(value.Value) ?? -1;
            }
            
            writer.WriteValue(subsystemId);
            writer.WriteEndObject();
        }

        public override Parameter<BehaviourEngine> ReadJson(JsonReader reader, Type objectType, Parameter<BehaviourEngine>? existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            var subsystem = (Parameter<BehaviourEngine>) Activator.CreateInstance(objectType);
            reader.Read();
            while (reader.TokenType != JsonToken.EndObject)
            {
                if (reader.TokenType == JsonToken.PropertyName && reader.Value as string == "$id")
                {
                    var id = reader.ReadAsInt32();
                    Context.SubsystemMap[subsystem] = id ?? -1;
                }
                reader.Read();
            }
            return subsystem;
        }
    }
}