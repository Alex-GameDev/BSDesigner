using BSDesigner.BehaviourTrees;
using BSDesigner.Core;
using BSDesigner.Core.Exceptions;
using BSDesigner.Core.Actions;
using TestBSD.BehaviourTrees.Mocks;
using ExecutionContext = BSDesigner.Core.ExecutionContext;

namespace TestBSD.BehaviourTrees
{
    [TestFixture]
    public class TestCompositeNodes
    {
        [Test]
        public void CompositeNode_RepeatedChildren_ThrowException()
        {
            var bt = new BehaviourTree();
            var leaf1 = bt.CreateLeafNode<ActionBtNode>();
            Assert.That(() => bt.CreateComposite<SequenceNode>(leaf1, leaf1), Throws.InstanceOf<ConnectionException>());
        }

        [Test]
        public void SerialCompositeNode_NoChildren_ThrowException()
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
        public void SelectorNode_ExecuteCHildren_Result(Status firstResult, Status secondResult, Status expectedResult)
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

        [Test]
        public void BranchSelectionNode_NoChildren_ThrowException()
        {
            var bt = new BehaviourTree();
            bt.CreateComposite<RandomBranchSelectionNode>();
            Assert.That(bt.Start, Throws.InstanceOf<MissingConnectionException>());
        }

        [Test]
        public void FunctionBranchSelectionNode_ValidFunction_LaunchEvents()
        {
            var bt = new BehaviourTree();
            var leaf1 = bt.CreateActionNode(new CustomActionTask() { OnUpdate = () => Status.Success });
            var leaf2 = bt.CreateActionNode(new CustomActionTask() { OnUpdate = () => Status.Success });
            var comp = bt.CreateComposite<FunctionBranchSelectionNode>(leaf1, leaf2);
            comp.NodeIndexFunction = () => 1;
            bt.ChangeRootNode(comp);

            bt.Start();
            bt.Update();
            Assert.That(leaf1.Status, Is.EqualTo(Status.None));
            Assert.That(leaf2.Status, Is.EqualTo(Status.Success));
        }

        [Test]
        public void FunctionBranchSelectionNode_NullFunction_LaunchEventsOnFirstChild()
        {
            var bt = new BehaviourTree();
            var leaf1 = bt.CreateActionNode(new CustomActionTask() { OnUpdate = () => Status.Success });
            var leaf2 = bt.CreateActionNode(new CustomActionTask() { OnUpdate = () => Status.Success });
            var comp = bt.CreateComposite<FunctionBranchSelectionNode>(leaf1, leaf2);
            bt.ChangeRootNode(comp);

            bt.Start();
            bt.Update();
            Assert.That(leaf1.Status, Is.EqualTo(Status.Success));
            Assert.That(leaf2.Status, Is.EqualTo(Status.None));
            bt.Stop();
            Assert.That(leaf1.Status, Is.EqualTo(Status.None));
        }

        [Test]
        [TestCase(-1)]
        [TestCase(2)]
        public void FunctionBranchSelectionNode_NoValidFunction_LaunchEvents(int functionResult)
        {
            var bt = new BehaviourTree();
            var leaf1 = bt.CreateActionNode(new CustomActionTask() { OnUpdate = () => Status.Success });
            var leaf2 = bt.CreateActionNode(new CustomActionTask() { OnUpdate = () => Status.Success });
            var comp = bt.CreateComposite<FunctionBranchSelectionNode>(leaf1, leaf2);
            bt.ChangeRootNode(comp);
            comp.NodeIndexFunction = () => functionResult;

            Assert.That(() => bt.Start(), Throws.InstanceOf<MissingConnectionException>());
        }

        [Test]
        [TestCase(0.5f, 0.5f, 0.3f, 0)]
        [TestCase(0.5f, 0.5f, 0.5f, 1)]
        [TestCase(0.5f, 0.5f, 0.6f, 1)]
        public void RandomBranchSelectionNode_Probabilities_LaunchEvents(float prob1, float prob2, float randomValue, int expectedSelectedBranch)
        {
            var bt = new BehaviourTree();
            var leaf1 = bt.CreateActionNode(new CustomActionTask() { OnUpdate = () => Status.Success });
            var leaf2 = bt.CreateActionNode(new CustomActionTask() { OnUpdate = () => Status.Success });
            var comp = bt.CreateComposite<RandomBranchSelectionNode>(leaf1, leaf2);

            var context = new ExecutionContext
            {
                RandomProvider = new MockedRandom()
                {
                    DoubleValue = randomValue
                }
            };
            bt.SetContext(context);

            comp.Probabilities[leaf1] = prob1;
            comp.Probabilities[leaf2] = prob2;
            bt.ChangeRootNode(comp);
            bt.Start();
            Assert.That(leaf1.Status, Is.EqualTo(expectedSelectedBranch == 0 ? Status.Running : Status.None));
            Assert.That(leaf2.Status, Is.EqualTo(expectedSelectedBranch == 1 ? Status.Running : Status.None));
        }

