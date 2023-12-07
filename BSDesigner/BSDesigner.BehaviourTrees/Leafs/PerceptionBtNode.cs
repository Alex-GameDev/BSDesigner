using BSDesigner.Core;
using BSDesigner.Core.Tasks;
using BSDesigner.Core.Exceptions;

namespace BSDesigner.BehaviourTrees
{
    /// <summary>
    /// LeafNode type that executes an perception and return Success if the perception is true or failure if its false.
    /// </summary>
    public class PerceptionBtNode : LeafBtNode
    {
        /// <summary>
        /// The perception that this node will use to get its status.
        /// </summary>
        public PerceptionTask? Perception;

        /// <summary>
        /// Which value give when perception returns true?
        /// </summary>
        public Status ValueOnTrue = Status.Success;

        /// <summary>
        /// Which value give when perception returns false?
        /// </summary>
        public Status ValueOnFalse = Status.Failure;

        /// <summary>
        /// Get the task associated with this leaf node.
        /// </summary>
        /// <returns>The task.</returns>
        /// <exception cref="MissingTaskException">Thrown if the action is null.</exception>
        protected override Task GetTask()
        {
            if (Perception == null)
                throw new MissingTaskException("Leaf nodes need an task to work.");

            return Perception;
        }

        /// <summary>
        /// <inheritdoc/>
        /// Updates the node execution.
        /// </summary>
        /// <returns>Success if the perception returns true, failure otherwise.</returns>
        /// <exception cref="MissingTaskException">Thrown if the perception is null.</exception>
        protected override Status UpdateStatus()
        {
            if (Perception == null)
                throw new MissingTaskException("Leaf nodes need a task to work.");

            var perceptionResult = Perception.Check();
            var statusResult = perceptionResult ? ValueOnTrue : ValueOnFalse;

            if(statusResult == Status.None)
                throw new ExecutionStatusException(this, "Leaf node cannot return Status.Node, check valueOnTrue and valueOnFalse values");

            return Status;
        }
    }
}