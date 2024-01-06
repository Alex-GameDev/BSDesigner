using BSDesigner.Core.Exceptions;
using BSDesigner.UtilitySystems;

namespace TestBSD.UtilitySystems
{
    [TestFixture]
    public class TestUtilityFusions
    {
        [Test]
        public void UtilityCurve_NoChild_ThrowException()
        {
            var us = new UtilitySystem();
            var factor = us.CreateFusion<MaxFusion>();
            us.CreateAction(factor);
            Assert.That(us.Start, Throws.InstanceOf<MissingConnectionException>());
        }

        [Test]
        public void Min_Curve_ComputeUtility_CorrectValue()
        {
            var us = new UtilitySystem();
            var leaf1 = us.CreateConstantLeaf(0.1f);
            var leaf2 = us.CreateConstantLeaf(0.2f);
            var factor = us.CreateFusion<MinFusion>(leaf1, leaf2);
            var action = us.CreateAction(factor);
            us.Start();
            us.Update();
            Assert.That(action.Utility, Is.EqualTo(0.1f));
        }

        [Test]
        public void MaxCurve_ComputeUtility_CorrectValue()
        {
            var us = new UtilitySystem();
            var leaf1 = us.CreateConstantLeaf(0.1f);
            var leaf2 = us.CreateConstantLeaf(0.2f);
            var factor = us.CreateFusion<MaxFusion>(leaf1, leaf2);
            var action = us.CreateAction(factor);
            us.Start();
            us.Update();
            Assert.That(action.Utility, Is.EqualTo(0.2f));
        }

        [Test]
        public void WeightedCurve_SameWeightsThanChildren_CorrectValue()
        {
            var us = new UtilitySystem();
            var leaf1 = us.CreateConstantLeaf(0.4f);
            var leaf2 = us.CreateConstantLeaf(0.8f);
            var factor = us.CreateFusion<WeightedFusion>(leaf1, leaf2);
            factor.Weights = new List<float> { 0.5f, 0.3f };
            var action = us.CreateAction(factor);
            us.Start();
            us.Update();
            Assert.That(action.Utility, Is.EqualTo(0.44f));
        }

        [Test]
        public void WeightedCurve_LessWeightsThanChildren_RestChildIsZero()
        {
            var us = new UtilitySystem();
            var leaf1 = us.CreateConstantLeaf(0.4f);
            var leaf2 = us.CreateConstantLeaf(0.8f);
            var factor = us.CreateFusion<WeightedFusion>(leaf1, leaf2);
            factor.Weights = new List<float> { 0.5f };
            var action = us.CreateAction(factor);
            us.Start();
            us.Update();
            Assert.That(action.Utility, Is.EqualTo(0.2f));
        }

        [Test]
        public void WeightedCurve_MoreWeightsThanChildren_IgnoreLeftoverWeight()
        {
            var us = new UtilitySystem();
            var leaf1 = us.CreateConstantLeaf(0.4f);
            var leaf2 = us.CreateConstantLeaf(0.8f);
            var factor = us.CreateFusion<WeightedFusion>(leaf1, leaf2);
            factor.Weights = new List<float> { 0.5f, 0.3f, 0.2f, 0.1f };
            var action = us.CreateAction(factor);
            us.Start();
            us.Update();
            Assert.That(action.Utility, Is.EqualTo(0.44f));
        }
    }
}