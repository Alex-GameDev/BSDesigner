using System;
using BSDesigner.Core;
using BSDesigner.Core.Exceptions;
using BSDesigner.Core.Tasks;

namespace BSDesigner.UtilitySystems
{
    public class UtilityAction : UtilityElement
    {
        /// <summary>
        /// The <see cref="Action"/> that this <see cref="UtilityAction"/> executes when is selected.
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

        public override Type ChildType => typeof(UtilityFactor);
        public override int MaxOutputConnections => 1;

        protected UtilityFactor Factor
        {
            get
            {
                if (_cachedFactor == null)
                {
                    if (Children.Count == 0) throw new MissingConnectionException("Can't find the child node if the children list is empty");
                    _cachedFactor = (UtilityFactor)Children[0];
                }
                return _cachedFactor;
            }
        }

        private UtilityFactor? _cachedFactor;

        public override bool HasPriority => false;

        /// <summary>
        /// <inheritdoc/>
        /// Updates the utility of the factor and gets it.
        /// </summary>
        /// <returns>The utility of the factor</returns>
        protected override float GetUtility()
        {
            Factor.UpdateUtility();
            return Factor.Utility;
        }

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
    }
}