using BSDesigner.Core;
using BSDesigner.Core.Exceptions;
using BSDesigner.Core.Tasks;
using BSDesigner.UtilitySystems;

namespace TestBSD.UtilitySystems;

[TestFixture]
public class TestUtilitySystems
{
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
    public void UtilitySystem_DisablePulling_IgnoreUpdates()
    {
        var us = new UtilitySystem();

        var factor1 = us.CreateConstantFactor(0f);
        var factor2 = us.CreateConstantFactor(0f);
        var curve = us.CreateCurve<CustomCurve>(factor1);
        curve.Function = x => x;
        var fusion = us.CreateFusion<MaxFusion>(curve, factor2);
        var action = us.CreateAction(fusion);
        us.Start();
        us.Update();
        Assert.That(factor1.Utility, Is.EqualTo(0f));
        Assert.That(factor2.Utility, Is.EqualTo(0f));
        Assert.That(curve.Utility, Is.EqualTo(0f));
        Assert.That(fusion.Utility, Is.EqualTo(0f));
        Assert.That(action.Utility, Is.EqualTo(0f));
        factor1.Value = 0.5f;
        factor2.Value = 0.2f;
        curve.EnableUtilityUpdating = false;
        us.Update();
        Assert.That(factor1.Utility, Is.EqualTo(0f));
        Assert.That(factor2.Utility, Is.EqualTo(0.2f));
        Assert.That(curve.Utility, Is.EqualTo(0f));
        Assert.That(fusion.Utility, Is.EqualTo(0.2f));
        Assert.That(action.Utility, Is.EqualTo(0.2f));
    }
}