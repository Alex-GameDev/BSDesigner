using System.Collections.Generic;
using System.Linq;
using BSDesigner.Core;
using Newtonsoft.Json;

namespace BSDesigner.JsonSerialization
{
    public static class JsonUtilities
    {
        #region Public static methods

        /// <summary>
        /// Serialize a single behaviour engine.
        /// </summary>
        /// <param name="engine">The serialized engine</param>
        /// <param name="settings">The settings.</param>
        /// <returns>The json string</returns>
        public static string Serialize(BehaviourEngine engine, JsonSettings? settings = null) => Serialize(new List<BehaviourEngine> { engine }, null, settings);

        /// <summary>
        /// Serialize a collection of behaviour engines in json format.
        /// </summary>
        /// <param name="engines">The serialized engines.</param>
        /// <param name="blackboard">The blackboard.</param>
        /// <param name="settings">The settings.</param>
        /// <returns>The json string</returns>
        public static string Serialize(IEnumerable<BehaviourEngine> engines, Blackboard? blackboard = null, JsonSettings? settings = null)
        {
            var dto = new BehaviourSystemDto
            {
                Engines = engines.Select(DtoConversion.FromEngineToDto).ToList(),
                Blackboard = blackboard?.GetAllFields().ToList(),
            };

            var context = new JsonSerializationContext
            {
                Engines = engines.ToList()
            };
            return SerializeDto(dto, context, settings ?? new JsonSettings());
        }

        /// <summary>
        /// Deserialize a collection of behaviour engines
        /// </summary>
        /// <param name="jsonData">The json string deserialized.</param>
        /// <returns>The list of behaviour engines deserialized</returns>
        public static List<BehaviourEngine> Deserialize(string jsonData, JsonSettings? settings = null)
        {
            var context = new JsonSerializationContext();
            var dto = DeserializeDto(jsonData, context, settings ?? new JsonSettings());
            var engines = dto.Engines.Select(DtoConversion.FromDtoToEngine).ToList();

            foreach (var (subsystem, index) in context.SubsystemMap)
            {
                subsystem.Value = engines[index];
            }

            return engines;
        }

        #endregion
        
        //public static string Serialize(Blackboard blackboard)
        //{
        //    var context = new JsonSerializationContext();
        //    var settings = CreateSerializerSettings(context);

        //    var fields = blackboard.GetAllFields();
        //    return JsonConvert.SerializeObject(fields, settings);
        //}

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
            serializerSettings.Converters.Add(new SubsystemConverter { Context = context });
            serializerSettings.Converters.Add(new ParameterConverter { Context = context });
            serializerSettings.Converters.Add(new BlackboardConverter { Context = context });
            return serializerSettings;
        }

    }
}
