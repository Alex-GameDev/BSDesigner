using System;
using BSDesigner.Core;

namespace BSDesigner.BehaviourTrees
{
    /// <summary>
    /// Node that execute its child node until returns a given value.
    /// </summary>
    public class LoopNode : DirectDecoratorNode
    {
        /// <summary>
        /// The maximum number of times that the child node can end its execution without end the decorator.
        /// if its value is -1 this number is infinite.
        /// </summary>
        public int MaxIterations = -1;

        /// <summary>
        /// The status that the child node must reach to end the loop.
        /// If is "None", the loop only ends reaching the iteration limit
        /// </summary>
        public Status TargetStatus;

        /// <summary>
        /// Event called every time an iteration is complete, using the index of the completed iteration and the status result.
        /// </summary>
        public event Action<int, Status>? IterationCompleted;

        private int _currentIterations;

        /// <summary>
        /// <inheritdoc/>
        /// Reset the current iterations.
        /// </summary>
        protected override void OnNodeStarted()
        {
            base.OnNodeStarted();
            _currentIterations = 0;
        }

        /// <summary>
        /// <inheritdoc/> If the child execution ends increase the current iteration count. If this count reaches <see cref="MaxIterations"/>
        /// or if the child status is equal to <see cref="TargetStatus"/> return the status of the child, otherwise restart the child and return Running.
        /// </summary>
        /// <param name="childStatus">The current child status.</param>
        /// <returns>Running if the iteration count and the target value are not reached, <paramref name="childStatus"/> otherwise.</returns>
        protected override Status ModifyStatus(Status childStatus)
        {
            if (childStatus == Status.Running) return childStatus;

            if (childStatus != TargetStatus)
            {
                IterationCompleted?.Invoke(_currentIterations, childStatus);
                _currentIterations++;
                if (_currentIterations != MaxIterations)
                {
                    // Restart the node execution
                    childStatus = Status.Running;
                    ChildNode.Stop();
                    ChildNode.Start();
                }
            }
            return childStatus;
        }
    }
}