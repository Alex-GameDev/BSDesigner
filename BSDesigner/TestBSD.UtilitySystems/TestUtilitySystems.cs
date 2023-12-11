using BSDesigner.Core;
using BSDesigner.Core.Exceptions;
using BSDesigner.Core.Tasks;
using BSDesigner.UtilitySystems;
using BSDesigner.UtilitySystems.UtilityElements;

namespace TestBSD.UtilitySystems
{
    [TestFixture]
    public class TestUtilitySystems
    {
        [Test]
        [TestCase(0.9f, 0.1f, 0)]
        [TestCase(0.1f, 0.9f, 1)]
        [TestCase(0.1f, 0.1f, 0)]
        public void UtilitySystem_Execute_SelectBestAction(float u1, float u2, int expectedSelectedIndex)
        {
            var us = new UtilitySystem();
            var factor1 = us.CreateConstantFactor(u1);
            var factor2 = us.CreateConstantFactor(u2);
            var action1 = us.CreateAction(factor1);
            var action2 = us.CreateAction(factor2);
            us.Start();
            us.Update();
            Assert.That(action1.Status, Is.EqualTo(expectedSelectedIndex == 0 ? Status.Running : Status.None));
            Assert.That(action2.Status, Is.EqualTo(expectedSelectedIndex == 1 ? Status.Running : Status.None));
        }

        [Test]
        public void UtilitySystem_Execute_PropagateEvents()
        {
            var isPaused = false;
            var us = new UtilitySystem();
            var factor = us.CreateConstantFactor(1f);
            var action = us.CreateAction(factor, new CustomActionTask { OnPause = () => isPaused = true, OnResume = () => isPaused = false });
            us.Start();
            Assert.That(action.Status, Is.EqualTo(Status.Running));
            us.Pause();
            Assert.That(isPaused, Is.True);
            us.Update();
            Assert.That(isPaused, Is.False);
            us.Stop();
            Assert.That(action.Status, Is.EqualTo(Status.None));
        }

        [Test]
        public void UtilitySystem_ExecuteWithAnyNode_ThrowException()
        {
            var us = new UtilitySystem();
            Assert.That(() => us.Start(), Throws.InstanceOf<EmptyGraphException>());
        }

        [Test]
        [TestCase(0.1f, 0.5f, 0.70f, 1.5f, 1)]
        [TestCase(0.1f, 0.5f, 0.75f, 1.5f, 0)]
        [TestCase(0.1f, 0.5f, 0.80f, 1.5f, 0)]
        public void UtilitySystem_SetInertia_SelectBestAction(float u1, float u2, float u1_2, float inertia, int expectedSelectedIndex)
        {
            var us = new UtilitySystem();
            var factor1 = us.CreateConstantFactor(u1);
            var factor2 = us.CreateConstantFactor(u2);
            var action1 = us.CreateAction(factor1);
            var action2 = us.CreateAction(factor2);
            us.Inertia = inertia;
            us.Start();
            us.Update();
            factor1.Value = u1_2;
            us.Update();
            Assert.That(action1.Status, Is.EqualTo(expectedSelectedIndex == 0 ? Status.Running : Status.None));
            Assert.That(action2.Status, Is.EqualTo(expectedSelectedIndex == 1 ? Status.Running : Status.None));
        }
    }
}