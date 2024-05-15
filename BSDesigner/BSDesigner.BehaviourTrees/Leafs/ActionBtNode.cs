using BSDesigner.Core;
using BSDesigner.Core.Actions;
using BSDesigner.Core.Exceptions;
using System.Collections.Generic;

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
        /// Starts the action execution.
        /// </summary>
        /// <exception cref="MissingTaskException">If the action is null</exception>
        protected override void OnNodeStarted()
        {
            if (Action == null)
                throw new MissingTaskException("Action leaf nodes need an task to work.");

            Action.Start();
        }

        /// <summary>
        /// <inheritdoc/>
        /// Stops the action execution.
        /// </summary>
        /// <exception cref="MissingTaskException"></exception>
        protected override void OnNodeStopped()
        {
            if (Action == null)
                throw new MissingTaskException("Action leaf nodes need an task to work.");

            Action.Stop();
        }

        /// <summary>
        /// <inheritdoc/>
        /// Stops the action execution.
        /// </summary>
        /// <exception cref="MissingTaskException"></exception>
        protected override void OnNodePaused()
        {
            if (Action == null)
                throw new MissingTaskException("Action leaf nodes need an task to work.");

            Action.Pause();
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

        public override void SetContext(ExecutionContext context)
        {
            Action?.SetContext(context);
        }
    }
}