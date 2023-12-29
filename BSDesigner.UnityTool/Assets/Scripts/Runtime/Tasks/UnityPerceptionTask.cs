using BSDesigner.Core;

namespace BSDesigner.Unity.Runtime
{
    /// <summary>
    /// Perception type specified for Unity Environment
    /// </summary>
    public abstract class UnityPerceptionTask : PerceptionTask
    {
        /// <summary>
        /// The execution context of the perception. Use it to get component references to the 
        /// object that executes the perception.
        /// </summary>
        protected UnityExecutionContext context;

        public override void SetContext(ExecutionContext ctx)
        {
            this.context = (UnityExecutionContext)ctx;
            OnSetContext();
        }

        /// <summary>
        /// Override this method to store component references from <see cref="context"/>.
        /// </summary>
        protected virtual void OnSetContext()
        {
            return;
        }
    }
}