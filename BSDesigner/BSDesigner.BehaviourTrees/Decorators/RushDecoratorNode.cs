using BSDesigner.Core;
using BSDesigner.Core.Utils;

namespace BSDesigner.BehaviourTrees
{
    public class RushDecoratorNode : DecoratorNode
    {
        /// <summary>
        /// The total time that the decorator waits to stops its child.
        /// </summary>
        public Parameter<float> Time = 0f;

        public ITimer Timer = new DefaultTimer();
        protected override Status UpdateStatus()
        {
            Timer.Resume();
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