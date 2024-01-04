namespace BSDesigner.Core
{
    /// <summary>
    /// Interface for timer providers.
    /// </summary>
    public interface ITimerProvider
    {
        /// <summary>
        /// Create a new timer.
        /// </summary>
        /// <returns>The timer created.</returns>
        ITimer CreateTimer();
    }
}
