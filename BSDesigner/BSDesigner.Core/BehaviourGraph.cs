using System;
using System.Collections.Generic;
using System.Linq;
using BSDesigner.Core.Exceptions;

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
        public IReadOnlyCollection<Node> Nodes => _nodeList;

        private readonly List<Node> _nodeList = new List<Node>();

        /// <summary>
        /// The type of nodes that this graph supports
        /// </summary>
        public abstract Type NodeType { get; }

        /// <summary>
        /// This graph allows loops when connections are created?
        /// </summary>
        public abstract bool CanCreateLoops { get; }

        /// <summary>
        /// Add a new node to the graph.
        /// </summary>
        /// <param name="node">The node added.</param>
        /// <exception cref="ArgumentException">Thrown if node already belongs to a graph or it's not compatible with this graph.</exception>
        public void AddNode(Node node)
        {
            if (node.Graph != null)
                throw new NodeException($"Can't add {nameof(node)} to the graph if already belongs to a graph.");

            if (!NodeType.IsInstanceOfType(node))
                throw new NodeException($"An instance of type {node.GetType()} cannot be added, this graph only handles nodes of types derived from {NodeType}");

            if(!node.GraphType.IsInstanceOfType(this))
                throw new NodeException($"An instance of type {node.GetType()} cannot be added, this node can only belongs to a graph of types derived from {node.GraphType}");

            _nodeList.Add(node);
            node.Graph = this;
        }

        /// <summary>
        /// Remove a node from the graph.
        /// </summary>
        /// <param name="node">The node removed.</param>
        public void RemoveNode(Node node)
        {
            if (node.Parents.Count > 0 || node.Children.Count > 0)
                throw new NodeException($"REMOVE ERROR: Can't remove a node with connections. Use {nameof(DisconnectAndRemove)} to delete the connections before remove the node.");

            _nodeList.Remove(node);
            node.Graph = null;
        }

        /// <summary>
        /// Remove all the connections of <paramref name="node"/> an then remove it from the graph.
        /// </summary>
        /// <param name="node">The node that will be removed.</param>
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
            _nodeList.Remove(node);
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
                throw new ConnectionException($"CONNECTION ERROR: {nameof(source)} is null reference.");

            if (target == null)
                throw new ConnectionException($"CONNECTION ERROR: {nameof(target)} is null reference.");

            if (source == target)
                throw new ConnectionException($"CONNECTION ERROR: {nameof(source)} cannot create a connection with itself.");

            if (source.Graph != this)
                throw new ConnectionException($"CONNECTION ERROR: {nameof(source)} is not in the graph.");

            if (target.Graph != this)
                throw new ConnectionException($"CONNECTION ERROR: {nameof(target)} is not in the graph.");

            if (!source.ChildType.IsInstanceOfType(target))
                throw new ConnectionException($"CONNECTION ERROR: Source node child type({source.GetType()}) can handle target's type ({target.GetType()}) as a child. It should be {source.ChildType}.");

            if (source.MaxOutputConnections != -1 && source.Children.Count >= source.MaxOutputConnections)
                throw new ConnectionException($"CONNECTION ERROR: Maximum child count reached in {nameof(source)}");

            if (target.MaxInputConnections != -1 && target.Parents.Count >= target.MaxInputConnections)
                throw new ConnectionException($"CONNECTION ERROR: Maximum parent count reached in {nameof(target)}");

            if (source.IsParentOf(target))
                throw new ConnectionException($"CONNECTION ERROR: {nameof(source)} and {nameof(target)} are already connected");

            if (!CanCreateLoops && AreNodesConnected(target, source))
                throw new ConnectionException($"CONNECTION ERROR: Cant connect nodes because loops are disabled for this type of graph.");

            source.InternalChildList.Insert(childIndex == -1 ? source.InternalChildList.Count : childIndex, target);
            target.InternalParentList.Insert(parentIndex == -1 ? target.InternalParentList.Count : parentIndex, source);
        }

        /// <summary>
        /// Remove the first found connection from <paramref name="source"/> to <paramref name="target"/>
        /// </summary>
        /// <param name="source">The first node of the connection</param>
        /// <param name="target">The last node of the connection</param>
        /// <exception cref="ConnectionException">If the nodes are not connected.</exception>
        public void Disconnect(Node source, Node target)
        {
            var childIndex = source.InternalChildList.IndexOf(target);
            var parentIndex = target.InternalParentList.IndexOf(source);

            if (childIndex == -1 || parentIndex == -1 || source.InternalChildList[childIndex] != target || target.InternalParentList[parentIndex] != source)
                throw new ConnectionException("Cant remove a connection if the indexes don't match both source and target nodes");

            source.InternalChildList.RemoveAt(childIndex);
            target.InternalParentList.RemoveAt(parentIndex);
        }

        /// <summary>
        /// Remove the output connection from <paramref name="source"/> in <paramref name="childIndex"/> position.
        /// </summary>
        /// <param name="source">The source node of the connection.</param>
        /// <param name="childIndex">The child index</param>
        /// <exception cref="ConnectionException">Thrown if the index is out of bounds.</exception>
        public void DisconnectChild(Node source, int childIndex)
        {
            if (childIndex < 0 || childIndex >= source.InternalChildList.Count)
                throw new ConnectionException("ERROR: Index is out of bounds");

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
        /// <exception cref="ConnectionException">Thrown if the index is out of bounds.</exception>
        public void DisconnectParent(Node target, int parentIndex)
        {
            if (parentIndex < 0 || parentIndex >= target.InternalParentList.Count)
                throw new ConnectionException($"ERROR: Index ({parentIndex}) is out of bounds (Count: {target.InternalParentList.Count})");

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

            unvisitedNodes.Add(target);
            while (unvisitedNodes.Count > 0)
            {
                var n = unvisitedNodes.First();
                unvisitedNodes.Remove(n);
                visitedNodes.Add(n);
                foreach (var parent in n.Parents)
                {
                    if (parent == source)
                        return true;
                    if (!visitedNodes.Contains(parent))
                        unvisitedNodes.Add(parent);
                }
            }
            return false;
        }


        /// <summary>
        /// <inheritdoc/>
        /// Passes the context to all its nodes.
        /// </summary>
        public override void SetContext(ExecutionContext context)
        {
            foreach (var node in Nodes)
            {
                node.SetContext(context);
            }
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

        /// <summary>
        /// Change the index of a node in the graph.
        /// </summary>
        /// <param name="node">The reordered node.</param>
        /// <param name="index">The new index of the node in the node list.</param>
        /// <exception cref="NodeException">If the node cannot be reordered to the given index.</exception>
        protected void ReorderNode(Node node, int index)
        {
            if (node == null)
                throw new NodeException("Cannot reorder a node that is null.");

            if (!Nodes.Contains(node))
                throw new NodeException("Cannot reorder a node that don't belong to this graph.");

            if(index < 0 || index >= Nodes.Count)
                throw new NodeException("Cannot reorder a node to a index that is out of bounds.");

            _nodeList.Remove(node);
            _nodeList.Insert(index, node);
        }
    }
}