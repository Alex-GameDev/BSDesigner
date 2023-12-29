using BSDesigner.Core;
using BSDesigner.Core.Tasks;
using UnityEngine;

namespace BSDesigner.Unity.Runtime
{
    /// <summary>
    /// Action that moves the agent to a determined position.
    /// </summary>
    [TaskCategory("Movement")]
    public class MoveToPointAction : MovementAction
    {
        /// <summary>
        /// The point to move.
        /// </summary>
        public Vector3 Point;
        
        public override string GetInfo() => $"Move agent to {Point}";

        protected override void OnBeginTask()
        {
            base.OnBeginTask();
            context.Movement.Target = Point;
        }

        protected override Status OnUpdateTask() => context.Movement.HasArrivedOnTarget ? Status.Success : Status.Failure;
    }
}