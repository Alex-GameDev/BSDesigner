using System.Collections.Generic;
using System.Linq;
using BSDesigner.Core;
using BSDesigner.JsonSerialization.Converters;
using BSDesigner.JsonSerialization.Model;
using BSDesigner.JsonSerialization.Settings;
using Newtonsoft.Json;

namespace BSDesigner.JsonSerialization
{
    /// <summary>
    /// Json utilities
    /// </summary>
    public static class JsonSerialization
    {
        /// <summary>
        /// Serialize a behaviour system in json format.
        /// </summary>
        /// <param name="system">The serialized system.</param>
        /// <returns>The serialized data.</returns>
        public static string Serialize(BehaviourSystem? system)
        {
            var context = new JsonConversionContext();
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,
                ContractResolver = new BSDContractResolver(),
                Converters = new List<JsonConverter>
                {
                    new BlackboardConverter(context),
                    new ParameterConverter(context)
                }
            };
            var serializableSystem = SystemToSerializedFormat(system);
            return JsonConvert.SerializeObject(serializableSystem, settings);
        }

        /// <summary>
        /// Deserialize a behaviour system from json format.
        /// </summary>
        /// <param name="jsonData">The serialized data.</param>
        /// <returns>The deserialized system.</returns>
        public static BehaviourSystem? Deserialize(string jsonData)
        {
            var context = new JsonConversionContext();
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,
                ContractResolver = new BSDContractResolver(),
                Converters = new List<JsonConverter>
                {
                    new BlackboardConverter(context),
                    new ParameterConverter(context)
                }
            };

            var serializableSystem = JsonConvert.DeserializeObject<SerializedSystem>(jsonData, settings);
            return SystemToBusinessFormat(serializableSystem);
        }

        #region Private methods

        private static SerializedSystem? SystemToSerializedFormat(BehaviourSystem? system)
        {
            if (system == null)
            {
                return null;
            }

            var serializedSystem = new SerializedSystem();

            if (system.engines.Count > 0)
            {
                serializedSystem.Engines = system.engines.Select(EngineToSerializedFormat).ToList();
            }
            serializedSystem.Blackboard = system.blackboard;
            return serializedSystem;
        }

        private static SerializedEngine EngineToSerializedFormat(BehaviourEngine engine)
        {
            var serializedEngine = new SerializedEngine
            {
                Engine =  engine
            };

            if (engine is BehaviourGraph graph)
            {
                serializedEngine.Nodes = graph.Nodes.ToList();
                serializedEngine.Connections = GetConnections(graph.Nodes);
            }

            return serializedEngine;
        }

        private static BehaviourSystem? SystemToBusinessFormat(SerializedSystem? serializedSystem)
        {
            if (serializedSystem == null)
            {
                return null;
            }

            var system = new BehaviourSystem();

            if (serializedSystem.Engines != null && serializedSystem.Engines.Count > 0)
            {
                system.engines = serializedSystem.Engines.Select(EngineToBusinessFormat).ToList();
            }

            system.blackboard = serializedSystem.Blackboard;
            return system;
        }

        private static BehaviourEngine? EngineToBusinessFormat(SerializedEngine? serializedEngine)
        {
            var engine = serializedEngine?.Engine;
            if (engine is BehaviourGraph graph && serializedEngine?.Nodes != null)
            {
                serializedEngine.Nodes.ForEach(node => graph.AddNode(node));

                if (serializedEngine.Connections != null)
                {
                    foreach (var connection in serializedEngine.Connections)
                    {
                        var source = graph.Nodes[connection.SourceId];
                        var target = graph.Nodes[connection.TargetId];
                        graph.ConnectNodes(source, target);
                    }
                }
            }

            return engine;
        }

        private static List<SerializedConnection>? GetConnections(IEnumerable<Node> nodes)
        {
            var i = 0;
            var nodeIndexMap = nodes.ToDictionary(n => n, _ => i++);
            var connections = new List<SerializedConnection>();
            foreach (var node in nodes)
            {
                var sourceId = nodeIndexMap.GetValueOrDefault(node, -1);
                connections.AddRange(node.Children.Select(
                        child => nodeIndexMap.GetValueOrDefault(child, -1))
                    .Select(targetId => new SerializedConnection { SourceId = sourceId, TargetId = targetId }));
            }

            return connections.Count > 0 ? connections : null;
        }

        #endregion
    }
}