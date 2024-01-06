using System.Collections.Generic;
using System.Linq;
using BSDesigner.Core;
using BSDesigner.JsonSerialization.Converters;
using Newtonsoft.Json;

namespace BSDesigner.JsonSerialization
{
    public static class JsonUtilities
    {
        private static readonly string Version = "1.0.0";
        private static readonly string Target = ".NET Standard 2.1";

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
            return SerializeDto(dto, context, new JsonSettings());
        }

        /// <summary>
        /// Serialize a collection of behaviour engines in json format
        /// </summary>
        /// <param name="engines">The serialized engines</param>
        /// <returns>The json string</returns>
        public static string Serialize(IEnumerable<BehaviourEngine> engines, JsonSettings settings)
        {
            var dto = new BehaviourSystemDto
            {
                Engines = engines.Select(DtoConversion.FromEngineToDto).ToList()
            };

            var context = new JsonSerializationContext
            {
                Engines = engines.ToList()
            };
            return SerializeDto(dto, context, settings);
        }


        /// <summary>
        /// Deserialize a collection of behaviour engines
        /// </summary>
        /// <param name="jsonData">The json string deserialized.</param>
        /// <returns>The list of behaviour engines deserialized</returns>
        public static List<BehaviourEngine> Deserialize(string jsonData)
        {
            var context = new JsonSerializationContext();
            var dto = DeserializeDto(jsonData, context, new JsonSettings());
            var engines = dto.Engines.Select(DtoConversion.FromDtoToEngine).ToList();

            foreach (var (subsystem, index) in context.SubsystemMap)
            {
                subsystem.Value = engines[index];
            }

            return engines;
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
            var context = new JsonSerializationContext();
            var dto = DeserializeDto(jsonData, context, settings);
            var engines = dto.Engines.Select(DtoConversion.FromDtoToEngine).ToList();

            foreach (var (subsystem, index) in context.SubsystemMap)
            {
                subsystem.Engine = engines[index];
            }

            return engines;
        }

        private static string SerializeDto(BehaviourSystemDto? dto, JsonSerializationContext context, JsonSettings serializerSettings)
        {
            var settings = CreateSerializerSettings(context, serializerSettings);
            return JsonConvert.SerializeObject(dto, settings);
        }

        private static BehaviourSystemDto? DeserializeDto(string jsonData, JsonSerializationContext context, JsonSettings settings)
        {
            var serializerSettings = CreateSerializerSettings(context, settings);
            return JsonConvert.DeserializeObject<BehaviourSystemDto>(jsonData, serializerSettings);
        }

        private static JsonSerializerSettings CreateSerializerSettings(JsonSerializationContext context, JsonSettings settings)
        {
            var serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new BSDContractResolver(),
                TypeNameHandling = TypeNameHandling.Auto,
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            //settings.Converters.Add(new NodeConnectionConverter());
            settings.Converters.Add(new SubsystemConverter { Context = context });
            settings.Converters.Add(new ParameterConverter { Context = context });
            settings.Converters.Add(new BlackboardConverter { Context = context });
            return settings;
        }

    }
}
