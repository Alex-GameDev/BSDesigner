using BSDesigner.Core;
using BSDesigner.Core.Utils;

namespace BSDesigner.BehaviourTrees
{
    public class TimerDecoratorNode : DecoratorNode
    {
        /// <summary>
        /// The total time that the decorator waits to execute its child.
        /// </summary>
        public float Time;

        public ITimer Timer = new DefaultTimer();

        protected override Status UpdateStatus()
        {
            Timer.Resume();

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