using BSDesigner.BehaviourTrees;
using BSDesigner.Core;
using BSDesigner.Core.Exceptions;
using BSDesigner.Core.Tasks;
using TestBSD.BehaviourTrees.Mocks;
using ExecutionContext = BSDesigner.Core.ExecutionContext;

namespace TestBSD.BehaviourTrees
{
    [TestFixture]
    public class TestDecoratorNodes
    {
        [Test]
        public void DecoratorNode_ConnectMultipleChildren_ThrowException()
        {
            var bt = new BehaviourTree();
            var node1 = bt.CreateLeafNode<ActionBtNode>();
            var node2 = bt.CreateLeafNode<ActionBtNode>();
            var dec = bt.CreateDecorator<InverterNode>(node1);
            Assert.That(() => bt.ConnectNodes(dec, node2), Throws.InstanceOf<ConnectionException>());
        }

        [Test]
        public void DecoratorNode_ExecuteWithoutChild_ThrowException()
        {
            var bt = new BehaviourTree();
            bt.AddNode(new SuccederNode());
            Assert.That(() => bt.Start(), Throws.InstanceOf<MissingConnectionException>());
        }

        [Test]
        public void SuccederNode_Execute_ReturnCorrectValue()
        {
            var status = Status.Running;
            var bt = new BehaviourTree();
            var leaf = bt.CreateActionNode(new CustomActionTask { OnUpdate = () => status });
            var decorator = bt.CreateDecorator<SuccederNode>(leaf);
            bt.ChangeRootNode(decorator);

            bt.Start();
            bt.Update();
            Assert.That(bt.Status, Is.EqualTo(Status.Running));

            status = Status.Success;
            bt.Update();
            Assert.That(bt.Status, Is.EqualTo(Status.Success));

            bt.Stop();
            bt.Start();
            status = Status.Failure;
            bt.Update();
            Assert.That(bt.Status, Is.EqualTo(Status.Success));
        }

        [Test]
        public void InverterNode_Execute_ReturnCorrectValue()
        {
            var status = Status.Running;
            var bt = new BehaviourTree();
            var leaf = bt.CreateActionNode(new CustomActionTask { OnUpdate = () => status });
            var decorator = bt.CreateDecorator<InverterNode>(leaf);
            bt.ChangeRootNode(decorator);

            bt.Start();
            bt.Update();
            Assert.That(bt.Status, Is.EqualTo(Status.Running));

            status = Status.Success;
            bt.Update();
            Assert.That(bt.Status, Is.EqualTo(Status.Failure));

            bt.Stop();
            bt.Start();
            status = Status.Failure;
            bt.Update();
            Assert.That(bt.Status, Is.EqualTo(Status.Success));
        }

        [Test]
        public void LoopNode_Execute_ReturnCorrectValue()
        {
            var bt = new BehaviourTree();
            var leaf = bt.CreateActionNode(new CustomActionTask { OnUpdate = () => Status.Success });
            var decorator = bt.CreateDecorator<LoopNode>(leaf);
            bt.ChangeRootNode(decorator);

            decorator.Iterations = -1;
            bt.Start();
            bt.Update();
            Assert.That(bt.Status, Is.EqualTo(Status.Running));
            bt.Stop();

            decorator.Iterations = 1;
            bt.Start();
            bt.Update();
            Assert.That(bt.Status, Is.EqualTo(Status.Success));
        }

        [Test]
        [TestCase(Status.Success, Status.Success, -1, Status.Success)]
        [TestCase(Status.Success, Status.Failure, -1, Status.Running)]
        [TestCase(Status.Success, Status.Failure, +1, Status.Success)]
        [TestCase(Status.Failure, Status.Failure, +1, Status.Failure)]
        public void LoopUntilNode_Execute_ReturnCorrectValue(Status returnedStatus, Status targetStatus, int maxIterations, Status expectedStatus)
        {
            var bt = new BehaviourTree();
            var leaf = bt.CreateActionNode(new CustomActionTask { OnUpdate = () => returnedStatus });
            var decorator = bt.CreateDecorator<LoopUntilNode>(leaf);
            bt.ChangeRootNode(decorator);

            decorator.MaxIterations = maxIterations;
            decorator.TargetStatus = targetStatus;
            bt.Start();
            bt.Update();
            Assert.That(bt.Status, Is.EqualTo(expectedStatus));
        }

        [Test]
        public void ConditionDecoratorNode_PerceptionIsTrue_ExecuteChild()
        {
            var bt = new BehaviourTree();
            var leaf = bt.CreateActionNode(new CustomActionTask() { OnUpdate = () => Status.Success });
            var dec = bt.CreateDecorator<ConditionDecoratorNode>(leaf);
            dec.Perception = new CustomPerceptionTask() { OnCheck = () => true };
            bt.ChangeRootNode(dec);

            bt.Start();
            Assert.That(leaf.Status, Is.EqualTo(Status.Running));
            bt.Update();
            Assert.That(bt.Status, Is.EqualTo(Status.Success));
        }

