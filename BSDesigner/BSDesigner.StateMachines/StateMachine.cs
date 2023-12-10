using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using BSDesigner.Core;
using BSDesigner.Core.Exceptions;
using BSDesigner.Core.Tasks;

namespace BSDesigner.StateMachines
{
    /// <summary>
    /// Decision system built as a State machine. Each frame, the <see cref="CurrentState"/> is executed and then it checks its
    /// transitions to change the current state or finish the execution. The first state is <see cref="EntryState"/>
    /// </summary>
    public class StateMachine : BehaviourGraph
    {
        public override Type NodeType => typeof(FsmNode);

        /// <summary>
        /// The first state that will be executed when the machine started
        /// </summary>
        protected State EntryState
        {
            get
            {
                if (_cachedEntryState == null)
                {
                    if (Nodes.Count == 0) throw new EmptyGraphException("Can't find the entry state if graph is empty");

                    _cachedEntryState = (State)Nodes.First();
                }
                return _cachedEntryState;
            }
        }

        private State? _cachedEntryState;

        /// <summary>
        /// The current state being executed
        /// </summary>
        public State CurrentState
        {
            get
            {
                if (_cachedCurrentState == null)
                    _cachedCurrentState = EntryState;
                return _cachedCurrentState;
            }
            private set
            {
                if(value.Graph != this)
                    throw new InvalidOperationException();
                _cachedCurrentState = value;
            }
        }

        private State? _cachedCurrentState;

        /// <summary>
        /// Create a new state of type <typeparamref name="T"/>.
        /// </summary>
        /// <returns>The <see cref="T"/> created.</returns>
        public T CreateState<T>() where T : State, new() => CreateNode<T>();

        /// <summary>
        /// Create a new <see cref="ActionState"/> that executes <paramref name="action"/> when is the current state.
        /// </summary>
        /// <param name="action">The action this state executes.</param>
        /// <returns>The <see cref="ActionState"/> created.</returns>
        public ActionState CreateState(ActionTask action)
        {
            var actionState = CreateNode<ActionState>();
            actionState.Action = action;
            return actionState;
        }

        /// <summary>
        /// Create a new transition that connect <paramref name="from"/> with <paramref name="to"/>.
        /// This transition will check <paramref name="perception"/> every frame if <paramref name="from"/> is the current state and its status matches with <paramref name="flags"/>.
        /// If perception is null, the check method always returns true.
        /// </summary>
        /// <param name="from">The source state of the transition.</param>
        /// <param name="to">The target state of the transition.</param>
        /// <param name="perception">The perception assigned to the transition.</param>
        /// <param name="flags">The status that <paramref name="from"/> should have to check the transition.</param>
        /// <returns>The transition created.</returns>
        public StateTransition CreateTransition(State from, State to, PerceptionTask? perception = null, StatusFlags flags = StatusFlags.Active)
        {
            var transition = CreateInternalTransition<StateTransition>(from, perception, flags);
            ConnectNodes(transition, to);
            return transition;
        }

        /// <summary>
        /// Create a new exit transition in <paramref name="from"/>.
        /// This transition will check <paramref name="perception"/> every frame if <paramref name="from"/> is the current state and its status matches with <paramref name="flags"/>.
        /// If perception is null, the check method always returns true.
        /// </summary>
        /// <param name="from">The source state of the transition.</param>
        /// <param name="finalStatus">The status of the state machine when the transition was performed.</param>
        /// <param name="perception">The perception assigned to the transition.</param>
        /// <param name="flags">The status that <paramref name="from"/> should have to check the transition.</param>
        /// <returns>The exit transition created.</returns>
        public ExitTransition CreateExitTransition(State from, Status finalStatus, PerceptionTask? perception = null, StatusFlags flags = StatusFlags.Active)
        {
            var transition = CreateInternalTransition<ExitTransition>(from, perception, flags);
            transition.FinalStatus = finalStatus;
            return transition;
        }

