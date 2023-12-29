using BSDesigner.Core;
using BSDesigner.Core.Tasks;
using UnityEngine;

namespace BSDesigner.Unity.Runtime
{
    /// <summary>
    /// Action that moves an agent away from a transform, returning success when the position is arrived.
    /// </summary>
    [TaskCategory("Movement")]
    public class FleeAction : MovementAction
    {
        /// <summary>
        /// The transform that the agent chase.
        /// </summary>
        public Transform target;

        /// <summary>
        /// The distance of the target point.
        /// </summary>
        public float MinDistance;

        /// <summary>
        /// The maximum time the agent will run.
        /// </summary>
        public float MaxTime;

        private ITimer _timer;

        protected override void OnSetContext()
        {
            _timer = context.TimerProvider?.CreateTimer();
        }

        public override string GetInfo() => $"Flee from {target.gameObject.name}";

        protected override void OnBeginTask()
        {
            base.OnBeginTask() ;
            _timer.Start(MaxTime);
        }

        protected override void OnEndTask()
        {
            base.OnEndTask();
            _timer.Stop();
        }

        protected override void OnPauseTask()
        {
            base.OnPauseTask();
            _timer.Pause();
        }

        protected override Status OnUpdateTask()
        {
            _timer.Tick();
            if (_timer.IsTimeout) return Status.Failure;

            var dir = (context.Transform.position - target.position).normalized;
            context.Movement.Target = context.Transform.position + dir;

            return Vector3.Distance(target.position, context.Transform.position) > MinDistance ? Status.Success : Status.Running;
        }
    }
}