using BSDesigner.Core;
using UnityEngine;

namespace BSDesigner.Unity.Runtime.Movement
{
    public class PathAction : MovementAction
    {
        /// <summary>
        /// The positions along the path
        /// </summary>
        public Transform[] Positions;

        int currentTargetPosId;

        public override string GetInfo() => $"Move to {currentTargetPosId +1 } / ({Positions.Length}) position.";

        protected override void OnBeginTask()
        {
            base.OnBeginTask();
            currentTargetPosId = 0;
        }

        protected override Status OnUpdateTask()
        {
            if (Positions.Length == 0) return Status.Failure;

            if (context.Movement.HasArrivedOnTarget)
            {
                currentTargetPosId++;

                if (currentTargetPosId >= Positions.Length)
                {
                    return Status.Success;
                }
                context.Movement.Target = Positions[currentTargetPosId].position;
            }

            return Status.Running;

        }
    }
}