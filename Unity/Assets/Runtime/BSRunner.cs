using BSDesigner.Core;
using BSDesigner.Unity;
using UnityEngine;

/// <summary>
/// Defines an unity component that runs a behaviour system
/// </summary>
public abstract class BSRunner : MonoBehaviour
{
    [Tooltip("Restart execution when finished?")]
    public bool executeOnLoop;

    BehaviourEngine _engine;

    bool _systemRunning;

    private void Awake() => InitBehaviour();

    private void Start() => StartBehaviour();

    private void Update() => UpdateBehaviour();

    private void OnDisable() => PauseBehaviour();


    /// <summary>
    /// Creates the behaviour system
    /// </summary>
    protected abstract BehaviourEngine CreateBehaviourSystem();

    /// <summary>
    /// Override this method to set the Awake event instead of implement it.
    /// <para>Always include the base call!</para>
    /// </summary>
    protected virtual void InitBehaviour()
    {
        _engine = CreateBehaviourSystem();
        UnityExecutionContext context = CreateContext();
        _engine?.SetContext(context);
    }

    /// <summary>
    /// Override this method to set the Start event instead of implement it.
    /// <para>Always include the base call!</para>
    /// </summary>
    protected virtual void StartBehaviour()
    {
        if (_engine == null)
        {
            Debug.LogWarning("EXECUTION ERROR: This runner has not system attached.", this);
            Destroy(this);
        }

        _engine.Start();
        _systemRunning = true;
    }

    /// <summary>
    /// Override this method to set the Update event instead of implement it.
    /// <para>Always include the base call!</para>
    /// </summary>
    protected virtual void UpdateBehaviour()
    {
        if (_engine == null)
        {
            Debug.LogWarning("EXECUTION ERROR: This runner has not system attached.", this);
            Destroy(this);
        }

        if (_engine.Status != Status.Running) return;

        _engine.Update();

        if(_engine.Status != Status.Running && _engine.Status != Status.None)
        {
            _engine.Stop();
        }
    }
    /// <summary>
    /// Override this method to set the OnDisable event instead of implement it.
    /// <para>Always include the base call!</para>
    /// </summary>
    protected virtual void PauseBehaviour()
    {
        if (!_systemRunning || _engine == null)
            return;

        _engine.Pause();
    }

    /// <summary>
    /// Override this method to specify which context the runner is going to use.
    /// </summary>
    protected virtual UnityExecutionContext CreateContext()
    {
        UnityExecutionContext context = new UnityExecutionContext(this);
        return context;
    }
}