        /// <summary>
        /// Create a new probability transition in <paramref name="from"/> that can transit to any state in <paramref name="targetStates"/> .
        /// This transition will check <paramref name="perception"/> every frame if <paramref name="from"/> is the current state and its status matches with <paramref name="flags"/>.
        /// If perception is null, the check method always returns true.
        /// </summary>
        /// <param name="from">The source state of the transition.</param>
        /// <param name="perception">The perception assigned to the transition.</param>
        /// <param name="flags">The status that <paramref name="from"/> should have to check the transition.</param>
        /// <param name="targetStates">The target states for the transition.</param>
        /// <returns>The probability transition created.</returns>
        public ProbabilityTransition CreateProbabilityTransition(State from, PerceptionTask? perception = null, StatusFlags flags = StatusFlags.Active, params State[] targetStates) => CreateProbabilityTransition(from, targetStates, perception, flags);

        /// <summary>
        /// Create a new probability transition in <paramref name="from"/> that can transit to any state in <paramref name="targetStates"/> .
        /// This transition will check <paramref name="perception"/> every frame if <paramref name="from"/> is the current state and its status matches with <paramref name="flags"/>.
        /// If perception is null, the check method always returns true.
        /// </summary>
        /// <param name="from">The source state of the transition.</param>
        /// <param name="perception">The perception assigned to the transition.</param>
        /// <param name="flags">The status that <paramref name="from"/> should have to check the transition.</param>
        /// <param name="targetStates">The target states for the transition.</param>
        /// <returns>The probability transition created.</returns>
        public ProbabilityTransition CreateProbabilityTransition(State from, IEnumerable<State> targetStates, PerceptionTask? perception = null, StatusFlags flags = StatusFlags.Active)
        {
            var transition = CreateInternalTransition<ProbabilityTransition>(from, perception, flags);
            foreach (var child in targetStates)
            {
                ConnectNodes(transition, child);
            }
            return transition;
        }

        /// <summary>
        /// Internal method to create a new transition of type <typeparamref name="T"/> and add it to the machine.
        /// </summary>
        /// <typeparam name="T">The type of the transition created.</typeparam>
        /// <param name="from">The source state of the transition.</param>
        /// <param name="perception">The perception assigned to the transition.</param>
        /// <param name="flags">The status that <paramref name="from"/> should have to check the transition.</param>
        /// <returns>The transition created</returns>
        protected T CreateInternalTransition<T>(State from, PerceptionTask? perception, StatusFlags flags) where T : Transition, new()
        {
            var transition = CreateNode<T>();
            transition.Perception = perception;
            transition.StatusFlags = flags;
            ConnectNodes(from, transition);
            return transition;
        }

        /// <summary>
        /// <inheritdoc/>
        /// Start the entry state.
        /// </summary>
        protected override void OnStarted()
        {
            CurrentState = EntryState;
            CurrentState.Enter();
        }

        /// <summary>
        /// <inheritdoc/>
        /// Update the current state.
        /// </summary>
        protected override void OnUpdated()
        {
            CurrentState.Update();
        }

        /// <summary>
        /// <inheritdoc/>
        /// Stop the current state.
        /// </summary>
        protected override void OnStopped()
        {
            CurrentState.Exit();
            _cachedCurrentState = null;
        }

        /// <summary>
        /// <inheritdoc/>
        /// Pause the current state.
        /// </summary>
        protected override void OnPaused()
        {
            CurrentState.Pause();
        }

        /// <summary>
        /// Change the current state, usually from a transition.
        /// </summary>
        public void ChangeState(State targetState)
        {
            CurrentState.Exit();
            CurrentState = targetState;
            CurrentState.Enter();
        }

        /// <summary>
        /// Change the execution status of the state machine, usually from an exit transition.
        /// </summary>
        /// <param name="finalStatus">The final status of the machine.</param>
        public void FinishExecution(Status finalStatus) => Status = finalStatus;
    }
}
