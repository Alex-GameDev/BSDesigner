using BSDesigner.Core.Exceptions;
using BSDesigner.UtilitySystems;

namespace TestBSD.UtilitySystems
{
    [TestFixture]
    public class TestLeafFactors
    {
        [Test]
        public void LeafFactor_ConnectChild_ThrowException()
        {
            var us = new UtilitySystem();
            var factor1 = us.CreateConstantFactor(1f);
            var factor2 = us.CreateConstantFactor(1f);
            Assert.That(() => us.ConnectNodes(factor1, factor2), Throws.InstanceOf<ConnectionException>());
        }

        [Test]
        [TestCase(0.5f, 0.5f)]
        [TestCase(-1f, 0f)]
        [TestCase(10f, 1f)]
        public void Factor_SetUtilityValue_Clamp01(float factorValue, float expectedValue)
        {
            var us = new UtilitySystem();
            var factor = us.CreateConstantFactor(factorValue);
            us.CreateAction(factor);
            us.Start();
            us.Update();
            Assert.That(factor.Utility, Is.EqualTo(expectedValue));
        }
    }
}