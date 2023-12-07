using BSDesigner.Core;
using BSDesigner.Core.Exceptions;
using System;

namespace BSDesigner.BehaviourTrees
{
    public abstract class BtNode : Node, IStatusHandler
    {
        public override int MaxInputConnections => 1;
        public override Type ChildType => typeof(BtNode);
        public override Type GraphType => typeof(BehaviourTree);

        /// <summary>
        /// The execution status of the node.
        /// </summary>
        public Status Status { get; protected set; }

        /// <summary>
        /// Event called when the status changes
        /// </summary>
        public event Action<Status>? StatusChanged;

        public void Start()
        {
            if (Status != Status.None)
                throw new ExecutionStatusException(this, $"This node ({Name}) is already been executed");

            Status = Status.Running;
            OnNodeStarted();
        }

        public void Update()
        {
            if (Status == Status.None)
                throw new ExecutionStatusException(this, $"This node ({Name}) must be started before update.");

            if (Status != Status.Running) return;

            Status = UpdateStatus();
        }

        /// <summary>
        /// Stop the node execution, changing <see cref="Status"/> to None. 
        /// </summary>
        /// <exception cref="Exception">If was already stopped.</exception>
        public virtual void Stop()
        {
            if (Status == Status.None)
                throw new ExecutionStatusException(this, $"This node ({Name}) is already been stopped");

            Status = Status.None;
            OnNodeStopped();
        }

        /// <summary>
        /// Called when the node is in a running branch and the graph is paused.
        /// </summary>
        public virtual void Pause()
        {
            if (Status == Status.Running)
                OnNodePaused();
        }

        /// <summary>
        /// Get the updated status of the node.
        /// </summary>
        /// <returns>The new status of the node.</returns>
        protected abstract Status UpdateStatus();

        protected abstract void OnNodeStarted();

        protected abstract void OnNodeStopped();

        protected abstract void OnNodePaused();
    }
}