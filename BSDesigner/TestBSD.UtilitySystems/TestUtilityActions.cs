using BSDesigner.Core;
using BSDesigner.Core.Exceptions;
using BSDesigner.Core.Actions;
using BSDesigner.UtilitySystems;

namespace TestBSD.UtilitySystems
{
    [TestFixture]
    public class TestUtilityActions
    {
        [Test]
        public void UtilityAction_NoFactor_ThrowException()
        {
            var us = new UtilitySystem();
            var action1 = new UtilityAction();
            us.AddNode(action1);
            Assert.That(us.Start, Throws.InstanceOf<MissingConnectionException>());
        }

        [Test]
        public void UtilityAction_FinishOnComplete_FinishSystem()
        {
            var us = new UtilitySystem();
            var factor = us.CreateConstantLeaf(1f);
            us.CreateAction(factor, new CustomActionTask{ OnUpdate = () => Status.Success}, finishOnComplete: true);
            us.Start();
            us.Update();
            Assert.That(() => us.Status, Is.EqualTo(Status.Success));
        }

        [Test]
        public void UtilityAction_ExecuteOnLoop_KeepStatusInRunning()
        {
            var us = new UtilitySystem();
            var factor = us.CreateConstantLeaf(1f);
            var action = us.CreateAction(factor, new CustomActionTask { OnUpdate = () => Status.Success }, executeOnLoop: true);
            us.Start();
            us.Update();
            Assert.That(() => action.Status, Is.EqualTo(Status.Running));
        }

        [Test]
        public void UtilityAction_ExecuteOnLoopAndFinishOnComplete_KeepStatusInRunning()
        {
            var us = new UtilitySystem();
            var factor = us.CreateConstantLeaf(1f);
            var action = us.CreateAction(factor, new CustomActionTask { OnUpdate = () => Status.Success }, executeOnLoop: true, finishOnComplete: true);
            us.Start();
            us.Update();
            Assert.That(() => action.Status, Is.EqualTo(Status.Running));
        }
    }
}