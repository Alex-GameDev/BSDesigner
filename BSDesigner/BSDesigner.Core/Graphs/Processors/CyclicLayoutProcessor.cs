using BSDesigner.Core;
using BSDesigner.Core.Utils;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace BSDesigner.Core.Graphs.Processors
{
    internal class CyclicLayoutProcessor : LayoutProcessor
    {
        Random random = new Random();

        public CyclicLayoutProcessor(Vector2 offset) : base(offset)
        {
        }

        protected override void ComputeLayout(BehaviourGraph graph)
        {
            Dictionary<Node, Vector2> disPositionMap = new Dictionary<Node, Vector2>();
            Vector2 area = NodeOffset * MathF.Sqrt(graph.Nodes.Count);

            int iterations = 100;

            float force = MathF.Sqrt((area.X * area.Y) / graph.Nodes.Count);

            for (int i = 0; i < graph.Nodes.Count; i++)
            {
                var pos = area * new Vector2((float)random.NextDouble(), (float)random.NextDouble());
                graph.Nodes[i].Position = pos;
                disPositionMap[graph.Nodes[i]] = new Vector2(0f, 0f);
            }

            var maxdist = 100f;
            for (int i = 0; i < iterations; i++)
            {
                for (int j = 0; j < graph.Nodes.Count; j++)
                {
                    var node = graph.Nodes[j];
                    for (int k = 0; k < graph.Nodes.Count; k++)
                    {
                        var otherNode = graph.Nodes[k];
                        if (node != otherNode)
                        {
                            var dist = node.Position - otherNode.Position;
                            var magnitude = dist.Magnitude();
                            disPositionMap[node] += dist.Normalize() * GetRepulsionForce(magnitude, force);
                        }
                    }
                }

                for (int j = 0; j < graph.Nodes.Count; j++)
                {
                    var node = graph.Nodes[j];
                    for (int k = 0; k < node.Parents.Count; k++)
                    {
                        var otherNode = node.Parents[k];

                        if (otherNode == null) continue;

                        var dist = node.Position - otherNode.Position;
                        var magnitude = dist.Magnitude();
                        disPositionMap[node] -= dist.Normalize() * GetAttractionForce(magnitude, force);
                        disPositionMap[otherNode] += dist.Normalize() * GetAttractionForce(magnitude, force);
                    }

                    for (int k = 0; k < node.Children.Count; k++)
                    {
                        var otherNode = node.Children[k];

                        if (otherNode == null) continue;

                        var dist = node.Position - otherNode.Position;
                        var magnitude = dist.Magnitude();
                        disPositionMap[node] -= dist.Normalize() * GetAttractionForce(magnitude, force);
                        disPositionMap[otherNode] += dist.Normalize() * GetAttractionForce(magnitude, force);
                    }
                }

                maxdist *= 0.95f;
                for (int j = 0; j < graph.Nodes.Count; j++)
                {
                    var node = graph.Nodes[j];
                    var rawDisp = disPositionMap[node];
                    var magnitude = rawDisp.Magnitude();

                    if (magnitude > maxdist) rawDisp *= maxdist / magnitude;

                    node.Position += rawDisp;
                    disPositionMap[node] = new Vector2(0f, 0f);
                }
            }
        }

        float GetAttractionForce(float x, float k)
        {
            return x * x / k;
        }

        float GetRepulsionForce(float x, float k)
        {
            return k * k / x;
        }
    }
}
