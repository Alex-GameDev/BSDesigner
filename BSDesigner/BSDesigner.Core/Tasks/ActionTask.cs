namespace BSDesigner.Core.Tasks
{
    public abstract class ActionTask : Task
    {
        private Status _result;

        /// <summary>
        /// Updates the action by calling <see cref="OnUpdateTask"/> and call <see cref="Task.OnEndTask"/> if returns Success or Failure.
        /// <para>If the action is paused, resumes the execution and call <see cref="Task.OnResumeTask"/> before update.</para>
        /// </summary>
        /// <returns>The result of the action.</returns>
        public Status Update()
        {
            if (IsPaused)
            {
                IsPaused = false;
                OnResumeTask();
            }

            if (!IsActive) return _result;

            _result = OnUpdateTask();
            if (_result == Status.Running) return _result;

            IsActive = false;
            OnEndTask();

            return _result;
        }

        /// <summary>
        /// Event called when the action updates.
        /// </summary>
        /// <returns>The result of the action execution.</returns>
        protected abstract Status OnUpdateTask();
    }
}