using BSDesigner.Core;
using BSDesigner.Core.Exceptions;
using BSDesigner.StateMachines;
using BSDesigner.StateMachines.StackStateMachines;

namespace TestBSD.StateMachines
{
    [TestFixture]
    public class TestStackStateMachines
    {
        [Test]
        public void StackTransitions_Perform_ModifyStack()
        {
            var stackFsm = new StackStateMachine();
            var state1 = stackFsm.CreateState<ActionState>();
            var state2 = stackFsm.CreateState<ActionState>();
            stackFsm.CreatePushTransition(state1, state2);
            stackFsm.CreatePopTransition(state2);

            stackFsm.Start();
            Assert.That(stackFsm.Stack, Has.Count.EqualTo(0));
            Assert.That(state1.Status, Is.EqualTo(Status.Running));
            stackFsm.Update();
            Assert.That(stackFsm.Stack, Has.Count.EqualTo(1));
            Assert.That(stackFsm.Stack.Contains(state1), Is.True);
            Assert.That(state2.Status, Is.EqualTo(Status.Running));
            stackFsm.Update();
            Assert.That(stackFsm.Stack, Has.Count.EqualTo(0));
            Assert.That(state1.Status, Is.EqualTo(Status.Running));
        }

        [Test]
        public void PopTransitions_PerformWhenStackIsEmpty_NotChangeState()
        {
            var stackFsm = new StackStateMachine();
            var state1 = stackFsm.CreateState<ActionState>();
            stackFsm.CreatePopTransition(state1);

            stackFsm.Start();
            stackFsm.Update();
            Assert.That(stackFsm.Stack, Has.Count.EqualTo(0));
        }

        [Test]
        public void PopTransitions_ConnectTarget_ThrowException()
        {
            var stackFsm = new StackStateMachine();
            var state1 = stackFsm.CreateState<ActionState>();
            var state2 = stackFsm.CreateState<ActionState>();
            var transition = stackFsm.CreatePopTransition(state1);

            stackFsm.Start();
            stackFsm.Update();
            Assert.That(() => stackFsm.ConnectNodes(transition, state2), Throws.InstanceOf<ConnectionException>());
        }
    }
}
