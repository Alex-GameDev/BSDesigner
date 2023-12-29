using BSDesigner.Core;
using UnityEngine;

namespace BSDesigner.Unity.Runtime.Movement
{
    /// <summary>
    /// Action that moves the agent to a determined position.
    /// </summary>
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