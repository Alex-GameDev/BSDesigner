using BSDesigner.Core;
using BSDesigner.Core.Tasks;
using UnityEngine;

namespace BSDesigner.Unity.Runtime
{
    /// <summary>
    /// Action that moves the agent to a determined position.
    /// </summary>
    [TaskCategory("Movement")]
    public class MoveToTargetAction : MovementAction
    {
        /// <summary>
        /// The target to move.
        /// </summary>
        public Transform Target;

        public override string GetInfo() => $"Move agent to {Target.gameObject.name}";

        protected override void OnBeginTask()
        {
            base.OnBeginTask();
            context.Movement.Target = Target.position;
        }

        protected override Status OnUpdateTask() => context.Movement.HasArrivedOnTarget ? Status.Success : Status.Failure;
    }
}