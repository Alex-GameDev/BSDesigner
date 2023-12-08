using System.Collections.Generic;
using System.Linq;
using BSDesigner.Core.Exceptions;

namespace BSDesigner.BehaviourTrees
{
    /// <summary>
    /// BtNode subtype that has multiple children and executes them according to certain conditions.
    /// </summary>
    public abstract class CompositeNode : BtNode
    {
        public sealed override int MaxOutputConnections => -1;

        private List<BtNode>? _cachedChildren;

        /// <summary>
        /// Get the bt child branch at the given index
        /// </summary>
        /// <param name="index">The index of the branch.</param>
        /// <returns>The bt child node at the given index</returns>
        /// <exception cref="MissingConnectionException">If the index is out of bounds in the child list.</exception>
        protected BtNode GetBranch(int index)
        {
            _cachedChildren ??= Children.Select(n => (BtNode)n).ToList();

            if (index < 0 || index >= _cachedChildren.Count)
                throw new MissingConnectionException($"Cannot get the child branch at index {index}");

            return _cachedChildren[index];
        }
    }
}
