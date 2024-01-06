using System;
using System.Collections.Generic;
using System.Linq;
using BSDesigner.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BehaviourDesigner.JsonSerialization.Converters
{
    public class ParameterConverter : ContextConverter<Parameter>
    {
        const string k_FieldToken = "$field";
        public override void WriteJson(JsonWriter writer, Parameter? value, JsonSerializer serializer)
        {
            if(value == null) return;

            if (value.BaseBoundField != null)
            {
                writer.WriteStartObject();
                writer.WritePropertyName(k_FieldToken);
                writer.WriteValue(value.BaseBoundField.Name);
                writer.WriteEndObject();
            }
            else
            {
                serializer.Serialize(writer, value.ObjectValue, value.Type);
            }

        }

        public override Parameter? ReadJson(JsonReader reader, Type objectType, Parameter? existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            var parameter = (Parameter)Activator.CreateInstance(objectType);
            var token = JToken.ReadFrom(reader);
            if (token.Children().Any() && token.First?.ToObject<string>() == k_FieldToken)
            {
                var value = token[k_FieldToken];
                if (value != null)
                {
                    parameter.BaseBoundField = Context.BlackboardFields.GetValueOrDefault(value.ToObject<string>());
                }
                //Context.BoundParameterMap.Add(parameter, value.ToObject<string>());
            }
            else
            {
                var value = token.ToObject(objectType, serializer);
            }

            return parameter;
        }
    }
}