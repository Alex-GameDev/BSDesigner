
using BSDesigner.Core;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace BSDesigner.Core.Graphs.Processors
{
    /// <summary>
    /// Layout processor for layered/directed acyclic graphs.
    /// </summary>
    public class LayeredLayoutProcessor : LayoutProcessor
    {

        Dictionary<Node, int> nodeLevelMap = new Dictionary<Node, int>();

        public LayeredLayoutProcessor(Vector2 nodeOffset) : base(nodeOffset)
        {
        }

        protected override void ComputeLayout(BehaviourGraph graph)
        {
            nodeLevelMap.Clear();

            foreach (var node in graph.Nodes)
            {
                if (!nodeLevelMap.ContainsKey(node))
                {
                    ComputeLevel(node);
                }
            }

            var list = nodeLevelMap.ToList();

            var maxLevel = list.Max(kvp => kvp.Value);

            for (int i = 0; i <= maxLevel; i++)
            {
                List<Node> nodes = list.FindAll(kvp => kvp.Value == i).Select(kvp => kvp.Key).ToList();
                var dist = i;
                ComputePositions(nodes, dist);
            }
        }

        private int ComputeLevel(Node node)
        {
            int currentLevel = 0;

            for (int i = 0; i < node.Children.Count; i++)
            {
                Node child = node.Children[i];
                int childValue = nodeLevelMap.TryGetValue(child, out int level) ? level : ComputeLevel(child);
                if (childValue + 1 > currentLevel) currentLevel = childValue + 1;
            }
            nodeLevelMap[node] = currentLevel;
            return currentLevel;
        }

        private void ComputePositions(List<Node> nodes, int level)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                if (level == 0)
                {
                    nodes[i].Position = new Vector2(level, i) * NodeOffset;
                }
                else
                {
                    nodes[i].Position = new Vector2(level, nodes[i].Children.Average(child => child.Position.Y));
                }
            }

            if (level != 0)
            {
                var midPos = nodes.Average(n => n.Position.Y);
                if (nodes.Count == 1)
                {
                    var dist = level * NodeOffset.X;
                    nodes[0].Position = new Vector2(dist, midPos);
                }
                else
                {
                    nodes = nodes.OrderBy(n => n.Position.Y).ToList();
                    var dist = level * NodeOffset.X;
                    var midCount = (nodes.Count - 1) / 2f;

                    for (int i = 0; i < nodes.Count; i++)
                    {
                        nodes[i].Position = new Vector2(dist, midPos) + NodeOffset * new Vector2(0, i - midCount);
                    }
                }
            }
        }
    }
}
