using BSDesigner.Core;

namespace BSDesigner.BehaviourTrees
{
    /// <summary>
    /// Node that execute its child node the number of times determined by <see cref="Iterations"/>
    /// </summary>
    public class LoopNode : DirectDecoratorNode
    {
        /// <summary>
        /// The number of times that the child node should end its execution to end the decorator.
        /// if its value is -1 this number is infinite.
        /// </summary>
        public int Iterations = -1;

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
        /// <inheritdoc/> If the child execution ends increase the current iteration count. If this count reaches <see cref="Iterations"/>, 
        /// return the status of the child, otherwise restart the child and return Running.
        /// </summary>
        /// <param name="childStatus">The current child status.</param>
        /// <returns>Running if the iteration count is not reached, <paramref name="childStatus"/> otherwise.</returns>
        protected override Status ModifyStatus(Status childStatus)
        {
            // If child execution ends, restart until currentIterations > MaxIterations
            if (childStatus != Status.Running)
            {
                _currentIterations++;
                if (Iterations == -1 || _currentIterations < Iterations)
                {
                    childStatus = Status.Running;
                    ChildNode.Stop();
                    ChildNode.Start();
                }
            }
            return childStatus;
        }
    }
}
