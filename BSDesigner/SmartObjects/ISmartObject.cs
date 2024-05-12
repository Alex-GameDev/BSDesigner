using BSDesigner.Core;

namespace BSDesigner.SmartObjects
{
    public interface ISmartObject
    {
        /// <summary>
        /// Request the interaction.
        /// </summary>
        /// <returns> The interaction provided. </returns>
        ActionTask RequestInteraction(ExecutionContext context);
    }
}
