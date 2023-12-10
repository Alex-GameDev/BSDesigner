using BSDesigner.Core;

namespace BSDesigner.StateMachines
{
    /// <summary>
    /// Transitions that finish the execution of the state machine when is triggered
    /// </summary>
    public class ExitTransition : Transition
    {
        public override int MaxOutputConnections => 0;

        /// <summary>
        /// The status that the state machine will have when this transition is performed.
        /// </summary>
        public Status FinalStatus;

        /// <summary>
        /// <inheritdoc/>
        /// Finish the execution of the state machine with the value specified in <see cref="FinalStatus"/>
        /// </summary>
        protected override void OnTransitionPerformed() => StateMachine.FinishExecution(FinalStatus);
    }
}
