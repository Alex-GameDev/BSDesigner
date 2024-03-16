using System;
using BSDesigner.Core;
using Newtonsoft.Json;

namespace BSDesigner.JsonSerialization.Converters
{
    /// <summary>
    /// Custom json converter for blackboards
    /// </summary>
    public class BlackboardConverter : ContextConverter<Blackboard>
    {
        public override void WriteJson(JsonWriter writer, Blackboard? value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
            }
            else
            {
                var list = value.GetAllFields();
                foreach (var field in list)
                {
                    var valueType = field.BaseValue?.GetType();

                    writer.WriteStartObject();

                    writer.WritePropertyName("id");
                    writer.WriteValue(field.Name);

                    if (valueType != field.Type)
                    {
                        writer.WritePropertyName("$type");
                        writer.WriteValue(field.Type);
                    }

                    if (field.BaseValue != null)
                    {
                        writer.WritePropertyName("value");
                        serializer.Serialize(writer, field.BaseValue, valueType);
                    }

                    writer.WriteEndObject();
                }
            }

            if (Context.GlobalBlackboard == null)
            {
                Context.GlobalBlackboard = value;
            }
            else
            {
                Context.LocalBlackboard = value;
            }
        }

        public override Blackboard? ReadJson(JsonReader reader, Type objectType, Blackboard? existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            var value = new Blackboard();

            //TODO:

            if (Context.GlobalBlackboard == null)
            {
                Context.GlobalBlackboard = value;
            }
            else
            {
                Context.LocalBlackboard = value;
            }

            return value;
        }

        public BlackboardConverter(JsonConversionContext context) : base(context)
        {
        }
    }
}