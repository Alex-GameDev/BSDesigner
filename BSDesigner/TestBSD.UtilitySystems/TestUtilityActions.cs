using BSDesigner.Core;
using BSDesigner.Core.Exceptions;
using BSDesigner.Core.Tasks;
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
    }
}