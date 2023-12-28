using UnityEngine;
using BSDesigner.Core;

namespace BSDesigner.UnityTool.Runtime
{
    /// <summary>
    /// Base class for all behaviour system runners.
    /// </summary>
    public abstract class BSRunner : MonoBehaviour
    {
        [Tooltip("Restart execution when finished?")]
        public bool ExecuteOnLoop;

        bool _systemRunning;

        BehaviourEngine _executionEngine;

        /// <summary>
        /// Override this method to set the context that the graph will use.
        /// </summary>
        /// <returns>The context created.</returns>
        protected virtual UnityExecutionContext CreateContext()
        {
            var context = new UnityExecutionContext(this);
            return context;
        }

        /// <summary>
        /// Gets the main engine that will be executed.
        /// </summary>
        /// <returns>The execution <see cref="BehaviourEngine"></see></returns>
        protected abstract BehaviourEngine CreateBehaviourSystem();


        private void Awake() => OnAwake();

        private void Start() => OnStarted();

        private void Update() => OnUpdated();

        private void OnEnable() => OnEnabled();

        private void OnDisable() => OnDisabled();


        /// <summary>
        /// Called in awake event.
        /// Create the behaviour graph and set the context.
        /// </summary>
        protected virtual void OnAwake()
        {
            _executionEngine = CreateBehaviourSystem();

            if (_executionEngine != null)
            {
                var context = CreateContext();
                _executionEngine.SetContext(context);
            }
        }

        /// <summary>
        /// Called in start event.
        /// Starts the graph execution.
        /// </summary>
        protected virtual void OnStarted()
        {
            if (_executionEngine != null)
            {
                _executionEngine.Start();
                _systemRunning = true;
            }
            else
            {
                Debug.LogWarning("EXECUTION ERROR: This runner has not graph attached.", this);
                Destroy(this);
            }
        }

        /// <summary>
        /// Called in update event.
        /// Update the graph executions and restart it when finish if ExecuteOnLoop flag is raised.
        /// </summary>
        protected virtual void OnUpdated()
        {
            if (_executionEngine != null)
            {
                if (_executionEngine.Status != Status.Running) return;

                _executionEngine.Update();

                if (_executionEngine.Status == Status.Running) return;

                _executionEngine.Stop();

                if (ExecuteOnLoop) _executionEngine.Start();

            }
            else
            {
                Debug.LogWarning("EXECUTION ERROR: This runner has not graphs attached.", this);
                Destroy(this);
            }
        }

        /// <summary>
        /// Called in ondisable event.
        /// The method called in this event depends on dontStopOnDisable configuration.
        /// </summary>
        protected virtual void OnDisabled()
        {
            if (!_systemRunning || _executionEngine == null)
                return;

            _executionEngine.Stop();
        }

        /// <summary>
        /// Called in onenable event.
        /// The method called in this event depends on dontStopOnDisable configuration.
        /// </summary>
        protected virtual void OnEnabled()
        {
            if (!_systemRunning || _executionEngine == null)
                return;

            if (_executionEngine.Status == Status.None)
            {
                _executionEngine.Start();
            }
        }
    }
}