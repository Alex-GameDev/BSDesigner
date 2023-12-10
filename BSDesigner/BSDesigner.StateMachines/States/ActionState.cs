using BSDesigner.Core;
using BSDesigner.Core.Tasks;

namespace BSDesigner.StateMachines
{
    /// <summary>
    /// Represents a state that executes an action task when is the current state of the machine.
    /// </summary>
    public class ActionState : State
    {
        /// <summary>
        /// The action that this state executes.
        /// </summary>
        public ActionTask? Action;

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
        protected override Status OnUpdated() => Action?.Update() ?? Status.None;
    }
}