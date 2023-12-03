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
        /// The execution status of the behaviour engine
        /// </summary>
        public Status Status
        {
            get => _status;
            protected set
            {
                if (_status == value) return;
                var previousValue = _status;
                _status = value;
                StatusChanged?.Invoke(previousValue, value);
            }
        }

        private Status _status;

        /// <summary>
        /// Event called when the status changes
        /// </summary>
        public event Action<Status, Status>? StatusChanged;

        /// <summary>
        /// Starts the execution and set the status value to Running.
        /// </summary>
        /// <exception cref="ExecutionStatusException">If the graph is already in execution.</exception>
        public void Start()
        {
            if (Status != Status.None)
                throw new ExecutionStatusException(this, "ERROR: This behaviour engine is already been executed.");

            Status = Status.Running;
            OnStarted();
        }

        /// <summary>
        /// Update the execution if is not finished yet.
        /// </summary>
        public void Update()
        {
            if (Status != Status.Running) return;
            OnUpdated();
        }

        /// <summary>
        /// Stops the execution and sets the status value to None.
        /// </summary>
        /// <exception cref="ExecutionStatusException">If the graph is not in execution.</exception>
        public void Stop()
        {
            if (Status == Status.None)
                throw new ExecutionStatusException(this, "ERROR: This behaviour engine is not running.");

            Status = Status.None;
            OnStopped();
        }

        /// <summary>
        /// Pauses the execution of the graph if is not paused yet.
        /// </summary>
        public void Pause()
        {
            if (Status != Status.Running) return;

            Status = Status.Paused;
            OnPaused();
        }

        /// <summary>
        /// Resumes the execution of the graph is was paused before.
        /// </summary>
        public void Resume()
        {
            if (Status != Status.Paused) return;

            Status = Status.Running;
            OnResumed();
        }

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

        /// <summary>
        /// Called when the graph is resumed
        /// </summary>
        protected abstract void OnResumed();
    }
}