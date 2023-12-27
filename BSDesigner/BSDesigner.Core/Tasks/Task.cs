namespace BSDesigner.Core
{
    public abstract class Task
    {
        /// <summary>
        /// A custom description of the task.
        /// </summary>
        public string Description = string.Empty;

        /// <summary>
        /// A summary of the task, generated depending on its configuration.
        /// </summary>
        /// <returns>The task summary</returns>
        public abstract string GetInfo();

        /// <summary>
        /// Is the task active (started but not finished)?
        /// </summary>
        protected bool IsActive;

        /// <summary>
        /// Is the task paused?
        /// </summary>
        protected bool IsPaused;

        public void Start()
        {
            if (IsActive) return;

            IsActive = true;
            IsPaused = false;
            OnBeginTask();
        }

        public void Pause()
        {
            if (IsPaused || !IsActive) return;
            IsPaused = true;
            OnPauseTask();
        }

        public void Stop()
        {
            if (!IsActive) return;

            IsActive = false;
            IsPaused = false;
            OnEndTask();
        }

        protected abstract void OnBeginTask();

        protected abstract void OnEndTask();

        protected abstract void OnPauseTask();

        protected abstract void OnResumeTask();

        /// <summary>
        /// Override this method to use the execution context.
        /// </summary>
        /// <param name="context">The context passed.</param>
        public virtual void SetContext(ExecutionContext context)
        {
        }
    }
}