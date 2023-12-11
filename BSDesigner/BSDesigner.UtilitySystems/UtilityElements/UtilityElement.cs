using System;
using BSDesigner.Core;
using BSDesigner.Core.Exceptions;

namespace BSDesigner.UtilitySystems.UtilityElements
{
    /// <summary>
    /// Utility node that can be selected and executed by a utility system.
    /// </summary>
    public abstract class UtilityElement : UtilityNode, IStatusHandler
    {
        public override int MaxInputConnections => 1;

        /// <summary>
        /// The execution status of the node.
        /// </summary>
        public Status Status
        {
            get => _status;
            protected set
            {
                if (_status != value)
                {
                    _status = value;
                    StatusChanged?.Invoke(_status);
                }
            }
        }
        private Status _status;

        /// <summary>
        /// Event called when current status changed.
        /// </summary>
        public event Action<Status> StatusChanged;

        /// <summary>
        /// Starts the execution of the node when the utility system selects it.
        /// </summary>
        /// <exception cref="ExecutionStatusException">If it's already running.</exception>
        public void Start()
        {
            if (Status != Status.None)
                throw new ExecutionStatusException(this, $"ERROR: This node ({Name}) is already been executed");

            Status = Status.Running;
            OnNodeStarted();
        }

        /// <summary>
        /// Updates the execution each frame the node is selected.
        /// </summary>
        public void Update()
        {
            if (Status == Status.None)
                throw new ExecutionStatusException(this, $"This node ({Name}) must be started before update.");

            if (Status != Status.Running) return;

            Status = OnNodeUpdated();
        }

        /// <summary>
        /// Stops the execution when the node is no longer selected or the <see cref="UtilitySystem"/> was stopped.
        /// </summary>
        /// <exception cref="ExecutionStatusException">If it's not running.</exception>
        public void Stop()
        {
            if (Status == Status.None)
                throw new ExecutionStatusException(this, $"ERROR: This node ({Name}) is already been stopped");

            Status = Status.None;
            OnNodeStopped();
        }

        /// <summary>
        /// Called when the node is being selected and the graph is paused.
        /// </summary>
        public virtual void Pause()
        {
            if (Status == Status.Running)
                OnNodePaused();
        }

        protected abstract void OnNodeStarted();

        protected abstract void OnNodeStopped();

        protected abstract Status OnNodeUpdated();

        protected abstract void OnNodePaused();

    }
}
