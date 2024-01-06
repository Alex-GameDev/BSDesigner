using System;
using System.Numerics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Linq;
using System.Linq;

namespace BSDesigner.Core
{
    /// <summary>
    /// The main element of the behaviour graphs.
    /// </summary>
    public abstract class Node
    {
        /// <summary>
        /// The name of the node used to identify it.
        /// </summary>
        [DefaultValue("")]
        public string Name = string.Empty;

        /// <summary>
        /// The description of the node behaviour or purpose
        /// </summary>
        [DefaultValue("")]
        public string Description = string.Empty;

        /// <summary>
        /// The position of the node (for editor tools)
        /// </summary>
        public Vector2 Position = Vector2.Zero;

        internal readonly List<Node> InternalParentList = new List<Node>();

        internal readonly List<Node> InternalChildList = new List<Node>();

        /// <summary>
        /// The graph to which this node belongs.
        /// </summary>
        public BehaviourGraph? Graph { get; internal set; }

        /// <summary>
        /// A collection with all the nodes connected to this one as output.
        /// </summary>
        public IReadOnlyList<Node> Children => InternalChildList;

        /// <summary>
        /// A collection with all the nodes connected to this one as input.
        /// </summary>
        public IReadOnlyList<Node> Parents => InternalParentList;

        /// <summary>
        /// The type of graph that this node can belong.
        /// </summary>
        public abstract Type GraphType { get; }

        /// <summary>
        /// The type of nodes that this node can handle as a child(s).
        /// </summary>
        public abstract Type ChildType { get; }

        /// <summary>
        /// Maximum number of elements in <see cref="Parents"/>.
        /// </summary>
        public abstract int MaxInputConnections { get; }

        /// <summary>
        /// Maximum number of elements in <see cref="Children"/>.
        /// </summary>
        public abstract int MaxOutputConnections { get; }

        /// <summary>
        /// Checks if there is a connection from <paramref name="otherNode"/> to this node.
        /// </summary>
        /// <param name="otherNode">The source of the connection checked</param>
        /// <returns>True if <paramref name="otherNode"/> is a parent of this node.</returns>
        public bool IsChildOf(Node otherNode)
        {
            return InternalParentList.Contains(otherNode);
        }

        /// <summary>
        /// Checks if there is a connection from this node to <paramref name="otherNode"/>.
        /// </summary>
        /// <param name="otherNode">The target of the connection checked</param>
        /// <returns>True if <paramref name="otherNode"/> is a child of this node.</returns>
        public bool IsParentOf(Node otherNode)
        {
            return InternalChildList.Contains(otherNode);
        }

        /// <summary>
        /// Override this method to define how it uses the execution context.
        /// <param name="context">The context provided.</param>
        /// </summary>
        public virtual void SetContext(ExecutionContext context)
        {
        }

        /// <summary>
        /// Get all the nodes that can reach this through its children.
        /// </summary>
        /// <returns>A set of nodes connected with this by its children.</returns>
        public HashSet<Node> GetConnectedNodesAsParent()
        {
            var visitedNodes = new HashSet<Node>();
            var unvisitedNodes = new HashSet<Node>{ this };
            while (unvisitedNodes.Count > 0)
            {
                var node = unvisitedNodes.First();
                unvisitedNodes.Remove(node);
                visitedNodes.Add(node);

                foreach (var connectedElement in node.Parents)
                {
                    if (!visitedNodes.Contains(connectedElement))
                    {
                        unvisitedNodes.Add(connectedElement);
                    }
                }
            }

            return visitedNodes;
        }

        /// <summary>
        /// Get all the nodes that this node can reach through its children.
        /// </summary>
        /// <returns>A set of nodes connected with this by its parents.</returns>
        public HashSet<Node> GetConnectedNodesAsChild()
        {
            var visitedNodes = new HashSet<Node>();
            var unvisitedNodes = new HashSet<Node> { this };
            while (unvisitedNodes.Count > 0)
            {
                var node = unvisitedNodes.First();
                unvisitedNodes.Remove(node);
                visitedNodes.Add(node);

                foreach (var connectedElement in node.Children)
                {
                    if (!visitedNodes.Contains(connectedElement))
                    {
                        unvisitedNodes.Add(connectedElement);
                    }
                }
            }

            return visitedNodes;
        }
    }
}
