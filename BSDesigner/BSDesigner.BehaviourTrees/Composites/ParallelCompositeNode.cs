using BSDesigner.Core;

namespace BSDesigner.BehaviourTrees
{
    /// <summary>
    /// Composite node that executes all its children in all execution frames. 
    /// </summary>
    public class ParallelCompositeNode : CompositeNode
    {
        /// <summary>
        /// If true, the parallel action will stop when any action ends with success.
        /// </summary>
        public bool finishOnAnySuccess;

        /// <summary>
        /// If true, the parallel action will stop when any action ends with success.
        /// </summary>
        public bool finishOnAnyFailure;

        /// <summary>
        /// <inheritdoc/>
        /// Update all its children node. The returned <see cref="Status"/> value depends on the children status and the values of <see cref="finishOnAnySuccess"/> and <see cref="finishOnAnyFailure"/>.
        /// </summary>
        /// <returns>Success if any end with success and <see cref="finishOnAnySuccess"/> is true,
        /// Failure if any ends with failure and <see cref="finishOnAnyFailure"/> is true,
        /// if all children ends, return the last result given, otherwise returns Running.
        /// </returns>
        protected override Status UpdateStatus()
        {
            var returnedStatus = Status.Running;
            var lastResult = Status.Running;
            var anyRunning = false;

            for (var i = 0; i < Children.Count; i++)
            {
                var currentChild = GetBranch(i);
                currentChild.Update();
                var childStatus = currentChild.Status;

                if ((finishOnAnySuccess && childStatus == Status.Success) ||
                    (finishOnAnyFailure && childStatus == Status.Failure))
                {
                    returnedStatus = childStatus;
                }

                anyRunning |= childStatus == Status.Running;
                lastResult = childStatus;
            }

            if (!anyRunning && returnedStatus == Status.Running) returnedStatus = lastResult;
            return returnedStatus;
        }

        /// <summary>
        /// <inheritdoc/>
        /// Start all children.
        /// </summary>
        protected override void OnNodeStarted()
        {
            for (var i = 0; i < Children.Count; i++)
            {
                GetBranch(i).Start();
            }
        }

        /// <summary>
        /// <inheritdoc/>
        /// Stop all children.
        /// </summary>
        protected override void OnNodeStopped()
        {
            for (var i = 0; i < Children.Count; i++)
            {
                GetBranch(i).Stop();
            }
        }

        /// <summary>
        /// <inheritdoc/>
        /// Pause all children.
        /// </summary>
        protected override void OnNodePaused()
        {
            for (var i = 0; i < Children.Count; i++)
            {
                GetBranch(i).Pause();
            }
        }
    }
}