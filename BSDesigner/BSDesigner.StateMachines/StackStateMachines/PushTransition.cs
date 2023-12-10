using BSDesigner.Core.Exceptions;

namespace BSDesigner.StateMachines.StackStateMachines
{
    /// <summary>
    /// Stack transition between two states that saves the source state in the stack when is performed.
    /// </summary>
    public class PushTransition : StackTransition
    {
        public override int MaxOutputConnections => 1;

        /// <summary>
        /// The target state of the transition
        /// </summary>
        protected State TargetState
        {
            get
            {
                if (_cachedTargetState == null)
                {
                    if (Children.Count == 0) 
                        throw new MissingConnectionException("Can't find the child node if the children list is empty");
                    _cachedTargetState = (State)Children[0];
                }
                return _cachedTargetState;
            }
        }

        private State? _cachedTargetState;

        /// <summary>
        /// <inheritdoc/>
        /// Set target state to the current state of the fsm and push source state in the stack.
        /// </summary>
        protected override void OnTransitionPerformed() => StackStateMachine.PushState(TargetState);
    }
}