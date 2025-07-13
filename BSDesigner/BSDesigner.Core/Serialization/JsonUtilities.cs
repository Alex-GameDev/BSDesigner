using BSDesigner.Core.Graphs;
using BSDesigner.Core.Serialization.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace BSDesigner.Core.Serialization
{
    public static class JsonUtilities
    {
        /// <summary>
        /// Serialize a behaviour graph to a string in json format
        /// </summary>
        /// <param name="graph">Serialized graph</param>
        /// <returns>Json generated</returns>
        public static string Serialize(BehaviourGraph graph)
        {
            var serializableData = ConvertToDto(graph);
            var settings = CreateSerializerSettings();
            return JsonConvert.SerializeObject(serializableData, settings);
        }

        /// <summary>
        /// Serialize a node to a string in json format
        /// </summary>
        /// <param name="node">Serialized node</param>
        /// <returns>Json generated</returns>
        public static string SerializeNode(Node node)
        {
            var settings = CreateSerializerSettings();
            return JsonConvert.SerializeObject(node, settings);
        }

        /// <summary>
        /// Serialize a behaviour graph to a string in json format
        /// </summary>
        /// <param name="json">Serialized graph string</param>
        /// <returns>Json generated</returns>
        public static BehaviourGraph? Deserialize(string json)
        {
            var settings = CreateSerializerSettings();
            var data = JsonConvert.DeserializeObject<BehaviourGraphSerializableData>(json, settings);
            
            if (data == null) return null;
            return ConvertToGraph(data);
        }

        /// <summary>
        /// Serialize a behaviour graph to a string in json format
        /// </summary>
        /// <param name="json">Serialized graph string</param>
        /// <returns>Json generated</returns>
        public static T? Deserialize<T>(string json) where T: BehaviourGraph
        {
            var settings = CreateSerializerSettings();
            var data = JsonConvert.DeserializeObject<BehaviourGraphSerializableData>(json, settings);

            if (data == null) return null;
            return (T?) ConvertToGraph(data);
        }

        /// <summary>
        /// Serialize a node to a string in json format
        /// </summary>
        /// <param name="json">Serialized graph string</param>
        /// <returns>Json generated</returns>
        public static Node? DeserializeNode(string json)
        {
            var settings = CreateSerializerSettings();
            return JsonConvert.DeserializeObject<Node>(json, settings);
        }

        /// <summary>
        /// Serialize a node to a string in json format
        /// </summary>
        /// <param name="json">Serialized node string</param>
        /// <returns>Json generated</returns>
        public static T? DeserializeNode<T>(string json) where T: Node
        {
            var settings = CreateSerializerSettings();
            return JsonConvert.DeserializeObject<T>(json, settings);
        }

        private static JsonSerializerSettings CreateSerializerSettings()
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new BSDContractResolver(),
                TypeNameHandling = TypeNameHandling.Auto,
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            };
            return settings;
        }

        #region Model conversion

        private static BehaviourGraphSerializableData ConvertToDto(BehaviourGraph graph)
        {
            var dto = new BehaviourGraphSerializableData
            {
                Graph = graph,
                Nodes = graph.Nodes,
                Connections = GetConnections(graph.Nodes),
            };
            return dto;
        }

        private static BehaviourGraph ConvertToGraph(BehaviourGraphSerializableData dto)
        {
            var graph = dto.Graph;
            foreach (var node in dto.Nodes)
            {
                graph.AddNode(node);
            }

            foreach (var connection in dto.Connections)
            {
                var source = graph.Nodes[connection.SourceId];
                var target = graph.Nodes[connection.TargetId];
                graph.ConnectNodes(source, target);
            }

            return graph;
        }

        private static List<ConnectionSerializableData> GetConnections(IEnumerable<Node> nodes)
        {
            var i = 0;
            var nodeIndexMap = nodes.ToDictionary(n => n, _ => i++);
            var connections = new List<ConnectionSerializableData>();
            foreach (var node in nodes)
            {
                var sourceId = nodeIndexMap.GetValueOrDefault(node, -1);
                foreach (var child in node.Children)
                {
                    var targetId = nodeIndexMap.GetValueOrDefault(child, -1);
                    connections.Add(new ConnectionSerializableData { SourceId = sourceId, TargetId = targetId });
                }
            }
            return connections;
        }

        #endregion
    }
}
