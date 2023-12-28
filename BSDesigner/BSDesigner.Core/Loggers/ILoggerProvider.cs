namespace BSDesigner.Core
{
    public interface ILoggerProvider
    {
        /// <summary>
        /// Create a new logger.
        /// </summary>
        /// <returns>The logger generated.</returns>
        ILogger CreateLogger();
    }
}