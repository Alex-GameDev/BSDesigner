using BSDesigner.Core.Exceptions;

namespace BSDesigner.BehaviourTrees
{
    public abstract class DecoratorNode : BtNode
    {
        public sealed override int MaxOutputConnections => 1;

        protected BtNode ChildNode
        {
            get
            {
                if (_cachedChildNode == null)
                {
                    if (Children.Count == 0) throw new MissingConnectionException("Can't find the child node if the children list is empty");
                    _cachedChildNode = (BtNode)Children[0];
                }
                return _cachedChildNode;
            }
        }

        private BtNode? _cachedChildNode;
    }
}
