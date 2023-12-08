using BSDesigner.Core;

namespace BSDesigner.BehaviourTrees
{
    /// <summary>
    /// Serial Composite node that executes its children until one of them returns success.
    /// </summary>
    public class SelectorNode : SerialCompositeNode
    {
        protected override bool GoToNextChild(Status status) => status == Status.Failure;

        protected override Status GetFinalStatus(Status status)=> status;
    }
}
