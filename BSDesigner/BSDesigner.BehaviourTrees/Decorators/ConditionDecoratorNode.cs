using BSDesigner.Core;
using BSDesigner.Core.Exceptions;

namespace BSDesigner.BehaviourTrees
{
    public class ConditionDecoratorNode : DecoratorNode
    {
        public PerceptionTask? Perception;

        public bool IsReactive = false;

        private bool _lastPerceptionResult;

        protected override Status UpdateStatus()
        {
            if (Perception == null)
                throw new MissingTaskException();

            if (IsReactive)
            {
                _lastPerceptionResult = Perception.Check();
            }

            if (_lastPerceptionResult)
            {
                if (ChildNode.Status == Status.None)
                {
                    ChildNode.Start();
                }
                ChildNode.Update();
                Status = ChildNode.Status;
            }
            else
            {
                if (ChildNode.Status == Status.Running)
                {
                    ChildNode.Stop();
                }
                Status = IsReactive ? Status.Running : Status.Failure;
            }
            return Status;
        }

        protected override void OnNodeStarted()
        {
            if (Perception == null)
                throw new MissingTaskException();

            Perception.Start();
            _lastPerceptionResult = Perception.Check();

            if (_lastPerceptionResult)
            {
                ChildNode.Start();
            }
        }

        protected override void OnNodeStopped()
        {
            if (Perception == null)
                throw new MissingTaskException();

            Perception.Stop();

            if (ChildNode.Status != Status.None)
            {
                ChildNode.Stop();
            }
        }

        protected override void OnNodePaused()
        {
            if (Perception == null)
                throw new MissingTaskException();

            Perception.Pause();
            ChildNode.Pause();
        }
    }
}