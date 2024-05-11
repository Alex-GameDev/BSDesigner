namespace BSDesigner.Core
{
    /// <summary>
    /// Element that can be shared between elements in a behaviour system
    /// </summary>
    public class ExecutionContext
    {
        /// <summary>
        /// The context timer provider.
        /// </summary>
        public ITimerProvider? TimerProvider { get; set; }

        /// <summary>
        /// The context random provider.
        /// </summary>
        public IRandomProvider? RandomProvider { get; set; }
    }
}
