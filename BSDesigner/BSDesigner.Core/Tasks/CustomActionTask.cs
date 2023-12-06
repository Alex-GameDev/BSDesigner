using System;

namespace BSDesigner.Core.Tasks
{
    public class CustomActionTask : ActionTask
    {
        public Action? OnBegin;
        public Action? OnEnd;
        public Action? OnPause;
        public Action? OnResume;
        public Func<Status>? OnUpdate;

        public override string GetInfo() => "Custom action";

        protected override void OnBeginTask() => OnBegin?.Invoke();

        protected override void OnEndTask() => OnEnd?.Invoke();

        protected override void OnPauseTask() => OnPause?.Invoke();

        protected override void OnResumeTask() => OnResume?.Invoke();

        protected override Status OnUpdateTask() => OnUpdate?.Invoke() ?? Status.Failure;
    }
}