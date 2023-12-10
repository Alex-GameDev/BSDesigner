namespace BSDesigner.StateMachines.StackStateMachines
{
    /// <summary>
    /// Stack Transition that returns to the last state saved in the stack of the stack fsm.
    /// </summary>
    public class PopTransition : StackTransition
    {
        public override int MaxOutputConnections => 0;

        /// <summary>
        /// <inheritdoc/>
        /// Returns to the last state saved in the stack of the stack fsm.
        /// </summary>
        protected override void OnTransitionPerformed() => StackStateMachine.PopState();
    }
}