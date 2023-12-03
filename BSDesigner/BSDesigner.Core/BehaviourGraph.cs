using System;
using System.Collections.Generic;
using System.Linq;

namespace BSDesigner.Core
{
    /// <summary>
    /// Behaviour system formed by nodes connected to each other forming a directed graph.
    /// </summary>
    public abstract class BehaviourGraph : BehaviourEngine
    {
        /// <summary>
        /// A collection with all the nodes in this graph.
        /// </summary>
        public IReadOnlyCollection<Node> Nodes => _nodes;

        private readonly HashSet<Node> _nodes = new HashSet<Node>();

        /// <summary>
        /// The type of nodes that this graph supports
        /// </summary>
        public abstract Type NodeType { get; }

        /// <summary>
        /// Add a new node to the graph.
        /// </summary>
        /// <param name="node">The node added.</param>
        /// <exception cref="ArgumentException">Thrown if node already belongs to a graph or it's not compatible with this graph.</exception>
        public void AddNode(Node node)
        {
            if (node.Graph != null)
                throw new ArgumentException($"ADD NODE ERROR: Can't add {nameof(node)} to the graph if already belongs to a graph.");

            if (!NodeType.IsInstanceOfType(node))
                throw new ArgumentException($"ADD NODE ERROR: An instance of type {node.GetType()} cannot be added, this graph only handles nodes of types derived from {NodeType}");

            if(!node.GraphType.IsInstanceOfType(this))
                throw new ArgumentException($"ADD NODE ERROR: An instance of type {node.GetType()} cannot be added, this node can only belongs to a graph of types derived from {node.GraphType}");

            _nodes.Add(node);
            node.Graph = this;
        }

        /// <summary>
        /// Remove a node from the graph.
        /// </summary>
        /// <param name="node">The node removed.</param>
        public void RemoveNode(Node node)
        {
            if (node.Parents.Count > 0 || node.Children.Count > 0)
                throw new ArgumentException($"REMOVE ERROR: Can't remove a node with connections. Use {nameof(DisconnectAndRemove)} to delete the connections before remove the node.");

            _nodes.Remove(node);
            node.Graph = null;
        }

        public void DisconnectAndRemove(Node node)
        {
            foreach (var other in node.InternalChildList)
            {
                other.InternalParentList.Remove(node);
            }

            foreach (var other in node.InternalParentList)
            {
                other.InternalChildList.Remove(node);
            }

            node.InternalChildList.Clear();
            node.InternalParentList.Clear();
            _nodes.Remove(node);
            node.Graph = null;
        }

        /// <summary>
        /// Create a new connection from <paramref name="source"/> to <paramref name="target"/>.
        /// Value in <paramref name="childIndex"/> will be the index of <paramref name="target"/> in <paramref name="source"/>'s child list and
        /// value <paramref name="parentIndex"/> will be the index of <paramref name="source"/> in <paramref name="target"/>'s parent list.
        /// If index value is -1 (default), the element will be added at the end of the list.
        /// </summary>
        /// <param name="source">The source node of the connection</param>
        /// <param name="target">The target node of the connection.</param>
        /// <param name="childIndex">The index of target node in source's child list.</param>
        /// <param name="parentIndex">The index of source node in target's parent list.</param>
        /// <exception cref="ArgumentException">Thrown if any of the arguments are not valid.</exception>
        public void ConnectNodes(Node source, Node target, int childIndex = -1, int parentIndex = -1)
        {
            if (source == null)
                throw new ArgumentNullException($"CONNECTION ERROR: {nameof(source)} is null reference");

            if (target == null)
                throw new ArgumentNullException($"CONNECTION ERROR: {nameof(target)} is null reference");

            if (source.Graph != this)
                throw new ArgumentException($"CONNECTION ERROR: {nameof(source)} is not in the graph.");

            if (target.Graph != this)
                throw new ArgumentException($"CONNECTION ERROR: {nameof(target)} is not in the graph.");

            if (!source.ChildType.IsInstanceOfType(target))
                throw new ArgumentException($"CONNECTION ERROR: Source node child type({source.GetType()}) can handle target's type ({target.GetType()}) as a child. It should be {source.ChildType}.");

            if (source.MaxOutputConnections != -1 && source.Children.Count >= source.MaxOutputConnections)
                throw new ArgumentException($"CONNECTION ERROR: Maximum child count reached in {nameof(source)}");

            if (target.MaxInputConnections != -1 && target.Parents.Count >= target.MaxInputConnections)
                throw new ArgumentException($"CONNECTION ERROR: Maximum parent count reached in {nameof(target)}");

            source.InternalChildList.Insert(childIndex == -1 ? source.InternalChildList.Count : childIndex, target);
            target.InternalParentList.Insert(parentIndex == -1 ? target.InternalParentList.Count : parentIndex, source);
        }

