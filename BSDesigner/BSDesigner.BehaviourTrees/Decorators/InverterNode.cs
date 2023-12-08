using BSDesigner.Core;

namespace BSDesigner.BehaviourTrees
{
    /// <summary>
    /// Extension methods for Status enum.
    /// </summary>
    public class InverterNode : DirectDecoratorNode
    {
        /// <summary>
        /// <inheritdoc/>
        /// Get the inverted value of <paramref name="childStatus"/>.
        /// </summary>
        /// <param name="childStatus"><inheritdoc/></param>
        /// <returns>Success if <paramref name="childStatus"/> is Failure, Failure if <paramref name="childStatus"/> is success, Running otherwise.</returns>
        protected override Status ModifyStatus(Status childStatus)
        {
            if (childStatus == Status.Success) return Status.Failure;
            if (childStatus == Status.Failure) return Status.Success;
            return childStatus;
        }

    }
}