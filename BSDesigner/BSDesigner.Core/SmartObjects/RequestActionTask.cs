
namespace BSDesigner.Core.SmartObjects
{
    /// <summary>
    /// Action that request a behaviour to a smart object.
    /// </summary>
    public abstract class RequestActionTask : ActionTask
    {
        /// <summary>
        /// The current interaction that this action is executing.
        /// </summary>
        ActionTask? currentInteraction;

        /// <summary>
        /// The context that the request action will propagate throught the provided actions.
        /// </summary>
        protected ExecutionContext Context { get; private set; } = null!;

        /// <summary>
        /// Get the provider that this request action will use to find the smart objects.
        /// </summary>
        /// <returns>The smart object provider.</returns>
        protected abstract ISmartObject GetSmartObject();

        public override void SetContext(ExecutionContext context)
        {
           Context = context;
        }

        protected override void OnBeginTask()
        {
            var smartObject = GetSmartObject();

            if(smartObject != null)
            {
                var interaction = smartObject.RequestInteraction(Context);
                currentInteraction = interaction;
                interaction.Start();
            }
        }

        protected override void OnEndTask()
        {
            currentInteraction?.Stop();
            currentInteraction = null;
        }

        protected override void OnPauseTask()
        {
            currentInteraction?.Pause();
        }

        protected override Status OnUpdateTask()
        {
            if (currentInteraction != null)
            {
                return currentInteraction.Update(); ;
            }
            else
            {
                return Status.Failure;
            }
        }
    }
}