        [Test]
        public void ConditionDecoratorNode_PerceptionIsFalse_IgnoreChild()
        {
            var bt = new BehaviourTree();
            var leaf = bt.CreateActionNode(new CustomActionTask() { OnUpdate = () => Status.Success });
            var dec = bt.CreateDecorator<ConditionDecoratorNode>(leaf);
            dec.Perception = new CustomPerceptionTask() { OnCheck = () => false };
            bt.ChangeRootNode(dec);

            bt.Start();
            Assert.That(leaf.Status, Is.EqualTo(Status.None));
            bt.Update();
            Assert.That(bt.Status, Is.EqualTo(Status.Failure));
            Assert.That(leaf.Status, Is.EqualTo(Status.None));
        }

        [Test]
        public void ConditionDecoratorNode_NullPerception_ThrowException()
        {
            var bt = new BehaviourTree();
            var leaf = bt.CreateActionNode(new CustomActionTask() { OnUpdate = () => Status.Success });
            var dec = bt.CreateDecorator<ConditionDecoratorNode>(leaf);
            bt.ChangeRootNode(dec);

            Assert.That(() => bt.Start(), Throws.InstanceOf<MissingTaskException>());
        }

        [Test]
        [TestCase(Status.Running, false, Status.None, Status.Running)]
        [TestCase(Status.Success, false, Status.None, Status.Running)]
        [TestCase(Status.Running, true, Status.Running, Status.Running)]
        [TestCase(Status.Success, true, Status.Success, Status.Success)]
        public void ReactiveConditionDecoratorNode_Execute_LaunchChildDependingOnPerception(Status childResult, bool perceptionValue, Status expectedChildResult, Status expectedTreeResult)
        {
            var bt = new BehaviourTree();
            var leaf = bt.CreateActionNode(new CustomActionTask { OnUpdate = () => childResult });
            var dec = bt.CreateDecorator<ConditionDecoratorNode>(leaf);
            dec.Perception = new CustomPerceptionTask() { OnCheck = () => perceptionValue };
            dec.IsReactive = true;
            bt.ChangeRootNode(dec);

            bt.Start();
            bt.Update();
            Assert.That(bt.Status, Is.EqualTo(expectedTreeResult));
            Assert.That(leaf.Status, Is.EqualTo(expectedChildResult));
        }

        [Test]
        public void ReactiveConditionDecoratorNode_ChangePerception_LaunchEvents()
        {
            var childResult = Status.Running;
            var perceptionValue = false;
            var bt = new BehaviourTree();
            var leaf = bt.CreateActionNode(new CustomActionTask { OnUpdate = () => childResult });
            var dec = bt.CreateDecorator<ConditionDecoratorNode>(leaf);
            dec.Perception = new CustomPerceptionTask() { OnCheck = () => perceptionValue };
            dec.IsReactive = true;
            bt.ChangeRootNode(dec);

            bt.Start();
            Assert.That(leaf.Status, Is.EqualTo(Status.None));

            bt.Update();
            Assert.That(bt.Status, Is.EqualTo(Status.Running));
            Assert.That(leaf.Status, Is.EqualTo(Status.None));

            perceptionValue = true;
            bt.Update();
            Assert.That(bt.Status, Is.EqualTo(Status.Running));
            Assert.That(leaf.Status, Is.EqualTo(Status.Running));

            perceptionValue = false;
            bt.Update();
            Assert.That(bt.Status, Is.EqualTo(Status.Running));
            Assert.That(leaf.Status, Is.EqualTo(Status.None));

            perceptionValue = true;
            childResult = Status.Success;
            bt.Update();
            Assert.That(bt.Status, Is.EqualTo(Status.Success));
            Assert.That(leaf.Status, Is.EqualTo(Status.Success));
        }

        [Test]
        public void ReactiveConditionDecoratorNode_Reset_LaunchEvents()
        {
            var childResult = Status.Success;
            var perceptionValue = true;
            var bt = new BehaviourTree();
            var leaf = bt.CreateActionNode(new CustomActionTask { OnUpdate = () => childResult });
            var dec = bt.CreateDecorator<ConditionDecoratorNode>(leaf);
            dec.Perception = new CustomPerceptionTask() { OnCheck = () => perceptionValue };
            dec.IsReactive = true;
            bt.ChangeRootNode(dec);

            bt.Start();
            bt.Update();
            Assert.That(bt.Status, Is.EqualTo(Status.Success));
            Assert.That(leaf.Status, Is.EqualTo(Status.Success));

            bt.Stop();
            perceptionValue = false;
            bt.Start();
            bt.Update();
            Assert.That(bt.Status, Is.EqualTo(Status.Running));
            Assert.That(leaf.Status, Is.EqualTo(Status.None));
        }

