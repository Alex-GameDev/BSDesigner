using System;

namespace BSDesigner.Core.Tasks
{
    public class CustomPerceptionTask : PerceptionTask
    {
        public Action? OnBegin;
        public Action? OnEnd;
        public Action? OnPause;
        public Action? OnResume;
        public Func<bool>? OnCheck;

        public override string GetInfo() => "Custom perception";

        protected override void OnBeginTask() => OnBegin?.Invoke();

        protected override void OnEndTask() => OnEnd?.Invoke();

        protected override void OnPauseTask() => OnPause?.Invoke();

        protected override void OnResumeTask() => OnResume?.Invoke();

        protected override bool OnCheckPerception() => OnCheck?.Invoke() ?? false;
    }
}
