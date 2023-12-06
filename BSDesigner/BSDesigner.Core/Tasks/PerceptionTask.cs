namespace BSDesigner.Core.Tasks
{
    public abstract class PerceptionTask : Task
    {
        private bool _result;

        /// <summary>
        /// Checks the perception by calling <see cref="OnCheckPerception"/>.
        /// <para>If task is paused, resumes the execution and call <see cref="Task.OnResumeTask"/> before update.</para>
        /// </summary>
        /// <returns>The returned result of the perception.</returns>
        public bool Check()
        {
            if (IsPaused)
            {
                IsPaused = false;
                OnResumeTask();
            }

            if (!IsActive) return false;

            _result = OnCheckPerception();
            return _result;
        }

        /// <summary>
        /// Event called when the action updates.
        /// </summary>
        /// <returns>The result of the action execution.</returns>
        protected abstract bool OnCheckPerception();
    }
}
