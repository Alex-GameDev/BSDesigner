namespace BSDesigner.Core
{
    /// <summary>
    /// Describes an element that can execute along time.
    /// </summary>
    public interface IBehaviour
    {
        /// <summary>
        /// Starts the execution.
        /// </summary>
        void Start();

        /// <summary>
        /// Update the execution.
        /// </summary>
        Status Update();

        /// <summary>
        /// Stops the execution.
        /// </summary>
        void Stop();

        /// <summary>
        /// Pauses the execution.
        /// </summary>
        void Pause();

        /// <summary>
        /// Set the execution context of the behaviour system.
        /// </summary>
        /// <param name="context">The <see cref="ExecutionContext"/> provided.</param>
        void SetContext(ExecutionContext context);
    }
}
