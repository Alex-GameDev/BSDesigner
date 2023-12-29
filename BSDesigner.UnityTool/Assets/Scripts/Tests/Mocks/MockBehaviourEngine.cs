using BSDesigner.Core;

public class MockBehaviourEngine : BehaviourEngine
{
    public ActionTask ActionTask { get; set; }

    public override void SetContext(ExecutionContext context) => ActionTask.SetContext(context);

    protected override void OnStarted() => ActionTask.Start();

    protected override void OnUpdated() => ActionTask.Update();

    protected override void OnStopped() => ActionTask.Stop();

    protected override void OnPaused() => ActionTask.Pause();
}
