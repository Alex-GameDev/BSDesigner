using System;

namespace BSDesigner.Core.Perceptions
{
    /// <summary>
    /// Perception Task that execute custom methods on events
    /// </summary>
    public class CustomPerceptionTask : PerceptionTask
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
        /// Method called on check event. Must return a boolean value.
        /// </summary>
        public Func<bool>? OnCheck;

        public override string GetInfo() => "Custom perception";

        protected override void OnBeginTask() => OnBegin?.Invoke();

        protected override void OnEndTask() => OnEnd?.Invoke();

        protected override void OnPauseTask() => OnPause?.Invoke();

        protected override void OnResumeTask() => OnResume?.Invoke();

        protected override bool OnCheckPerception() => OnCheck?.Invoke() ?? false;
    }
}
