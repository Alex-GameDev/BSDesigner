using BSDesigner.Core;

namespace BSDesigner.Unity.Runtime
{
    /// <summary>
    /// Action type specified for Unity Environment
    /// </summary>
    public abstract class UnityActionTask : ActionTask
    {
        /// <summary>
        /// The execution context of the action. Use it to get component references to the 
        /// object that executes the action.
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
