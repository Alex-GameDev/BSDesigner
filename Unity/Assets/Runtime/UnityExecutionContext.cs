using BSDesigner.Core;

namespace BSDesigner.Unity
{
    /// <summary>
    /// BS execution context created using an unity component.
    /// </summary>
    public class UnityExecutionContext : ExecutionContext
    {
        /// <summary>
        /// Unity component that is running a behaviour system using this context.
        /// </summary>
        public BSRunner Runner { get; }

        public UnityExecutionContext(BSRunner runner)
        {
            Runner = runner;
        }
    }
}
