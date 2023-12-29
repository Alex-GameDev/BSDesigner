using BSDesigner.Core;
using UnityEngine;

namespace BSDesigner.Unity.Runtime.Movement
{
    public class ChaseAction : MovementAction
    {
        /// <summary>
        /// The transform that the agent chase.
        /// </summary>
        public Transform target;

        /// <summary>
        /// The distance that the agent must be to its target to end with success.
        /// </summary>
        public float maxDistance;

        /// <summary>
        /// The max time that the agent will chase.
        /// </summary>
        public float maxTime;

        private ITimer _timer;

        protected override void OnSetContext()
        {
            _timer = context.TimerProvider?.CreateTimer();
        }

        public override string GetInfo() => $"Chase {target.gameObject.name}";

        protected override void OnBeginTask()
        {
            base.OnBeginTask();
            _timer.Start(maxTime);
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

            context.Movement.Target = target.position;

            return Vector3.Distance(target.position, context.Transform.position) < maxDistance ? Status.Success : Status.Running;
        }
    }
}