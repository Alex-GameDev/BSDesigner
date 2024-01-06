using BSDesigner.Core;
using BSDesigner.Core.Exceptions;

namespace BSDesigner.BehaviourTrees
{
    /// <summary>
    /// LeafNode type that executes an action and return its results.
    /// </summary>
    public class ActionBtNode : LeafBtNode
    {
        /// <summary>
        /// The action that this node will use to get its status.
        /// </summary>
        public ActionTask? Action;

        /// <summary>
        /// Get the task associated with this leaf node.
        /// </summary>
        /// <returns>The task.</returns>
        /// <exception cref="MissingTaskException">Thrown if the action is null.</exception>
        protected override Task GetTask()
        {
            if (Action == null)
                throw new MissingTaskException("Leaf nodes need a task to work.");

            return Action;
        }

        /// <summary>
        /// <inheritdoc/>
        /// Updates the action execution.
        /// </summary>
        /// <returns>The result of the action</returns>
        /// <exception cref="MissingTaskException">Thrown if the action is null.</exception>
        protected override Status UpdateStatus()
        {
            if (Action == null)
                throw new MissingTaskException("Leaf nodes need a task to work.");

            var actionResult = Action.Update();
            Status = actionResult;
            return Status;
        }

        public override void SetContext(ExecutionContext context) => Action?.SetContext(context);
    }
}