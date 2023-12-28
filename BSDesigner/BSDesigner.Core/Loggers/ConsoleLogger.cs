using System;

namespace BSDesigner.Core
{
    /// <summary>
    /// Default logger that prints the logs in the console
    /// </summary>
    public class ConsoleLogger : ILogger
    {
        public void LogMessage(string message, LogLevel logLevel = LogLevel.Info)
        {
            Console.WriteLine($"[{nameof(logLevel)}] {message}");
        }

        public void LogFormatMessage(string format, LogLevel logLevel = LogLevel.Info, params object[] args)
        {
            Console.WriteLine($"[{nameof(logLevel)}] {format}", args);
        }
    }
}