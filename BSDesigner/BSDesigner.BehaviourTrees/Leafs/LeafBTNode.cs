using BSDesigner.Core;
using BSDesigner.Core.Exceptions;

namespace BSDesigner.BehaviourTrees
{
    /// <summary>
    /// BTNode type that has no children attached.
    /// </summary>
    public abstract class LeafBtNode : BtNode
    {
        public sealed override int MaxOutputConnections => 0; 
    }
}
