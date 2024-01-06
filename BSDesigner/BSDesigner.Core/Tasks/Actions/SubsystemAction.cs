using BSDesigner.Core.Exceptions;

namespace BSDesigner.Core.Actions
{
    /// <summary>
    /// Action that executes a behaviour engine in.
    /// </summary>
    public class SubsystemAction : ActionTask
    {
        public Parameter<BehaviourEngine> SubSystem;

        public override string GetInfo()
        {
            return $"Subsystem Action {{{SubSystem?.Value.Name ?? "No subsystem"}}}";
        }

        /// <summary>
        /// <inheritdoc/>
        /// Starts the execution of the subsystem.
        /// </summary>
        /// <exception cref="MissingBehaviourSystemException">Thrown if <see cref="SubSystem"/> is null.</exception>
        protected override void OnBeginTask()
        {
            if (SubSystem?.Value == null)
                throw new MissingBehaviourSystemException("Subsystem cannot be null");

            SubSystem.Value.Start();
        }

        /// <summary>
        /// <inheritdoc/>
        /// Stops the execution of the subsystem.
        /// </summary>
        /// <exception cref="MissingBehaviourSystemException"></exception>
        protected override void OnEndTask()
        {
            if (SubSystem?.Value == null)
                throw new MissingBehaviourSystemException("Subsystem cannot be null");

            SubSystem.Value.Stop();
        }

        /// <summary>
        /// <inheritdoc/>
        /// Pauses the execution of the subsystem.
        /// </summary>
        /// <exception cref="MissingBehaviourSystemException">Thrown if <see cref="SubSystem"/> is null.</exception>
        protected override void OnPauseTask()
        {
            if (SubSystem?.Value == null)
                throw new MissingBehaviourSystemException("Subsystem cannot be null");

            SubSystem.Value.Pause();
        }

        /// <summary>
        /// <inheritdoc/>
        /// The subsystem's resume event is called at update so this method doesn't need to invoke anything.
        /// <exception cref="MissingBehaviourSystemException">Thrown if <see cref="SubSystem"/> is null.</exception>
        /// </summary>
        protected override void OnResumeTask() {}

        /// <summary>
        /// Updates the execution of the subsystem.
        /// </summary>
        /// <returns>The current status of the subsystem.</returns>
        /// <exception cref="MissingBehaviourSystemException">Thrown if <see cref="SubSystem"/> is null.</exception>
        protected override Status OnUpdateTask()
        {
            if (SubSystem?.Value == null)
                throw new MissingBehaviourSystemException("Subsystem cannot be null");

            SubSystem.Value.Update();
            return SubSystem.Value.Status;
        }
    }
}