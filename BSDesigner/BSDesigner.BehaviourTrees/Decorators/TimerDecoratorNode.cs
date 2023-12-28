using System;
using BSDesigner.Core;

namespace BSDesigner.BehaviourTrees
{
    public class TimerDecoratorNode : DecoratorNode
    {
        /// <summary>
        /// The total time that the decorator waits to execute its child.
        /// </summary>
        public float Time;

        /// <summary>
        /// Value used to generate the probabilities.
        /// </summary>
        protected ITimer Timer => _timer ??= new DefaultTimer();
        private ITimer? _timer = new DefaultTimer();

        public override void SetContext(ExecutionContext context)
        {
            if (context.TimerProvider == null)
                throw new NullReferenceException("The execution context has no timer provider.");

            _timer = context.TimerProvider.CreateTimer();
        }

        protected override Status UpdateStatus()
        {
            Timer.Tick();
            if (!Timer.IsTimeout) return Status.Running;

            if(ChildNode.Status == Status.None) ChildNode.Start();

            ChildNode.Update();
            return ChildNode.Status;
        }

        protected override void OnNodeStarted()
        {
            Timer.Start(Time);
        }

        protected override void OnNodeStopped()
        {
            Timer.Stop();

            if (ChildNode.Status != Status.None) ChildNode.Stop();
        }

        protected override void OnNodePaused()
        {
            Timer.Pause();
            if (ChildNode.Status == Status.Running) ChildNode.Pause();
        }
    }
}