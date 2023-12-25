using System.Diagnostics.CodeAnalysis;

namespace BSDesigner.Core.Actions
{
    /// <summary>
    /// Action that executes a custom method when is started and always returns success.
    /// </summary>
    public class SimpleActionTask : ActionTask
    {
        /// <summary>
        /// Delegate called when the Action is started.
        /// </summary>
        public System.Action? Action;

        public override string GetInfo() => "Simple Action";

        protected override void OnBeginTask() => Action?.Invoke();

        protected override void OnEndTask() {}

        protected override void OnPauseTask() {}

        protected override void OnResumeTask() {}

        protected override Status OnUpdateTask() => Status.Success;
    }
}