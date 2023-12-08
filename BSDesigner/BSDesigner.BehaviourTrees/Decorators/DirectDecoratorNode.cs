using BSDesigner.Core;

namespace BSDesigner.BehaviourTrees
{
    /// <summary>
    /// BTNode that alters the result returned by its child node or its execution.
    /// </summary>
    public abstract class DirectDecoratorNode : DecoratorNode
    {
        /// <summary>
        /// <inheritdoc/>
        /// Starts the execution of its child.
        /// </summary>
        protected override void OnNodeStarted() => ChildNode.Start();

        /// <summary>
        /// <inheritdoc/>
        /// Stops the execution of its child.
        /// </summary>
        protected override void OnNodeStopped() => ChildNode.Stop();

        /// <summary>
        /// <inheritdoc/>
        /// Pauses the child node.
        /// </summary>
        protected override void OnNodePaused() => ChildNode.Pause();

        /// <summary>
        /// <inheritdoc/>
        /// Updates the execution of its child and returns the value modified.
        /// </summary>
        protected override Status UpdateStatus()
        {
            ChildNode.Update();
            var childStatus = ChildNode.Status;
            return ModifyStatus(childStatus);
        }

        /// <summary>
        /// Implement this method to modify the children status.
        /// </summary>
        /// <param name="childStatus">The child current status.</param>
        /// <returns>The child status modified.</returns>
        protected abstract Status ModifyStatus(Status childStatus);
    }
}
