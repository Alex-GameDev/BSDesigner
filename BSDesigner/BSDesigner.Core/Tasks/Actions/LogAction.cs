using System.ComponentModel;
using BSDesigner.Core.Exceptions;
using BSDesigner.Core.Tasks;

namespace BSDesigner.Core.Actions
{
    /// <summary>
    /// Action that logs a message.
    /// </summary>
    [TaskCategory("Debug")]
    public class LogAction : ActionTask
    {
        /// <summary>
        /// The message logged.
        /// </summary>
        [DefaultValue("")]
        public string Message = string.Empty;
        
        /// <summary>
        /// The log level
        /// </summary>
        public LogLevel Level;

        private ILogger? _logger;

        public override string GetInfo() => $"Log \"{Message}\"";

        protected override void OnBeginTask()
        {
            if (_logger == null)
                throw new ExecutionContextException("Not ILogger provided.");

            _logger.LogMessage(Message, Level);
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