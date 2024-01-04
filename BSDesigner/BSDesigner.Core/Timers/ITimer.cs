namespace BSDesigner.Core
{
    /// <summary>
    /// Interface for timers.
    /// </summary>
    public interface ITimer
    {
        /// <summary>
        /// Start the time count with the specified amount of time in seconds.
        /// </summary>
        /// <param name="timeInSeconds">The amount of time in seconds.</param>
        public void Start(float timeInSeconds);

        /// <summary>
        /// Stop the time count.
        /// </summary>
        public void Stop();

        /// <summary>
        /// Pause the time count
        /// </summary>
        public void Pause();

        /// <summary>
        /// Resume the time count.
        /// </summary>
        public void Resume();

        /// <summary>
        /// True if the time count is completed.
        /// </summary>
        public bool IsTimeout { get; }
    }
}