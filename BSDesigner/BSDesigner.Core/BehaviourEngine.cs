using System;
using BSDesigner.Core.Exceptions;

namespace BSDesigner.Core
{
    public abstract class BehaviourEngine : IStatusHandler
    {
        /// <summary>
        /// The name of the behaviour engine
        /// </summary>
        public string Name = string.Empty;

        /// <summary>
        /// The local blackboard for this engine
        /// </summary>
        public Blackboard LocalBlackboard => _localBlackboard ??= new Blackboard();
        private Blackboard? _localBlackboard;

        /// <summary>
        /// The execution status of the behaviour engine
        /// </summary>
        public Status Status
        {
            get => _status;
            protected set
            {
                if (_status == value) return;
                _status = value;
                StatusChanged?.Invoke(_status);
            }
        }

        private Status _status;

        /// <summary>
        /// The execution is currently paused?
        /// </summary>
        public bool IsPaused { get; protected set; }

        /// <summary>
        /// Event called when the status changes
        /// </summary>
        public event Action<Status>? StatusChanged;

        /// <summary>
        /// Starts the execution and set the status value to Running.
        /// </summary>
        /// <exception cref="ExecutionStatusException">If the graph is already in execution.</exception>
        public void Start()
        {
            if (Status != Status.None)
                throw new ExecutionStatusException(this, "This behaviour engine is already been executed.");

            Status = Status.Running;
            OnStarted();
        }

        /// <summary>
        /// Update the execution if is not finished yet.
        /// </summary>
        public void Update()
        {
            if(Status == Status.None)
                throw new ExecutionStatusException(this, "This behaviour engine is not running.");

            if (Status != Status.Running) return;

            IsPaused = false;
            OnUpdated();
        }

        /// <summary>
        /// Stops the execution and sets the status value to None.
        /// </summary>
        /// <exception cref="ExecutionStatusException">If the graph is not in execution.</exception>
        public void Stop()
        {
            if (Status == Status.None)
                throw new ExecutionStatusException(this, "This behaviour engine already stopped.");

            Status = Status.None;
            IsPaused = false;
            OnStopped();
        }

        /// <summary>
        /// Pauses the execution of the graph if is not paused yet.
        /// </summary>
        public void Pause()
        {
            if (IsPaused || Status != Status.Running) return;

            IsPaused = true;
            OnPaused();
        }

        /// <summary>
        /// Finish the execution of the engine giving it a final status value.
        /// </summary>
        /// <param name="status">The result of the engine execution.</param>
        /// <exception cref="ExecutionStatusException">If the given value is not Success or Failure.</exception>
        public void Finish(Status status)
        {
            if (((uint)status & (uint)StatusFlags.Finished) == 0)
                throw new ExecutionStatusException(this,$"Cannot finish execution with the given value ({status}), the value must be Success or Failure.");
            Status = status;
        }

        /// <summary>
        /// Set the execution context of the behaviour system.
        /// </summary>
        /// <param name="context">The <see cref="ExecutionContext"/> provided.</param>
        public abstract void SetContext(ExecutionContext context);

        /// <summary>
        /// Called when the graph starts the execution.
        /// </summary>
        protected abstract void OnStarted();

        /// <summary>
        /// Called every frame while the graph is executing, until its finished or stopped. 
        /// </summary>
        protected abstract void OnUpdated();

        /// <summary>
        /// Called when the graph is stopped.
        /// </summary>
        protected abstract void OnStopped();

        /// <summary>
        /// Called when the graph is paused.
        /// </summary>
        protected abstract void OnPaused();
    }
}