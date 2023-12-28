using BSDesigner.Core;
using System;

namespace BSDesigner.BehaviourTrees
{
    public class RushDecoratorNode : DecoratorNode
    {
        /// <summary>
        /// The total time that the decorator waits to stops its child.
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
            if (Timer.IsTimeout)
            {
                ChildNode.Stop();
                return Status.Failure;
            }
            else
            {
                ChildNode.Update();
                return ChildNode.Status;
            }
        }

        protected override void OnNodeStarted()
        {
            Timer.Start(Time);
            ChildNode.Start();
        }

        protected override void OnNodeStopped()
        {
            Timer.Stop();
            ChildNode.Stop();
        }

        protected override void OnNodePaused()
        {
            Timer.Pause();
            ChildNode.Pause();
        }
    }
}