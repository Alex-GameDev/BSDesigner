using System;
using BSDesigner.Core.Exceptions;
using System.ComponentModel;
using BSDesigner.Core.Tasks;

namespace BSDesigner.Core.Actions
{
    /// <summary>
    /// Action that logs a formatted message.
    /// </summary>
    [TaskCategory("Debug")]
    public class LogFormatAction : ActionTask
    {
        /// <summary>
        /// The message logged.
        /// </summary>
        [DefaultValue("")]
        public string Message = string.Empty;

        /// <summary>
        /// The format arguments
        /// </summary>
        [DefaultValue(new object[0])]
        public object[] Args = Array.Empty<object>();

        /// <summary>
        /// The log level
        /// </summary>
        public LogLevel Level;

        private ILogger? _logger;

        public override string GetInfo() => $"Log \"{string.Format(Message, Args)}\"";

        protected override void OnBeginTask()
        {
            if (_logger == null)
                throw new ExecutionContextException("Not ILogger provided.");

            _logger.LogFormatMessage(Message, Level, Args);
        }

        protected override void OnEndTask() { }

        protected override void OnPauseTask() { }

        protected override void OnResumeTask() { }

        protected override Status OnUpdateTask() => Status.Success;

        public override void SetContext(ExecutionContext context)
        {
            _logger = context.LoggerProvider?.CreateLogger();
        }
    }
}