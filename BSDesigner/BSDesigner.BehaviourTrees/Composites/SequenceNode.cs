using BSDesigner.Core;

namespace BSDesigner.BehaviourTrees
{
    /// <summary>
    /// Serial Composite node that executes its children until one of them returns failure.
    /// </summary>
    public class SequenceNode : SerialCompositeNode
    {
        protected override bool GoToNextChild(Status status) => status == Status.Success;

        protected override Status GetFinalStatus(Status status) => status;
    }
}