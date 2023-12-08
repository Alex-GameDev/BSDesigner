using BSDesigner.BehaviourTrees;
using BSDesigner.Core;
using BSDesigner.Core.Exceptions;
using BSDesigner.Core.Tasks;

namespace TestBSD.BehaviourTrees
{
    [TestFixture]
    public class TestCompositeNodes
    {
        [Test]
        public void CompositeNode_NoChildren_ThrowException()
        {
            var bt = new BehaviourTree();
            var leaf1 = bt.CreateLeafNode<ActionBtNode>();
            Assert.That(() => bt.CreateComposite<SequenceNode>(leaf1, leaf1), Throws.InstanceOf<ConnectionException>());
        }

        [Test]
        public void CompositeNode_RepeatedChildren_ThrowException()
        {
            var bt = new BehaviourTree();
            var seq = bt.CreateComposite<SequenceNode>();
            bt.ChangeRootNode(seq);
            Assert.That(bt.Start, Throws.InstanceOf<MissingConnectionException>());
        }

        [Test]
        public void CompositeNode_StopExecution_ResetAllChildren()
        {
            var bt = new BehaviourTree();
            var leaf1 = bt.CreateActionNode(new CustomActionTask() { OnUpdate = () => Status.Success });
            var leaf2 = bt.CreateActionNode(new CustomActionTask() { OnUpdate = () => Status.Success });
            var seq = bt.CreateComposite<SequenceNode>(leaf1, leaf2);
            bt.ChangeRootNode(seq);
            bt.Start();
            bt.Update();
            Assert.That(leaf1.Status, Is.EqualTo(Status.None));
            Assert.That(leaf2.Status, Is.EqualTo(Status.Running));
            bt.Stop();
            bt.Start();
            Assert.That(leaf1.Status, Is.EqualTo(Status.Running));
            Assert.That(leaf2.Status, Is.EqualTo(Status.None));
        }

        [Test]
        [TestCase(Status.Success, Status.Success, Status.Success)]
        [TestCase(Status.Success, Status.Failure, Status.Failure)]
        [TestCase(Status.Failure, Status.Failure, Status.Failure)]
        [TestCase(Status.Failure, Status.Success, Status.Failure)]
        public void SequenceNode_ExecuteChildren_Result(Status firstResult, Status secondResult, Status expectedResult)
        {
            var bt = new BehaviourTree();
            var leaf1 = bt.CreateActionNode(new CustomActionTask() { OnUpdate = () => firstResult });
            var leaf2 = bt.CreateActionNode(new CustomActionTask() { OnUpdate = () => secondResult });
            var seq = bt.CreateComposite<SequenceNode>(leaf1, leaf2);
            bt.ChangeRootNode(seq);
            bt.Start();
            bt.Update();
            bt.Update();
            Assert.That(bt.Status, Is.EqualTo(expectedResult));
        }

        [Test]
        [TestCase(Status.Success, Status.Success, Status.Success)]
        [TestCase(Status.Success, Status.Failure, Status.Success)]
        [TestCase(Status.Failure, Status.Failure, Status.Failure)]
        [TestCase(Status.Failure, Status.Success, Status.Success)]
        public void SelectorNode_Test(Status firstResult, Status secondResult, Status expectedResult)
        {
            var bt = new BehaviourTree();
            var leaf1 = bt.CreateActionNode(new CustomActionTask() { OnUpdate = () => firstResult });
            var leaf2 = bt.CreateActionNode(new CustomActionTask() { OnUpdate = () => secondResult });
            var seq = bt.CreateComposite<SelectorNode>(leaf1, leaf2);
            bt.ChangeRootNode(seq);
            bt.Start();
            bt.Update();
            bt.Update();
            Assert.That(bt.Status, Is.EqualTo(expectedResult));
        }
    }
}