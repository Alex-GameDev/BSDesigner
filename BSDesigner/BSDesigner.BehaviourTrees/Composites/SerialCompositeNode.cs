using BSDesigner.Core;

namespace BSDesigner.BehaviourTrees
{
    public abstract class SerialCompositeNode : CompositeNode
    {
        private int _currentChildIdx;

        /// <summary>
        /// <inheritdoc/>
        /// Starts the first node execution.
        /// </summary>
        protected override void OnNodeStarted()
        {
            _currentChildIdx = 0;
            GetBranch(_currentChildIdx).Start();
        }

        /// <summary>
        /// <inheritdoc/>
        /// Stops the current executed child.
        /// </summary>
        protected override void OnNodeStopped()
        {
            GetBranch(_currentChildIdx).Stop();
        }

        /// <summary>
        /// <inheritdoc/>
        /// Pauses the current executed child.
        /// </summary>
        protected override void OnNodePaused()
        {
            GetBranch(_currentChildIdx).Pause();
        }

        /// <summary>
        /// <inheritdoc/>
        /// Update the execution of the current child. If it returned status is not the final
        /// status, stop the child and starts the next. If there are no more children, return the final status.
        /// </summary>
        /// <returns>Running if no child has returned the target status and all children were not executed yet. Else returns the status of the last node.</returns>
        protected override Status UpdateStatus()
        {
            var currentChild = GetBranch(_currentChildIdx);
            currentChild.Update();
            var status = currentChild.Status;

            if (GoToNextChild(status) && _currentChildIdx < Children.Count - 1)
            {
                _currentChildIdx++;
                currentChild.Stop();
                currentChild = GetBranch(_currentChildIdx);
                currentChild.Start();
                return Status.Running;
            }
            else
            {
                return GetFinalStatus(status);
            }
        }

        /// <summary>
        /// Return if the execution should jump to the next child if exists.
        /// </summary>
        /// <param name="status">The current status of the child.</param>
        /// <returns>true if <paramref name="status"/> is not the target value, false otherwise. </returns>
        protected abstract bool GoToNextChild(Status status);

        /// <summary>
        /// Get the final status of the composite node (must be success or failure).
        /// </summary>
        /// <param name="status">The current status of the node.</param>
        /// <returns>The final execution status of the node.</returns>
        protected abstract Status GetFinalStatus(Status status);
    }
}

//TODO: Add random order flag