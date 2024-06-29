namespace BSDesigner.Core
{
    public abstract class Task
    {
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

        public virtual void SetContext(ExecutionContext context)
        {
            return;
        }

        protected abstract void OnBeginTask();

        protected abstract void OnEndTask();

        protected abstract void OnPauseTask();

        protected abstract void OnResumeTask();
    }
}