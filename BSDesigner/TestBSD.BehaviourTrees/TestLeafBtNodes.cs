using BSDesigner.BehaviourTrees;
using BSDesigner.Core;
using BSDesigner.Core.Exceptions;
using BSDesigner.Core.Tasks;

namespace TestBSD.BehaviourTrees
{
    [TestFixture]
    public class Tests
    {
        private ActionTask _action = null!;
        private PerceptionTask _perception = null!;
        private string _lastEvent = string.Empty;

        [SetUp]
        public void Setup()
        {
            _action = new CustomActionTask
            {
                OnBegin = () => _lastEvent = "BEGIN",
                OnUpdate = () => { _lastEvent = "UPDATE"; return Status.Running; },
                OnEnd = () => _lastEvent = "END",
                OnPause = () => _lastEvent = "PAUSE",
                OnResume = () => _lastEvent = "RESUME"
            };

            _perception = new CustomPerceptionTask
            {
                OnBegin = () => _lastEvent = "BEGIN",
                OnCheck = () => { _lastEvent = "CHECK"; return false; },
                OnEnd = () => _lastEvent = "END",
                OnPause = () => _lastEvent = "PAUSE",
                OnResume = () => _lastEvent = "RESUME"
            };
        }

        [Test]
        public void BTNode_StartWhenIsAlreadyRunning_ThrowException()
        {
            var bt = new BehaviourTree();
            bt.CreateActionNode(_action);
            bt.Start();
            Assert.That(bt.Start, Throws.InstanceOf<ExecutionStatusException>());
        }

        [Test]
        public void BTNode_StopWhenIsNotRunning_ThrowException()
        {
            var bt = new BehaviourTree();
            bt.CreateActionNode(_action);
            Assert.That(bt.Stop, Throws.InstanceOf<ExecutionStatusException>());
        }

        [Test]
        public void BTNode_UpdateWhenIsFinished_NotChangeStatus()
        {
            Status returnedStatus = Status.Success;
            var bt = new BehaviourTree();
            bt.CreateActionNode(new CustomActionTask() { OnUpdate = () => returnedStatus});
            bt.Start();
            bt.Update();
            returnedStatus = Status.Failure;
            bt.Update();
            Assert.That(bt.Status, Is.EqualTo(Status.Success));
        }

        [Test]
        public void LeafBTNode_ConnectChild_ThrowException()
        {
            var bt = new BehaviourTree();
            var node1 = bt.CreateActionNode(_action);
            var node2 = bt.CreateActionNode(_action);
            Assert.That(() => bt.ConnectNodes(node1, node2), Throws.InstanceOf<ConnectionException>());
        }

        [Test]
        public void LeafBTNode_ConnectMultipleParents_ThrowException()
        {
            var bt = new BehaviourTree();
            var node1 = bt.CreateActionNode(_action);
            var node2 = bt.CreateDecorator<InverterNode>(node1);
            Assert.That(() => bt.CreateDecorator<InverterNode>(node1), Throws.InstanceOf<ConnectionException>());
        }

        [Test]
        public void ActionBtNode_NullAction_ThrowException()
        {
            var bt = new BehaviourTree();
            bt.CreateLeafNode<ActionBtNode>();
            Assert.That(() => bt.Start(), Throws.InstanceOf<MissingTaskException>());
        }

        [Test]
        public void ActionBTNode_AssignAction_LaunchEvents()
        {
            var bt = new BehaviourTree();
            bt.CreateActionNode(_action);
            bt.Start();
            Assert.That(_lastEvent, Is.EqualTo("BEGIN"));
            bt.Pause();
            Assert.That(_lastEvent, Is.EqualTo("PAUSE"));
            bt.Update();
            Assert.That(_lastEvent, Is.EqualTo("UPDATE"));
            bt.Stop();
            Assert.That(_lastEvent, Is.EqualTo("END"));
        }

        [Test]
        public void LeafBtNode_NullAction_ThrowException()
        {
            var bt = new BehaviourTree();
            bt.CreateLeafNode<PerceptionBtNode>();
            Assert.That(() => bt.Start(), Throws.InstanceOf<MissingTaskException>());
        }

        [Test]
        public void PerceptionBtNode_AssignPerception_LaunchEvents()
        {
            var bt = new BehaviourTree();
            bt.CreatePerceptionNode(_perception);
            bt.Start();
            Assert.That(_lastEvent, Is.EqualTo("BEGIN"));
            bt.Pause();
            Assert.That(_lastEvent, Is.EqualTo("PAUSE"));
            bt.Update();
            Assert.That(_lastEvent, Is.EqualTo("CHECK"));
            bt.Stop();
            Assert.That(_lastEvent, Is.EqualTo("END"));
        }

        [Test]
        [TestCase(Status.Success, Status.Failure, false, Status.Failure)]
        [TestCase(Status.Success, Status.Failure, true, Status.Success)]
        [TestCase(Status.Success, Status.Running, false, Status.Running)]
        [TestCase(Status.Success, Status.Running, true, Status.Success)]
        [TestCase(Status.Running, Status.Failure, false, Status.Failure)]
        [TestCase(Status.Running, Status.Failure, true, Status.Running)]
        public void PerceptionBtNode_BoolToStatus_ReturnCorrectStatus(Status trueValue, Status falseValue, bool perceptionResult, Status expectedResult)
        {
            var bt = new BehaviourTree();
            var leaf = bt.CreatePerceptionNode(new CustomPerceptionTask() { OnCheck = () => perceptionResult});
            leaf.ValueOnTrue = trueValue;
            leaf.ValueOnFalse = falseValue;
            bt.Start();
            bt.Update();
            Assert.That(bt.Status, Is.EqualTo(expectedResult));
        }

        [Test]
        public void PerceptionBtNode_NotValidResult_ThrowException()
        {
            var bt = new BehaviourTree();
            var leaf = bt.CreatePerceptionNode(_perception);
            leaf.ValueOnFalse = Status.None;
            bt.Start();

            Assert.That(() => bt.Update(), Throws.InstanceOf<ExecutionStatusException>());
        }
    }
}