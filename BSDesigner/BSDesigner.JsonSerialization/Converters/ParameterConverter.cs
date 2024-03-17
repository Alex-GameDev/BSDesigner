using System;
using System.Linq;
using System.Reflection;
using BSDesigner.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BSDesigner.JsonSerialization.Converters
{
    /// <summary>
    /// Json converter that serializes bound and value parameters.
    /// </summary>
    public class ParameterConverter : ContextConverter<Parameter>
    {

        private const string k_FieldToken = "id";

        public override void WriteJson(JsonWriter writer, Parameter? value, JsonSerializer serializer)
        {
            if (value == null) //Parameter is null
            {
                writer.WriteNull();
            }
            else if (value.BaseBoundField != null) //Parameter is bound to a blackboard
            {
                writer.WriteStartObject();
                writer.WritePropertyName(k_FieldToken);
                writer.WriteValue(value.BaseBoundField.Name);
                writer.WriteEndObject();
            }
            else if(value.InternalValue != null)//Parameter is unbound
            {
                serializer.Serialize(writer, value.InternalValue, value.Type);
            }
            else
            {
                writer.WriteNull();
            }
        }

        public override Parameter? ReadJson(JsonReader reader, Type objectType, Parameter? existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            var token = JToken.ReadFrom(reader);
            var parameter = (Parameter)Activator.CreateInstance(objectType);
           
            if (token.HasValues)
            {
                var fieldToken = token[k_FieldToken];
                if (fieldToken != null)
                {
                    var fieldId = fieldToken.ToObject<string>();
                    var field = GetBoundField(fieldId);
                    var fieldInfo = objectType.GetField("m_BoundField", BindingFlags.Instance | BindingFlags.NonPublic);
                    fieldInfo?.SetValue(parameter, field);
                    return parameter;
                }
            }

            var valueInfo = objectType.GetField("m_Value", BindingFlags.Instance | BindingFlags.NonPublic);
            var value = token.ToObject(parameter.Type);
            valueInfo?.SetValue(parameter, value);
            return parameter;
        }

        private BlackboardField GetBoundField(string? fieldId)
        {
            return Context?.GlobalBlackboard?.GetFieldById(fieldId);
        }

        public ParameterConverter(JsonConversionContext context) : base(context)
        {
        }
    }
}