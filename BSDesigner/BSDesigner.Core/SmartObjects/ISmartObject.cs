namespace BSDesigner.Core.SmartObjects
{
    /**
     * Defines an smart object that generates an ActionTask for the given
     * context or agent.
     */
    public interface ISmartObject
    {
        /// <summary>
        /// Request the interaction.
        /// </summary>
        /// <returns> The interaction provided. </returns>
        ActionTask RequestInteraction(ExecutionContext context);
    }
}
