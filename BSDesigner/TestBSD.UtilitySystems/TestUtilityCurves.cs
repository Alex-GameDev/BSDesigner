using System.Numerics;
using BSDesigner.Core.Exceptions;
using BSDesigner.UtilitySystems;

namespace TestBSD.UtilitySystems
{
    [TestFixture]
    public class TestUtilityCurves
    {
        [Test]
        public void UtilityCurve_NoChild_ThrowException()
        {
            var us = new UtilitySystem();
            var factor = new LinearCurve();
            us.AddNode(factor);
            us.CreateAction(factor);
            Assert.That(() => us.Start(), Throws.InstanceOf<MissingConnectionException>());
        }

        [Test]
        [TestCase(0, 0.5f, 0.5f, 0.5f)]
        [TestCase(0.5f, 0, 0.5f, 0.25f)]
        [TestCase(-1f, 0, 0.5f, 0f)]
        [TestCase(1, 1, 0.5f, 1f)]
        public void LinearCurve_ComputeUtility_CorrectValue(float m, float n, float childValue, float expectedResult)
        {
            var us = new UtilitySystem();
            var leaf = us.CreateConstantFactor(childValue);
            var factor = us.CreateCurve<LinearCurve>(leaf);
            factor.Slope = m;
            factor.YIntercept = n;
            var action = us.CreateAction(factor);
            us.Start();
            Assert.That(action.Utility, Is.EqualTo(expectedResult));
        }

        [Test]
        [TestCase(0, 0f, 0, 0.5f, 1f)]
        [TestCase(1, 0f, 0, 0.5f, 0.5f)]
        [TestCase(2f, 0f, 0, 0.5f, 0.25f)]
        [TestCase(2f, .25f, 0.2f, 0.5f, 0.2625f)]
        [TestCase(-1f, 0f, -0.5f, 0.8f, 0.75f)]
        public void ExponentialCurve_ComputeUtility_CorrectValue(float exp, float dx, float dy, float childValue, float expectedResult)
        {
            var us = new UtilitySystem();
            var leaf = us.CreateConstantFactor(childValue);
            var factor = us.CreateCurve<ExponentialCurve>(leaf);
            factor.Exponent = exp;
            factor.DespX = dx;
            factor.DespY = dy;
            var action = us.CreateAction(factor);
            us.Start();
            Assert.That(action.Utility, Is.EqualTo(expectedResult).Within(0.01f));
        }


        [Test]
        [TestCase(0, 10f,  0, 0.5f)]
        [TestCase(0.5f, 10f, 0.5f, 0.5f)]
        [TestCase(0.5f, -10f, 0f, 1f)]

        public void SigmoidCurve_ComputeUtility_CorrectValue(float midpoint, float grownRate, float childValue, float expectedResult)
        {
            var us = new UtilitySystem();
            var leaf = us.CreateConstantFactor(childValue);
            var factor = us.CreateCurve<SigmoidCurve>(leaf);
            factor.GrownRate = grownRate;
            factor.Midpoint = midpoint;
            var action = us.CreateAction(factor);
            us.Start();
            Assert.That(action.Utility, Is.EqualTo(expectedResult).Within(0.01f));
        }

        [Test]
        public void CustomCurve_ValidFunction_CorrectValue()
        {
            var us = new UtilitySystem();
            var leaf = us.CreateConstantFactor(0.5f);
            var factor = us.CreateCurve<CustomCurve>(leaf);
            factor.Function = (x) => x * x;
            var action = us.CreateAction(factor);
            us.Start();
            Assert.That(action.Utility, Is.EqualTo(0.25f));
        }

        [Test]
        public void CustomCurve_NoFunction_ReturnChildValue()
        {
            var us = new UtilitySystem();
            var leaf = us.CreateConstantFactor(0.5f);
            var factor = us.CreateCurve<CustomCurve>(leaf);
            var action = us.CreateAction(factor);
            us.Start();
            Assert.That(action.Utility, Is.EqualTo(0.5f));
        }

        [Test]
        public void DashedCurve_NoPoints_ReturnZero()
        {
            var us = new UtilitySystem();
            var leaf = us.CreateConstantFactor(0.5f);
            var factor = us.CreateCurve<DashedCurve>(leaf);
            var action = us.CreateAction(factor);
            us.Start();
            Assert.That(action.Utility, Is.EqualTo(0f));
        }

        [Test]
        [TestCase(0)]
        [TestCase(0.5f)]
        [TestCase(1)]
        public void DashedCurve_OnePoint_ReturnPointValue(float childValue)
        {
            var us = new UtilitySystem();
            var leaf = us.CreateConstantFactor(childValue);
            var factor = us.CreateCurve<DashedCurve>(leaf);
            factor.Points.Add(new Vector2(0.5f, 0.5f));
            var action = us.CreateAction(factor);
            us.Start();
            Assert.That(action.Utility, Is.EqualTo(0.5f));
        }

        [Test]
        [TestCase(0.1f, 0.7f)]
        [TestCase(0.4f, 0.4f)]
        [TestCase(0.8f, 0.1f)]
        public void DashedCurve_FewPoints_ReturnValue(float childValue, float expectedValue)
        {
            var us = new UtilitySystem();
            var leaf = us.CreateConstantFactor(childValue);
            var factor = us.CreateCurve<DashedCurve>(leaf);
            factor.Points.Add(new Vector2(0.2f, 0.7f));
            factor.Points.Add(new Vector2(0.6f, 0.1f));
            var action = us.CreateAction(factor);
            us.Start();
            Assert.That(action.Utility, Is.EqualTo(expectedValue).Within(0.01f));
        }
    }
}