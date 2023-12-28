namespace BSDesigner.Core
{
    /// <summary>
    /// Generic logger provider
    /// </summary>
    /// <typeparam name="T">The type of the generated logger.</typeparam>
    public class LoggerProvider<T> : ILoggerProvider where T : ILogger, new()
    {
        public ILogger CreateLogger() => new T();
    }
}