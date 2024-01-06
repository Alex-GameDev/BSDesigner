using System;
using System.Collections.Generic;
using System.Linq;
using BSDesigner.Core;

namespace BSDesigner.StateMachines
{
    /// <summary>
    /// Represents a state in a FSM graph.
    /// </summary>
    public abstract class State : FsmNode, IStatusHandler
    {
        public override Type ChildType => typeof(Transition);
        public override int MaxOutputConnections => -1;

        /// <summary>
        /// The execution status of the state.
        /// </summary>
        public Status Status
        {
            get => _status;
            protected set
            {
                if (_status == value) return;
                _status = value;
                StatusChanged?.Invoke(_status);
            }
        }

        private Status _status;

        /// <summary>
        /// Event called when current status changed.
        /// </summary>
        public event Action<Status> StatusChanged = delegate { };

        /// <summary>
        /// The transitions that have this state as source or parent.
        /// </summary>
        protected IEnumerable<Transition> Transitions
        {
            get
            {
                _cachedChildren ??= Children.Cast<Transition>().ToList();
                return _cachedChildren;
            }
        }

        private List<Transition>? _cachedChildren;

        /// <summary>
        /// Starts the execution of the state change the status to Running and starting the transitions.
        /// </summary>
        public void Enter()
        {
            Status = Status.Running;
            OnEntered();
            foreach (var transition in Transitions)
            {
                transition.Init();
            }
        }

        /// <summary>
        /// Stops the execution of the state changing its status to None and stopping the transitions.
        /// </summary>
        public void Exit()
        {
            Status = Status.None;
            OnExited();
            foreach (var transition in Transitions)
            {
                transition.Reset();
            }
        }

        /// <summary>
        /// Update the execution of the state and checks the transitions.
        /// </summary>
        public void Update()
        {
            Status = OnUpdated();
            foreach (var transition in Transitions)
            {
                if (((uint)Status & (uint)transition.StatusFlags.Value) != 0 && transition.Check())
                {
                    transition.Perform();
                    break;
                }
            }
        }

        /// <summary>
        /// Pauses the execution of the states and the transitions
        /// </summary>
        public void Pause()
        {
            OnPaused();
            foreach (var transition in Transitions)
            {
                transition.Pause();
            }
        }

        /// <summary>
        /// Method called when the state machine execution enters in this state.
        /// </summary>
        protected abstract void OnEntered();

        /// <summary>
        /// Method called when the state machine execution exits from this state.
        /// </summary>
        protected abstract void OnExited();

        /// <summary>
        /// Method called when the state machine execution pauses and its in this state.
        /// </summary>
        protected abstract void OnPaused();

        /// <summary>
        /// Method called when the state machine execution updates and its in this state.
        /// </summary>
        /// <returns>The execution status of the state.</returns>
        protected abstract Status OnUpdated();

    }
}