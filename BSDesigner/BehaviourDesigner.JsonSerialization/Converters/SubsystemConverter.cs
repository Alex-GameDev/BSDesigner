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
    public class SubsystemConverter : ContextConverter<Subsystem>
    {
        public override void WriteJson(JsonWriter writer, Subsystem? value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("$id");

            var subsystemId = -1;
            if (value != null && value.Engine != null)
            {
                subsystemId = Context.Engines?.IndexOf(value.Engine) ?? -1;
            }
            
            writer.WriteValue(subsystemId);
            writer.WriteEndObject();
        }

        public override Subsystem ReadJson(JsonReader reader, Type objectType, Subsystem existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            var subsystem = new Subsystem();
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