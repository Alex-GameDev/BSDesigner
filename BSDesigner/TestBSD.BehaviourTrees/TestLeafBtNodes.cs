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
        public void LeafBTNode_ConnectParent_ThrowException()
        {
            var bt = new BehaviourTree();
            var node1 = bt.CreateActionNode(_action);
            var node2 = bt.CreateActionNode(_action);
            Assert.That(() => bt.ConnectNodes(node1, node2), Throws.InstanceOf<ConnectionException>());
        }

        [Test]
        public void LeafBtNode_NullAction_ThrowException()
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
    }
}