        [Test]
        [TestCase(0)]
        [TestCase(1)]
        public void RandomBranchSelectionNode_NoProbabilities_LaunchEvents(int expectedSelectedBranch)
        {
            var bt = new BehaviourTree();
            var leaf1 = bt.CreateActionNode(new CustomActionTask() { OnUpdate = () => Status.Success });
            var leaf2 = bt.CreateActionNode(new CustomActionTask() { OnUpdate = () => Status.Success });
            var comp = bt.CreateComposite<RandomBranchSelectionNode>(leaf1, leaf2);
            var context = new ExecutionContext
            {
                RandomProvider = new MockedRandom()
                {
                    IntValue = expectedSelectedBranch
                }
            };
            bt.SetContext(context);
            bt.ChangeRootNode(comp);
            bt.Start();
            Assert.That(leaf1.Status, Is.EqualTo(expectedSelectedBranch == 0 ? Status.Running : Status.None));
            Assert.That(leaf2.Status, Is.EqualTo(expectedSelectedBranch == 1 ? Status.Running : Status.None));
        }

        [Test]
        [TestCase(true, true, Status.Success, Status.Running, Status.Success)]
        [TestCase(true, true, Status.Failure, Status.Running, Status.Failure)]
        [TestCase(true, true, Status.Running, Status.Running, Status.Running)]
        [TestCase(true, false, Status.Running, Status.Success, Status.Success)]
        [TestCase(true, false, Status.Running, Status.Failure, Status.Running)]
        [TestCase(true, false, Status.Failure, Status.Failure, Status.Failure)]
        [TestCase(false, true, Status.Running, Status.Failure, Status.Failure)]
        [TestCase(false, true, Status.Running, Status.Success, Status.Running)]
        [TestCase(false, true, Status.Success, Status.Success, Status.Success)]
        [TestCase(false, false, Status.Success, Status.Success, Status.Success)]
        [TestCase(false, false, Status.Success, Status.Running, Status.Running)]
        [TestCase(false, false, Status.Running, Status.Running, Status.Running)]
        [TestCase(false, false, Status.Success, Status.Failure, Status.Failure)]
        [TestCase(false, false, Status.Failure, Status.Success, Status.Success)]
        public void ParallelCompositeNode_ConfigureFlags_CorrectResult(bool finishAnySuccess, bool finishAnyFailure, Status firstResult, Status secondResult, Status expectedResult)
        {
            var bt = new BehaviourTree();
            var leaf1 = bt.CreateActionNode(new CustomActionTask() { OnUpdate = () => firstResult});
            var leaf2 = bt.CreateActionNode(new CustomActionTask() { OnUpdate = () => secondResult});
            var comp = bt.CreateComposite<ParallelCompositeNode>(leaf1, leaf2);
            comp.finishOnAnySuccess = finishAnySuccess;
            comp.finishOnAnyFailure = finishAnyFailure;
            bt.ChangeRootNode(comp);
            bt.Start();
            bt.Update();
            Assert.That(bt.Status, Is.EqualTo(expectedResult));
        }

        [Test]
        public void ParallelCompositeNode_Execute_LaunchEvents()
        {
            var pauseCount = 0;
            var bt = new BehaviourTree();
            var leaf1 = bt.CreateActionNode(new CustomActionTask() { OnUpdate = () => Status.Running, OnPause = () => pauseCount++, OnResume = () => pauseCount-- });
            var leaf2 = bt.CreateActionNode(new CustomActionTask() { OnUpdate = () => Status.Success, OnPause = () => pauseCount++, OnResume = () => pauseCount-- });
            var comp = bt.CreateComposite<ParallelCompositeNode>(leaf1, leaf2);
            bt.ChangeRootNode(comp);
            bt.Start();
            bt.Pause();
            Assert.That(pauseCount, Is.EqualTo(2));
            bt.Update();
            Assert.That(pauseCount, Is.EqualTo(0));
            bt.Stop();
            Assert.That(leaf1.Status, Is.EqualTo(Status.None));
            Assert.That(leaf2.Status, Is.EqualTo(Status.None));
        }
    }
}