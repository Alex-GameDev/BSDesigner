using System;
using System.Linq;
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

        private const string k_FieldToken = "$field";

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

            var fieldToken = token[k_FieldToken];
            if (fieldToken != null)
            {
                var fieldId = fieldToken.ToObject<string>();
                var field = GetBoundField(fieldId);
                //return Parameter.CreateByBoundField(field);
            }
            else
            {
                var value = token.ToObject(objectType, serializer);
                var parameterType = objectType.GetGenericArguments().FirstOrDefault();
                if (parameterType.IsAssignableFrom(value.GetType()))
                {

                }
            }
            return null;

        }

        private BlackboardField GetBoundField(string? fieldId)
        {
            throw new NotImplementedException();
        }

        //private Parameter CreateBoundedParameter(string? fieldId)
        //{

        //}

        //private Parameter CreateValueParameter(object value)
        //{

        //}
        public ParameterConverter(JsonConversionContext context) : base(context)
        {
        }
    }
}