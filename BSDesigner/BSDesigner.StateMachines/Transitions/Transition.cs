using System;
using BSDesigner.Core;
using BSDesigner.Core.Exceptions;
using BSDesigner.Core.Tasks;

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

        protected StateMachine StateMachine
        {
            get
            {
                if (_cachedTargetState == null)
                {
                    if (Graph is StateMachine stateMachine)
                        _cachedTargetState = stateMachine;
                    else
                        throw new MissingBehaviourSystemException();
                }
                return _cachedTargetState;
            }
        }

        private StateMachine? _cachedTargetState;

        /// <summary>
        /// Starts the perception.
        /// </summary>
        public void Init() => Perception?.Start();


        /// <summary>
        /// Stops the perception.
        /// </summary>
        public void Reset() => Perception?.Stop();


        /// <summary>
        /// Called when the transition is checked and performed.
        /// </summary>
        public abstract void Perform();

        /// <summary>
        /// Gets if the transition should be performed from the source state.
        /// </summary>
        /// <returns>The value returned by the perception or true if the perception is null.</returns>
        public bool Check() => Perception?.Check() ?? true;

        /// <summary>
        /// Pause the perception
        /// </summary>
        public void Pause() => Perception?.Pause();
    }
}