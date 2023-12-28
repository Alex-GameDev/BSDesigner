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
        void Start(float timeInSeconds);

        /// <summary>
        /// Stop the time count.
        /// </summary>
        void Stop();

        /// <summary>
        /// Pause the time count
        /// </summary>
        void Pause();

        /// <summary>
        /// Tick and resume the time count.
        /// </summary>
        void Tick();

        /// <summary>
        /// True if the time count is completed.
        /// </summary>
        bool IsTimeout { get; }

    }
}