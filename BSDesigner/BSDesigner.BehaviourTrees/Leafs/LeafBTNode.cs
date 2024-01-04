using BSDesigner.Core;
using BSDesigner.Core.Exceptions;

namespace BSDesigner.BehaviourTrees
{
    /// <summary>
    /// BTNode type that has no children and computes the status result by a task.
    /// </summary>
    public abstract class LeafBtNode : BtNode
    {
        public sealed override int MaxOutputConnections => 0;

        /// <summary>
        /// Get the task associated with this leaf node.
        /// </summary>
        /// <returns>The task.</returns>
        /// <exception cref="MissingTaskException">Thrown if the action is null.</exception>
        protected abstract Task GetTask();

        /// <summary>
        /// Starts the action execution.
        /// </summary>
        /// <exception cref="MissingTaskException">If the action is null</exception>
        protected override void OnNodeStarted() => GetTask().Start();

        /// <summary>
        /// <inheritdoc/>
        /// Stops the action execution.
        /// </summary>
        /// <exception cref="MissingTaskException"></exception>
        protected override void OnNodeStopped() => GetTask().Stop();

        /// <summary>
        /// <inheritdoc/>
        /// Stops the action execution.
        /// </summary>
        /// <exception cref="MissingTaskException"></exception>
        protected override void OnNodePaused() => GetTask().Pause();
    }
}
