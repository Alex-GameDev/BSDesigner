using BSDesigner.Core;
using UnityEngine;

namespace BSDesigner.Unity.Runtime.Movement
{
    /// <summary>
    /// Action that move the agent to a random position around.
    /// </summary>
    public class MoveToRandomPointAction : MovementAction
    {
        /// <summary>
        /// The max distance of the target point.
        /// </summary>
        public float MaxDistance;

        public override string GetInfo() => "Move to a random position around";

        protected override void OnBeginTask()
        {
            base.OnBeginTask();
            var randomMovement = Random.insideUnitCircle * MaxDistance;
            var delta = new Vector3(randomMovement.x, 0, randomMovement.y);
            context.Movement.Target = context.Movement.Transform.position + delta;
        }

        protected override Status OnUpdateTask() => context.Movement.HasArrivedOnTarget ? Status.Success : Status.Failure;
    }
}