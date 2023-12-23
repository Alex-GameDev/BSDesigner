using System.Collections.Generic;
using System.Linq;
using BehaviourDesigner.JsonSerialization.Converters;
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
            var settings = CreateSerializerSettings();
            var result = JsonConvert.SerializeObject(graph, settings);
            return result;
        }

        /// <summary>
        /// Serialize a collection of behaviour engines in json format
        /// </summary>
        /// <param name="engines">The serialized engines</param>
        /// <returns>The json string</returns>
        public static string SerializeList(IEnumerable<BehaviourEngine> engines)
        {
            var settings = CreateSerializerSettings();
            return JsonConvert.SerializeObject(engines, settings);
        }

        /// <summary>
        /// Serialize a collection of behaviour engines in json format
        /// </summary>
        /// <param name="engines">The serialized engines</param>
        /// <returns>The json string</returns>
        public static string Serialize(params BehaviourEngine[] engines) => SerializeList(engines);
        
        /// <summary>
        /// Deserialize a collection of behaviour engines
        /// </summary>
        /// <param name="jsonData"></param>
        /// <returns></returns>
        public static List<BehaviourEngine> Deserialize(string jsonData)
        {
            var settings = CreateSerializerSettings();
            var data = JsonConvert.DeserializeObject<IEnumerable<BehaviourEngine>>(jsonData, settings);
            return data.ToList();
        }

        private static JsonSerializerSettings CreateSerializerSettings()
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new BSDContractResolver(),
                TypeNameHandling = TypeNameHandling.Auto,
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            settings.Converters.Add(new BehaviourEngineConverter());
            //settings.Converters.Add(new BehaviourGraphConverter());
            settings.Converters.Add(new SubsystemConverter());
            //settings.Converters.Add(new DefaultEmptyStringConverter());

            return settings;
        }
    }
}
