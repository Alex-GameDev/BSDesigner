using BSDesigner.Core;

namespace BSDesigner.BehaviourTrees
{
    /// <summary>
    /// Composite node that selects one of its branch to execute it.
    /// </summary>
    public abstract class BranchSelectionNode : CompositeNode
    {
        private int _selectedBranch;
        /// <summary>
        /// <inheritdoc/>
        /// Select a branch and starts it.
        /// </summary>
        protected sealed override void OnNodeStarted()
        {
            _selectedBranch = SelectBranchIndex();
            GetBranch(_selectedBranch).Start();
        }

        /// <summary>
        /// <inheritdoc/>
        /// Returns the status of its selected branch.
        /// </summary>
        /// <returns><inheritdoc/></returns>
        protected sealed override Status UpdateStatus()
        {
            var selectedNode = GetBranch(_selectedBranch);
            selectedNode.Update();
            return selectedNode.Status;
        }

        /// <summary>
        /// <inheritdoc/>
        /// Stops the selected branch node.
        /// </summary>
        protected sealed override void OnNodeStopped() => GetBranch(_selectedBranch).Stop();

        /// <summary>
        /// <inheritdoc/>
        /// Pauses the selected branch node.
        /// </summary>
        protected sealed override void OnNodePaused() => GetBranch(_selectedBranch).Pause();

        /// <summary>
        /// Override this method to define how to select the branch that will be executed.
        /// </summary>
        /// <returns>The index of the child that will be executed.</returns>
        protected abstract int SelectBranchIndex();
    }
}