using BehaviourDesigner.JsonSerialization.Converters;
using BSDesigner.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BSDesigner.JsonSerialization.Converters
{
    public class BlackboardConverter : ContextConverter<Blackboard>
    {
        //private const string k_blackboardListToken = "fields";
        //private const string k_blackboardIdentifierToken = "$id";
        //private const string k_blackboardTypeToken = "$value";
        //private const string k_blackboardValueToken = "$value";

        public override Blackboard? ReadJson(JsonReader reader, Type objectType, Blackboard? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var list = serializer.Deserialize<List<BlackboardField>>(reader);

            foreach (var blackboardField in list)
            {
                Context.BlackboardFields.Add(blackboardField.Name, blackboardField);
            }

            var bb = new Blackboard(list);
            return bb;
        }

        public override void WriteJson(JsonWriter writer, Blackboard? value, JsonSerializer serializer)
        {
            if(value != null)
            {
                var list = value.GetAllFields().ToList();
                serializer.Serialize(writer, list);
            }
            else
            {
                writer.WriteNull();
            }

        }
    }
}
