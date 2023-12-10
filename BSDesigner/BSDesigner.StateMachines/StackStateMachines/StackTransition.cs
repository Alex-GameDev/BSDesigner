using BSDesigner.Core.Exceptions;
using System;

namespace BSDesigner.StateMachines.StackStateMachines
{
    /// <summary>
    /// Base class for stack fsm transitions 
    /// </summary>
    public abstract class StackTransition : Transition
    {
        public override Type GraphType => typeof(StackStateMachine);

        /// <summary>
        /// The stack fsm of this transition.
        /// </summary>
        protected StackStateMachine StackStateMachine
        {
            get
            {
                if (_cachedStackStateMachine == null)
                {
                    if (Graph is StackStateMachine stateMachine)
                        _cachedStackStateMachine = stateMachine;
                    else
                        throw new MissingBehaviourSystemException();
                }
                return _cachedStackStateMachine;
            }
        }

        private StackStateMachine? _cachedStackStateMachine;
    }
}