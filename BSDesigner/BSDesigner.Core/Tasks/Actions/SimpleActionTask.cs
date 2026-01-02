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
    }
}