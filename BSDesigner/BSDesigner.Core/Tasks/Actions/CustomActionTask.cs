using System;

namespace BSDesigner.Core.Actions
{
    /// <summary>
    /// Action Task that execute custom methods on events
    /// </summary>
    public class CustomActionTask : ActionTask
    {
        /// <summary>
        /// Method called on start event
        /// </summary>
        public Action? OnBegin;
        /// <summary>
        /// Method called on end event
        /// </summary>
        public Action? OnEnd;
        /// <summary>
        /// Method called on pause event
        /// </summary>
        public Action? OnPause;
        /// <summary>
        /// Method called on resume event
        /// </summary>
        public Action? OnResume;
        /// <summary>
        /// Method called on update tick event. Must return a status value.
        /// </summary>
        public Func<Status>? OnUpdate;

        public override string GetInfo() => "Custom action";

        protected override void OnBeginTask() => OnBegin?.Invoke();

        protected override void OnEndTask() => OnEnd?.Invoke();

        protected override void OnPauseTask() => OnPause?.Invoke();

        protected override void OnResumeTask() => OnResume?.Invoke();

        protected override Status OnUpdateTask() => OnUpdate?.Invoke() ?? Status.Failure;
    }
}