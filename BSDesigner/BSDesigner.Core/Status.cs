namespace BSDesigner.Core
{
    /// <summary>
    /// Defines the execution status of an element.
    /// </summary>
    public enum Status
    {
        /// <summary>
        /// The element is not being executed.
        /// </summary>
        None = 0,

        /// <summary>
        /// The element is being executed.
        /// </summary>
        Running = 1,

        /// <summary>
        /// The element execution ended with success.
        /// </summary>
        Success = 2,

        /// <summary>
        /// The element execution ended with failure.
        /// </summary>
        Failure = 4,
    }
}