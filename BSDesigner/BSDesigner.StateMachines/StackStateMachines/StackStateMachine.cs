using System.Collections.Generic;
using BSDesigner.Core;

namespace BSDesigner.StateMachines.StackStateMachines
{
    /// <summary>
    /// Subclass of fsm that stores its active states in a stack, and can push and pop these states.
    /// </summary>
    public class StackStateMachine : StateMachine
    {
        private readonly Stack<State> _stateStack = new Stack<State>();

        /// <summary>
        /// A read-only collection with all the states in the stack.
        /// </summary>
        public IReadOnlyCollection<State> Stack => _stateStack;

        /// <summary>
        /// Create a new <see cref="PushTransition"/> that goes from the state <paramref name="from"/> to the state <paramref name="to"/>.
        /// This transition will check <paramref name="perception"/> when <paramref name="from"/> Status matches
        /// <paramref name="flags"/>. If <paramref name="perception"/> is null, check always return true.
        /// When the transition is triggered, fsm current status changes to <paramref name="to"/> state, pushing the last state in the stack.
        /// </summary>
        /// <param name="from">The source state of the transition and it's parent node.</param>
        /// <param name="to">The target state of the transition and it's child node.</param>
        /// <param name="perception">The perception checked by the transition.</param>
        /// <param name="flags">The status that the source state can have to check the perception. If none, the transition will never be checked.</param>
        /// <returns>The push transition created.</returns>
        public PushTransition CreatePushTransition(State from, State to, PerceptionTask? perception = null,
            StatusFlags flags = StatusFlags.Active)
        {
            var pushTransition = CreateInternalTransition<PushTransition>(from, perception, flags);
            ConnectNodes(pushTransition, to);
            return pushTransition;
        }

        /// <summary>
        /// Create a new <see cref="PopTransition"/> from the state <paramref name="from"/>.
        /// This transition will check <paramref name="perception"/> when <paramref name="from"/> Status matches
        /// <paramref name="flags"/>. If <paramref name="perception"/> is null, check always return true.
        /// When the transition is triggered, fsm current status changes to the peak state in the stack, popping it.
        /// </summary>
        /// <param name="from">The source state of the transition and it's parent node.</param>
        /// <param name="perception">The perception checked by the transition.</param>
        /// <param name="flags">The status that the source state can have to check the perception. If none, the transition will never be checked.</param>
        /// <returns>The pop transition created.</returns>
        public PopTransition CreatePopTransition(State from, PerceptionTask? perception = null, StatusFlags flags = StatusFlags.Active)
        {
            var popTransition = CreateInternalTransition<PopTransition>(from, perception, flags);
            return popTransition;
        }

        /// <summary>
        /// Change the current state of the fsm and push the last in the stack.
        /// </summary>
        /// <param name="targetState">The new current state.</param>
        public void PushState(State targetState)
        {
            var lastState = CurrentState;
            ChangeState(targetState);
            _stateStack.Push(lastState);
        }

        /// <summary>
        /// Return to the last state saved in the stack, if exists.
        /// </summary>
        public void PopState()
        {
            if(_stateStack.Count == 0) return;

            var targetState = _stateStack.Pop();
            ChangeState(targetState);
        }
    }
}