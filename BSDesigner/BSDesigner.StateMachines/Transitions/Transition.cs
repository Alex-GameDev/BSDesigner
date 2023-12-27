using System;
using System.Linq;
using BSDesigner.Core;
using BSDesigner.Core.Exceptions;

namespace BSDesigner.StateMachines
{
    /// <summary>
    /// Base class for transitions in state machines
    /// </summary>
    public abstract class Transition : FsmNode
    {
        public override Type ChildType => typeof(State);
        public override int MaxInputConnections => 1;

        /// <summary>
        /// The perception checked by this transition.
        /// </summary>
        public PerceptionTask? Perception;

        /// <summary>
        /// The status values that the source state should match to check this perception.
        /// </summary>
        public StatusFlags StatusFlags;

        public event Action? TransitionPerformed;

        protected StateMachine StateMachine
        {
            get
            {
                if (_cachedStateMachine == null)
                {
                    if (Graph is StateMachine stateMachine)
                        _cachedStateMachine = stateMachine;
                    else
                        throw new MissingBehaviourSystemException();
                }
                return _cachedStateMachine;
            }
        }

        private StateMachine? _cachedStateMachine;

        /// <summary>
        /// Starts the perception.
        /// </summary>
        public void Init() => Perception?.Start();


        /// <summary>
        /// Stops the perception.
        /// </summary>
        public void Reset() => Perception?.Stop();

        /// <summary>
        /// Perform the transition, invoking OnTransitionPerformed event.
        /// </summary>
        /// <exception cref="InvalidTransitionException">Thrown if the source of the transition is not the current state.</exception>
        public void Perform()
        {
            var sourceNode = Parents.FirstOrDefault();
            if (sourceNode != null && StateMachine.CurrentState != sourceNode && !StateMachine.AnyStates.Contains(sourceNode))
                throw new InvalidTransitionException(this, "The source state of the transition is not the current state of the machine");

            TransitionPerformed?.Invoke();
            OnTransitionPerformed();
        }

        /// <summary>
        /// Called when the transition is checked and performed.
        /// </summary>
        protected abstract void OnTransitionPerformed();

        /// <summary>
        /// Gets if the transition should be performed from the source state.
        /// </summary>
        /// <returns>The value returned by the perception or true if the perception is null.</returns>
        public bool Check() => Perception?.Check() ?? true;

        /// <summary>
        /// Pause the perception
        /// </summary>
        public void Pause() => Perception?.Pause();

        public override void SetContext(ExecutionContext context) => Perception?.SetContext(context);
    }
}