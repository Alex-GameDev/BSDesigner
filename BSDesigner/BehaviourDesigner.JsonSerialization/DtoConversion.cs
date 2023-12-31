using System.Collections.Generic;
using System.Linq;
using BSDesigner.Core;

namespace BSDesigner.JsonSerialization
{
    /// <summary>
    /// utility class to convert from business classes to dto.
    /// </summary>
    public static class DtoConversion
    {
        /// <summary>
        /// Create a new Engine dto from a behaviour engine.
        /// </summary>
        /// <param name="engine">The engine provided</param>
        /// <returns>The dto generated</returns>
        public static BehaviourEngineDto FromEngineToDto(BehaviourEngine engine)
        {
            var dto = new BehaviourEngineDto
            {
                Engine = engine
            };

            if (engine is BehaviourGraph graph)
            {
                dto.Nodes = graph.Nodes.ToList();
                dto.Connections = GetConnections(graph.Nodes);
            }
            return dto;
        }

        /// <summary>
        /// Build a behaviour engine using a dto.
        /// </summary>
        /// <param name="dto">The dto provided</param>
        /// <returns>The engine generated</returns>
        public static BehaviourEngine FromDtoToEngine(BehaviourEngineDto dto)
        {
            var engine = dto.Engine;

            if (engine is BehaviourGraph graph)
            {
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
            }
            return engine;
        }

        private static List<ConnectionDto> GetConnections(IEnumerable<Node> nodes)
        {
            var i = 0;
            var nodeIndexMap = nodes.ToDictionary(n => n, _ => i++);
            var connections = new List<ConnectionDto>();
            foreach (var node in nodes)
            {
                var sourceId = nodeIndexMap.GetValueOrDefault(node, -1);
                foreach (var child in node.Children)
                {
                    var targetId = nodeIndexMap.GetValueOrDefault(child, -1);
                    connections.Add(new ConnectionDto { SourceId = sourceId, TargetId = targetId });
                }
            }
            return connections;
        }
    }
}