        [Test]
        [TestCase(true, true)]
        [TestCase(false, false)]
        public void ReactiveConditionDecoratorNode_Pause_PropagateIfIsActive(bool perceptionResult, bool childWillPause)
        {
            var pauseFlag = false;
            var childResult = Status.Success;
            var perceptionValue = true;
            var bt = new BehaviourTree();
            var leaf = bt.CreateActionNode(new CustomActionTask { OnUpdate = () => childResult, OnPause = () => pauseFlag = true });
            var dec = bt.CreateDecorator<ConditionDecoratorNode>(leaf);
            dec.Perception = new CustomPerceptionTask() { OnCheck = () => perceptionResult };
            dec.IsReactive = true;
            bt.ChangeRootNode(dec);

            bt.Start();
            bt.Pause();
            Assert.That(bt.IsPaused, Is.True);
            Assert.That(pauseFlag, Is.EqualTo(childWillPause));
        }

        [Test]
        public void TimerDecoratorNode_test()
        {
            var childResult = Status.Running;
            var pauseFlag = false;
            var bt = new BehaviourTree();
            var leaf = bt.CreateActionNode(new CustomActionTask { OnUpdate = () => childResult, OnPause = () => pauseFlag = true });
            var dec = bt.CreateDecorator<TimerDecoratorNode>(leaf);
            dec.Time = 1f;
            var timer = new MockedTimerProvider();

            var context = new ExecutionContext
            {
                TimerProvider = timer
            };
            bt.SetContext(context);

            bt.ChangeRootNode(dec);

            bt.Start();
            bt.Update();
            Assert.That(dec.Status, Is.EqualTo(Status.Running));
            Assert.That(leaf.Status, Is.EqualTo(Status.None));
            bt.Pause();
            Assert.That(pauseFlag, Is.False);

            timer.CurrentTime = 1f;
            bt.Update();
            Assert.That(dec.Status, Is.EqualTo(Status.Running));
            Assert.That(leaf.Status, Is.EqualTo(Status.Running));
            bt.Pause();
            Assert.That(pauseFlag, Is.True);
            childResult = Status.Success;
            bt.Update();
            Assert.That(dec.Status, Is.EqualTo(Status.Success));
            Assert.That(leaf.Status, Is.EqualTo(Status.Success));
            bt.Stop();
            Assert.That(dec.Status, Is.EqualTo(Status.None));
            Assert.That(leaf.Status, Is.EqualTo(Status.None));
        }

        [Test]
        public void RushDecoratorNode_ChildFinishBeforeTimeout_ChildResults()
        {
            var childResult = Status.Running;
            bool pauseFlag = false;
            var bt = new BehaviourTree();
            var leaf = bt.CreateActionNode(new CustomActionTask { OnUpdate = () => childResult, OnPause = () => pauseFlag = true });
            var dec = bt.CreateDecorator<RushDecoratorNode>(leaf);
            
            dec.Time = 1f;
            var timer = new MockedTimerProvider();
            var context = new ExecutionContext
            {
                TimerProvider = timer
            };
            bt.SetContext(context);

            bt.ChangeRootNode(dec);

            bt.Start();
            bt.Update();
            Assert.That(dec.Status, Is.EqualTo(Status.Running));
            Assert.That(leaf.Status, Is.EqualTo(Status.Running));
            bt.Pause();
            Assert.That(pauseFlag, Is.True);
            childResult = Status.Success;
            bt.Update();
            Assert.That(dec.Status, Is.EqualTo(Status.Success));
            Assert.That(leaf.Status, Is.EqualTo(Status.Success));
            bt.Stop();
            Assert.That(dec.Status, Is.EqualTo(Status.None));
            Assert.That(leaf.Status, Is.EqualTo(Status.None));
        }

        [Test]
        public void RushDecoratorNode_Timeout_ReturnFailure()
        {
            var childResult = Status.Running;
            var pauseFlag = false;
            var bt = new BehaviourTree();
            var leaf = bt.CreateActionNode(new CustomActionTask { OnUpdate = () => childResult, OnPause = () => pauseFlag = true });
            var dec = bt.CreateDecorator<RushDecoratorNode>(leaf);
            var timerProvider = new MockedTimerProvider();

            var context = new ExecutionContext
            {
                TimerProvider = timerProvider
            };
            bt.SetContext(context);
            dec.Time = 1f;
            bt.ChangeRootNode(dec);

            bt.Start();
            timerProvider.CurrentTime = 2f;
            bt.Update();
            Assert.That(dec.Status, Is.EqualTo(Status.Failure));
            Assert.That(leaf.Status, Is.EqualTo(Status.None));
        }
    }
}