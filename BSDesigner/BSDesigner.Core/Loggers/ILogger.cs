namespace BSDesigner.Core
{
    /// <summary>
    /// Interface for loggers
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Log a message.
        /// </summary>
        /// <param name="message">The printed string message.</param>
        /// <param name="logLevel">The level of the log (Info by default)</param>
        void LogMessage(string message, LogLevel logLevel = LogLevel.Info);

        /// <summary>
        ///  Log a formatted message.
        /// </summary>
        /// <param name="format">The formatted string template</param>
        /// <param name="logLevel">The level of the log (Info by default)</param>
        /// <param name="args">The arguments for the formatting.</param>
        void LogFormatMessage(string format, LogLevel logLevel = LogLevel.Info, params object[] args);
    }
}