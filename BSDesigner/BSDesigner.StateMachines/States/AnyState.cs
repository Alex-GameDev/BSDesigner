using BSDesigner.Core;

namespace BSDesigner.StateMachines
{
    /// <summary>
    /// State that is always active. Its transitions are always checked if the machine is active
    /// </summary>
    public class AnyState : State
    {
        public override int MaxInputConnections => 0;

        protected override void OnEntered() {}

        protected override void OnExited() {}

        protected override void OnPaused() {}

        protected override Status OnUpdated() => Status.Running;
    }
}