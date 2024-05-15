using BSDesigner.Core;
using BSDesigner.Core.Exceptions;
using System;

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
        /// Starts the action execution.
        /// </summary>
        /// <exception cref="MissingTaskException">If the action is null</exception>
        protected override void OnNodeStarted()
        {
            if (Perception == null)
                throw new MissingTaskException("Action leaf nodes need an task to work.");

            Perception.Start();
        }

        /// <summary>
        /// <inheritdoc/>
        /// Stops the action execution.
        /// </summary>
        /// <exception cref="MissingTaskException"></exception>
        protected override void OnNodeStopped()
        {
            if (Perception == null)
                throw new MissingTaskException("Action leaf nodes need an task to work.");

            Perception.Stop();
        }

        /// <summary>
        /// <inheritdoc/>
        /// Stops the action execution.
        /// </summary>
        /// <exception cref="MissingTaskException"></exception>
        protected override void OnNodePaused()
        {
            if (Perception == null)
                throw new MissingTaskException("Action leaf nodes need an task to work.");

            Perception.Pause();
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

            Status = statusResult;
            return Status;
        }

        public override void SetContext(ExecutionContext context)
        {
            Perception?.SetContext(context);
        }
    }
}