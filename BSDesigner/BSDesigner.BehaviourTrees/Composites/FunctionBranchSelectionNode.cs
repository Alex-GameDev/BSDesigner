using System;

namespace BSDesigner.BehaviourTrees
{
    /// <summary>
    /// Branch selector node that select the branch by a function that returns an index
    /// </summary>
    public class FunctionBranchSelectionNode : BranchSelectionNode
    {
        /// <summary>
        /// The function used to get the branch index. The result will be clamped between 0 and child count.
        /// </summary>
        public Func<int>? NodeIndexFunction;

        /// <summary>
        /// <inheritdoc/>
        /// Select the branch index using a function that return an integer.
        /// </summary>
        /// <returns><inheritdoc/></returns>
        protected override int SelectBranchIndex() => NodeIndexFunction?.Invoke() ?? 0;
    }
}