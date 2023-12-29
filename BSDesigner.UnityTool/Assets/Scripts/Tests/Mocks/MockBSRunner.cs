using BSDesigner.Core;
using BSDesigner.Unity.Runtime;

public class MockBSRunner : BSRunner
{
    public BehaviourEngine Engine { get; set; }

    protected override BehaviourEngine CreateBehaviourSystem() => Engine;

    private void Awake() { }
    private void OnEnable() { }
    private void Start() { }

    public void Initialize()
    {
        OnEnabled();
        OnAwake();
    }

    public void StartEngine() => OnStarted();
}
