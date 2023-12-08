using System;
using System.Linq;
using System.Xml.Linq;
using BSDesigner.Core;
using BSDesigner.Core.Exceptions;
using BSDesigner.Core.Tasks;

namespace BSDesigner.BehaviourTrees
{
    public class BehaviourTree : BehaviourGraph
    {
        public override Type NodeType => typeof(BtNode);

        protected BtNode RootNode
        {
            get
            {
                if (_cachedRootNode == null)
                {
                    if (Nodes.Count == 0) throw new EmptyGraphException("Can't find the root node if graph is empty");

                    _cachedRootNode = (BtNode)Nodes.First();
                }
                return _cachedRootNode;
            }
        }

        private BtNode? _cachedRootNode;

        /// <summary>
        /// Specify a new root node.
        /// </summary>
        /// <param name="node">The new root node of the behaviour tree.</param>
        public void ChangeRootNode(BtNode node) => ReorderNode(node, 0);

        /// <summary>
        /// Create a new decorator node of type <typeparamref name="T"/>  in this <see cref="BehaviourTree"/> that have <paramref name="child"/> as a child.
        /// </summary>
        /// <typeparam name="T">The type of decorator.</typeparam>
        /// <param name="child">The child BT Node.</param>
        /// <returns>The <typeparamref name="T"/> created.</returns>
        public T CreateDecorator<T>(BtNode child) where T : DecoratorNode, new()
        {
            var node = CreateNode<T>();
            ConnectNodes(node, child);
            return node;
        }

        /// <summary>
        /// Create a new <see cref="LeafBtNode"/> in this <see cref="BehaviourTree"/> that executes the action specified in <paramref name="action"/>.
        /// </summary>
        /// <returns>The <see cref="LeafBtNode"/> created.</returns>
        public T CreateLeafNode<T>() where T : LeafBtNode, new() => CreateNode<T>();

        /// <summary>
        /// Create a new <see cref="ActionBtNode"/> in this <see cref="BehaviourTree"/> that executes the action specified in <paramref name="action"/>.
        /// </summary>
        /// <param name="action">The action that the leaf node executes.</param>
        /// <returns>The <see cref="ActionBtNode"/> created.</returns>
        public ActionBtNode CreateActionNode(ActionTask action)
        {
            var node = CreateNode<ActionBtNode>();
            node.Action = action;
            return node;
        }

        /// <summary>
        /// Create a new <see cref="PerceptionBtNode"/> in this <see cref="BehaviourTree"/> that executes the action specified in <paramref name="action"/>.
        /// </summary>
        /// <param name="perception">The perception that the leaf node executes.</param>
        /// <returns>The <see cref="PerceptionBtNode"/> created.</returns>
        public PerceptionBtNode CreatePerceptionNode(PerceptionTask perception)
        {
            var node = CreateNode<PerceptionBtNode>();
            node.Perception = perception;
            return node;
        }

        protected override void OnStarted()
        {
            RootNode.Start();
        }

        protected override void OnUpdated()
        {
            RootNode.Update();
            Status = RootNode.Status;
        }

        protected override void OnStopped()
        {
            RootNode.Stop();
        }

        protected override void OnPaused()
        {
            RootNode.Pause();
        }
    }
}
