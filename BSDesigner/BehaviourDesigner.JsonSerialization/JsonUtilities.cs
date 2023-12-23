using System.Collections.Generic;
using System.Linq;
using BehaviourDesigner.JsonSerialization.Converters;
using BehaviourDesigner.JsonSerialization.Model;
using BSDesigner.Core;
using Newtonsoft.Json;

namespace BehaviourDesigner.JsonSerialization
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
            return SerializeDto(dto, context);
        }
        
        /// <summary>
        /// Deserialize a collection of behaviour engines
        /// </summary>
        /// <param name="jsonData">The json string deserialized.</param>
        /// <returns>The list of behaviour engines deserialized</returns>
        public static List<BehaviourEngine> Deserialize(string jsonData)
        {
            var context = new JsonSerializationContext();
            var dto = DeserializeDto(jsonData, context);
            var engines = dto.Engines.Select(DtoConversion.FromDtoToEngine).ToList();

            foreach (var (subsystem, index) in context.SubsystemMap)
            {
                subsystem.Engine = engines[index];
            }

            return engines;
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
            return settings;
        }
    }
}
