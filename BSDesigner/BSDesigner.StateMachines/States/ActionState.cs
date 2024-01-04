using BSDesigner.Core;
using BSDesigner.Core.Actions;

namespace BSDesigner.StateMachines
{
    /// <summary>
    /// Represents a state that executes an action task when is the current state of the machine.
    /// </summary>
    public class ActionState : State
    {
        public override int MaxInputConnections => -1;

        /// <summary>
        /// The action that this state executes.
        /// </summary>
        public ActionTask? Action;

        /// <summary>
        /// Should the action execute forever keeping the status on running until a transition is triggered?
        /// </summary>
        public bool ExecuteInLoop;

        /// <summary>
        /// <inheritdoc/>
        /// Starts the action.
        /// </summary>
        protected override void OnEntered() => Action?.Start();

        /// <summary>
        /// <inheritdoc/>
        /// Stops the action.
        /// </summary>
        protected override void OnExited() => Action?.Stop();

        /// <summary>
        /// <inheritdoc/>
        /// Pauses the action.
        /// </summary>
        protected override void OnPaused() => Action?.Pause();

        /// <summary>
        /// <inheritdoc/>
        /// Updates the action and returns its result.
        /// </summary>
        /// <returns>The result of the action</returns>
        protected override Status OnUpdated()
        {
            var result = Action?.Update() ?? Status.Running;
            if (!ExecuteInLoop || result == Status.Running) return result;

            Action?.Stop();
            Action?.Start();
            return Status.Running;
        }
    }
}