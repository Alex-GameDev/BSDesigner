using BSDesigner.Core.Graphs;
using BSDesigner.Core.Serialization.Converters;
using BSDesigner.Core.Serialization.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace BSDesigner.Core.Serialization
{
    public static class JsonUtilities
    {
        /// <summary>
        /// Serialize a behaviour graph in json format
        /// </summary>
        /// <param name="graph">The serialized graph</param>
        /// <returns>The json string</returns>
        public static string Serialize(BehaviourEngine graph)
        {
            var engines = new List<BehaviourEngine> { graph };
            return Serialize(engines);
        }

        /// <summary>
        /// Serialize a collection of behaviour engines in json format
        /// </summary>
        /// <param name="engines">The serialized engines</param>
        /// <returns>The json string</returns>
        public static string Serialize(IEnumerable<BehaviourEngine> engines)
        {
            var dto = new BehaviourSystemDto
            {
                Engines = engines.Select(DtoConversion.FromEngineToDto).ToList()
            };

            var context = new JsonSerializationContext
            {
                Engines = engines.ToList()
            };
            return SerializeDto(dto, context);
        }

        /// <summary>
        /// Serialize a node in json format
        /// </summary>
        /// <param name="node">The serialized node</param>
        /// <returns>The json string</returns>
        public static string SerializeNode(Node node)
        {
            var context = new JsonSerializationContext();
            var settings = CreateSerializerSettings(context);
            return JsonConvert.SerializeObject(node, settings);
        }

        /// <summary>
        /// Deserialize a collection of behaviour engines
        /// </summary>
        /// <param name="jsonData">The json string deserialized.</param>
        /// <returns>The list of behaviour engines deserialized</returns>
        public static BehaviourEngine? Deserialize(string jsonData)
        {
            var context = new JsonSerializationContext();
            var dto = DeserializeDto(jsonData, context);
            if (dto == null) return null;

            var engine = dto.Engines[0];
            return DtoConversion.FromDtoToEngine(engine);
        }

        /// <summary>
        /// Deserialize a collection of behaviour engines
        /// </summary>
        /// <param name="jsonData">The json string deserialized.</param>
        /// <returns>The list of behaviour engines deserialized</returns>
        public static List<BehaviourEngine> DeserializeList(string jsonData)
        {
            var context = new JsonSerializationContext();
            var dto = DeserializeDto(jsonData, context);

            if (dto == null) return new List<BehaviourEngine>();

            var engines = dto.Engines.Select(DtoConversion.FromDtoToEngine).ToList();

            foreach (var (subsystem, index) in context.SubsystemMap)
            {
                subsystem.Value = engines[index];
            }

            return engines;
        }



        /// <summary>
        /// Deserialize a node
        /// </summary>
        /// <param name="jsonData">The json string deserialized.</param>
        /// <returns>The list of behaviour engines deserialized</returns>
        public static T? DeserializeNode<T>(string jsonData) where T: Node
        {
            var context = new JsonSerializationContext();
            var settings = CreateSerializerSettings(context);
            return JsonConvert.DeserializeObject<T>(jsonData, settings);
        }

        public static string Serialize(Blackboard blackboard)
        {
            var context = new JsonSerializationContext();
            var settings = CreateSerializerSettings(context);

            var fields = blackboard.GetAllFields();
            return JsonConvert.SerializeObject(fields, settings);
        }


        public static Blackboard DeserializeBlackboard(string jsonData)
        {
            var context = new JsonSerializationContext();
            var settings = CreateSerializerSettings(context);

            var fields = JsonConvert.DeserializeObject<List<BlackboardField>>(jsonData, settings);

            return fields != null ? new Blackboard(fields) : new Blackboard();
        }

        private static string SerializeDto(BehaviourSystemDto? dto, JsonSerializationContext context)
        {
            var settings = CreateSerializerSettings(context);
            return JsonConvert.SerializeObject(dto, settings);
        }

        private static BehaviourSystemDto? DeserializeDto(string jsonData, JsonSerializationContext context)
        {
            var settings = CreateSerializerSettings(context);
            return JsonConvert.DeserializeObject<BehaviourSystemDto>(jsonData, settings);
        }

        private static JsonSerializerSettings CreateSerializerSettings(JsonSerializationContext context)
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new BSDContractResolver(),
                TypeNameHandling = TypeNameHandling.Auto,
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            };
            //settings.Converters.Add(new NodeConnectionConverter());
            settings.Converters.Add(new SubsystemConverter { Context = context });
            settings.Converters.Add(new ParameterConverter { Context = context });
            settings.Converters.Add(new BlackboardConverter { Context = context });
            return settings;
        }

    }
}
