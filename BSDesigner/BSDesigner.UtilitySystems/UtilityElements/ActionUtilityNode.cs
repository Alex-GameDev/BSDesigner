using BSDesigner.Core;
using BSDesigner.Core.Actions;
using System.Collections.Generic;
using BSDesigner.UtilitySystems.UtilityElements;

namespace BSDesigner.UtilitySystems
{
    /// <summary>
    /// Utility node that executes an action when is selected
    /// </summary>
    public class ActionUtilityNode : FactorizedUtilityNode
    {
        /// <summary>
        /// The <see cref="Action"/> that this <see cref="ActionUtilityNode"/> executes when is selected.
        /// </summary>
        public ActionTask? Action;

        /// <summary>
        /// Should the action execute forever keeping the status on running until another element is selected? (This flag has priority to <see cref="FinishSystemOnComplete"/>)
        /// </summary>
        public bool ExecuteInLoop;

        /// <summary>
        /// The utility system should end when the action finish the execution?
        /// </summary>
        public bool FinishSystemOnComplete;

        /// <summary>
        /// <inheritdoc/>
        /// Starts the execution of the action.
        /// </summary>
        protected override void OnElementStarted() => Action?.Start();

        /// <summary>
        /// <inheritdoc/>
        /// Stops the execution of the action.
        /// </summary>
        protected override void OnElementStopped() => Action?.Stop();

        /// <summary>
        /// <inheritdoc/>
        /// Updates the execution of the action.
        /// If the action finish and <see cref="FinishSystemOnComplete"/> is true,
        /// finish the execution of the system with the same result.
        /// </summary>
        protected override Status OnElementUpdated()
        {
            if (Status != Status.Running) return Status;

            var actionResult = Action?.Update() ?? Status.Running;

            if (ExecuteInLoop && actionResult != Status.Running)
            {
                Action?.Stop();
                Action?.Start();
                actionResult = Status.Running;
            }

            if (FinishSystemOnComplete && actionResult != Status.Running)
            {
                Graph?.Finish(actionResult);
            }

            return actionResult;
        }

        /// <summary>
        /// <inheritdoc/>
        /// Pauses the execution of the action.
        /// </summary>
        protected override void OnElementPaused() => Action?.Pause();

        public override void SetContext(ExecutionContext context)
        {
            Action?.SetContext(context);
        }
    }
}