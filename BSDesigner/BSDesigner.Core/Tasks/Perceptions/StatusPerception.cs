namespace BSDesigner.Core.Perceptions
{
    /// <summary>
    /// Perception that checks if the status of a status handler matches with a given status flags.
    /// </summary>
    public class StatusPerception : PerceptionTask
    {
        /// <summary>
        /// The checked status handler
        /// </summary>
        public IStatusHandler? StatusHandler;

        /// <summary>
        /// The flags that the status of the handler should match to make the transition return true on check.
        /// </summary>
        public StatusFlags Flags = StatusFlags.Running;

        public StatusPerception()
        {
        }

        public StatusPerception(IStatusHandler? statusHandler, StatusFlags flags)
        {
            StatusHandler = statusHandler;
            Flags = flags;
        }

        public override string GetInfo() => $"Status of {StatusHandler} matches flags ({Flags})";

        protected override void OnBeginTask()
        {
        }

        protected override void OnEndTask()
        {
        }

        protected override void OnPauseTask()
        {
        }

        protected override void OnResumeTask()
        {
        }

        /// <summary>
        /// Checks if the status of the handler matches with the flags. Returns false if the handler is not defined.
        /// </summary>
        /// <returns>True if the current status of the handler matches with the flags, false otherwise.</returns>
        protected override bool OnCheckPerception() => StatusHandler != null && ((uint)StatusHandler.Status & (uint)Flags) != 0;
    }
}