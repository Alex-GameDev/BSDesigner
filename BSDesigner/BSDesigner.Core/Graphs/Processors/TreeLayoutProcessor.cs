using BSDesigner.Core;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace BSDesigner.Core.Graphs.Processors
{
    /// <summary>
    /// Layout processor that creates a tree layout
    /// </summary>
    public class TreeLayoutProcessor : LayoutProcessor
    {
        private List<float> levelDistMap = new List<float>();

        /// <summary>
        /// Create a new tree layout processor
        /// </summary>
        /// <param name="offset">The distance between nodes.</param>
        public TreeLayoutProcessor(Vector2 offset) :base(offset)
        {
        }

        protected override void ComputeLayout(BehaviourGraph graph)
        {
            levelDistMap.Clear();
            foreach (var node in graph.Nodes)
            {
                if(node.Parents.Count == 0)
                {
                    ProcessTreeNode(node, 0, 0);
                }
            }
        }

        private float ProcessTreeNode(Node node, int currentDeep, float targetXPos)
        {
            var x = GetTreeNodeXPosition(node, currentDeep, targetXPos);

            node.Position = new Vector2(x, currentDeep) * NodeOffset;
            return x;
        }

        private float GetTreeNodeXPosition(Node node, int currentDeep, float targetXPos)
        {
            if (levelDistMap.Count >= currentDeep) levelDistMap.Add(targetXPos - 1);

            targetXPos = MathF.Max(levelDistMap[currentDeep] + 1, targetXPos);

            if (node.Children.Count == 0)
            {
                levelDistMap[currentDeep] = targetXPos;
            }
            else
            {
                levelDistMap[currentDeep] = GetTreeBranchXPosition(node, currentDeep, targetXPos);
            }

            return levelDistMap[currentDeep];
        }

        private float GetTreeBranchXPosition(Node branchNode, int currentDeep, float targetXPos)
        {
            float firstX = 0f;
            float lastX = 0f;
            for (int i = 0; i < branchNode.Children.Count; i++)
            {
                Node child = branchNode.Children[i];
                float childOffset = i - (branchNode.Children.Count - 1f) / 2f;
                float targetChildXPos = targetXPos + childOffset;
                float computedChildXPos = ProcessTreeNode(child, currentDeep + 1, targetChildXPos);

                targetXPos = Math.Max(computedChildXPos - childOffset, targetXPos);

                if (i == 0) firstX = computedChildXPos;
                if (i == branchNode.Children.Count - 1) lastX = computedChildXPos;
            }
            return (firstX + lastX) * .5f;
        }
    }
}