        /// <summary>
        /// Remove a connection from
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <exception cref="Exception"></exception>
        public void Disconnect(Node source, Node target)
        {
            var childIndex = source.InternalChildList.IndexOf(target);
            var parentIndex = target.InternalParentList.IndexOf(source);

            if (childIndex == -1 || parentIndex == -1 || source.InternalChildList[childIndex] != target || target.InternalParentList[parentIndex] != source)
                throw new Exception("Cant remove a connection if the indexes don't match both source and target nodes");

            source.InternalChildList.RemoveAt(childIndex);
            target.InternalParentList.RemoveAt(parentIndex);
        }

        /// <summary>
        /// Remove the output connection from <paramref name="source"/> in <paramref name="childIndex"/> position.
        /// </summary>
        /// <param name="source">The source node of the connection.</param>
        /// <param name="childIndex">The child index</param>
        /// <exception cref="ArgumentException">Thrown if the index is out of bounds.</exception>
        public void DisconnectChild(Node source, int childIndex)
        {
            if (childIndex < 0 || childIndex >= source.InternalChildList.Count)
                throw new ArgumentException("ERROR: Index is out of bounds");

            var target = source.InternalChildList[childIndex];
            var parentIndex = target.InternalParentList.IndexOf(source);

            source.InternalChildList.RemoveAt(childIndex);
            target.InternalParentList.RemoveAt(parentIndex);
        }

        /// <summary>
        /// Remove the input connection from <paramref name="target"/> in <paramref name="parentIndex"/> position.
        /// </summary>
        /// <param name="target">The target node of the connection.</param>
        /// <param name="parentIndex">The parent index</param>
        /// <exception cref="ArgumentException">Thrown if the index is out of bounds.</exception>
        public void DisconnectParent(Node target, int parentIndex)
        {
            if (parentIndex < 0 || parentIndex >= target.InternalParentList.Count)
                throw new ArgumentException($"ERROR: Index ({parentIndex}) is out of bounds (Count: {target.InternalParentList.Count})");

            var source = target.InternalParentList[parentIndex];
            var childIndex = source.InternalChildList.IndexOf(target);

            target.InternalParentList.RemoveAt(parentIndex);
            source.InternalChildList.RemoveAt(childIndex);
        }

        /// <summary>
        /// Returns if graph has a connection path between <paramref name="source"/> and <paramref name="target"/>.
        /// </summary>
        /// <param name="source">The source node.</param>
        /// <param name="target">The target node.</param>
        /// <returns>True if a path between the nodes exists.</returns>
        public bool AreNodesConnected(Node source, Node target)
        {
            var unvisitedNodes = new HashSet<Node>();
            var visitedNodes = new HashSet<Node>();
            unvisitedNodes.Add(source);
            while (unvisitedNodes.Count > 0)
            {
                var n = unvisitedNodes.First();
                unvisitedNodes.Remove(n);
                visitedNodes.Add(n);
                foreach (var child in n.Children)
                {
                    if (child == target)
                        return true;
                    if (!visitedNodes.Contains(child))
                        unvisitedNodes.Add(child);
                }
            }
            return false;
        }

        /// <summary>
        /// Gets a map that allows you to search for nodes by name
        /// </summary>
        /// <param name="ignoreRepeatedNames">True if nodes with repeated names should be ignored.</param>
        /// <returns>A dictionary in which the key is a string with the name of the node.</returns>
        public Dictionary<string, Node> GetNodeMap(bool ignoreRepeatedNames = false)
        {
            var nodeMap = new Dictionary<string, Node>();
            foreach (var node in Nodes)
            {
                if (string.IsNullOrEmpty(node.Name)) continue;

                if (!nodeMap.TryAdd(node.Name, node) && !ignoreRepeatedNames)
                    throw new ArgumentException("Error: This graph contains nodes with the same name.");
            }
            return nodeMap;
        }

        /// <summary>
        /// Create a default instance of <typeparamref name="T"/> and add it to the graph.
        /// Is used from the derived types of the graph to create their custom methods.
        /// </summary>
        /// <typeparam name="T">The type of the created node.</typeparam>
        /// <returns>The created node</returns>
        protected T CreateNode<T>() where T : Node, new()
        {
            var node = new T();
            AddNode(node);
            return node;
        }
    }
}