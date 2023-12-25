namespace BSDesigner.Core
{
    /// <summary>
    /// Generic timer provider that creates Timers of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the timers.</typeparam>
    public class TimerProvider<T> : ITimerProvider where T : ITimer, new()
    {
        /// <summary>
        /// Create a new Timer of type <typeparamref name="T"/>.
        /// </summary>
        /// <returns>The timer created.</returns>
        public ITimer CreateTimer()
        {
            var timer = new T();
            return timer;
        }
    }
}