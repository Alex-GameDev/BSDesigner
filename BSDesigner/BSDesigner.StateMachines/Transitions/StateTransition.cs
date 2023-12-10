using BSDesigner.Core.Exceptions;

namespace BSDesigner.StateMachines
{
    /// <summary>
    /// Transition between two states.
    /// </summary>
    public class StateTransition : Transition
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
                    if (Children.Count == 0) throw new MissingConnectionException("Can't find the child node if the children list is empty");
                    _cachedTargetState = (State)Children[0];
                }
                return _cachedTargetState;
            }
        }

        private State? _cachedTargetState;

        /// <summary>
        /// <inheritdoc/>
        /// Set the target state to the current state of the machine.
        /// </summary>
        public override void Perform() => StateMachine.ChangeState(TargetState);
    }
}