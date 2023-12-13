using BSDesigner.Core;
using BSDesigner.Core.Exceptions;
using BSDesigner.Core.Tasks;
using BSDesigner.UtilitySystems;
using System;

namespace TestBSD.UtilitySystems;

[TestFixture]
public class TestUtilityBuckets
{
    [Test]
    public void UtilityBucket_ExecuteWithNoChildren_ThrowException()
    {
        var us = new UtilitySystem();
        var bucket = us.CreateBucket<InertiaBucket>();
        us.ChangeRootNode(bucket);
        Assert.That(us.Start, Throws.InstanceOf<MissingConnectionException>());
    }

    [Test]
    public void UtilityBucket_Execute_PropagateEvents()
    {
        var isPaused = false;
        var us = new UtilitySystem();
        var factor = us.CreateConstantFactor(1f);
        var action = us.CreateAction(factor, new CustomActionTask { OnPause = () => isPaused = true, OnResume = () => isPaused = false });
        var bucket = us.CreateBucket<InertiaBucket>(action);
        us.ChangeRootNode(bucket);

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
    [TestCase(0.9f, 0.1f, 0)]
    [TestCase(0.1f, 0.9f, 1)]
    [TestCase(0.1f, 0.1f, 0)]
    public void InertiaBucket_Execute_SelectBestAction(float u1, float u2, int expectedSelectedIndex)
    {
        var us = new UtilitySystem();
        var factor1 = us.CreateConstantFactor(u1);
        var factor2 = us.CreateConstantFactor(u2);
        var action1 = us.CreateAction(factor1);
        var action2 = us.CreateAction(factor2);
        var bucket = us.CreateBucket<InertiaBucket>(action1, action2);
        us.ChangeRootNode(bucket);
        us.Start();
        us.Update();
        Assert.That(action1.Status, Is.EqualTo(expectedSelectedIndex == 0 ? Status.Running : Status.None));
        Assert.That(action2.Status, Is.EqualTo(expectedSelectedIndex == 1 ? Status.Running : Status.None));
    }

    [Test]
    [TestCase(0.1f, 0.5f, 0.70f, 1.5f, 1)]
    [TestCase(0.1f, 0.5f, 0.75f, 1.5f, 0)]
    [TestCase(0.1f, 0.5f, 0.80f, 1.5f, 0)]
    public void InertiaBucket_SetInertia_SelectBestAction(float u1, float u2, float u1_2, float inertia, int expectedSelectedIndex)
    {
        var us = new UtilitySystem();
        var factor1 = us.CreateConstantFactor(u1);
        var factor2 = us.CreateConstantFactor(u2);
        var action1 = us.CreateAction(factor1);
        var action2 = us.CreateAction(factor2);
        var bucket = us.CreateBucket<InertiaBucket>(action1, action2);
        us.ChangeRootNode(bucket);
        bucket.Inertia = inertia;
        us.Start();
        us.Update();
        factor1.Value = u1_2;
        us.Update();
        Assert.That(action1.Status, Is.EqualTo(expectedSelectedIndex == 0 ? Status.Running : Status.None));
        Assert.That(action2.Status, Is.EqualTo(expectedSelectedIndex == 1 ? Status.Running : Status.None));
    }

    [Test]
    [TestCase(0.4f, 0.1f, 0.5f,true, Description = "Not reach utility threshold")]
    [TestCase(0.8f, 0.1f, 0.5f, false, Description = "Reach utility threshold")]
    public void InertiaBucket_SetBucketThreshold_IgnoreBucket(float u1, float u2, float bucketThreshold, bool ignoreBucket)
    {
        var us = new UtilitySystem();
        var factor1 = us.CreateConstantFactor(u1);
        var factor2 = us.CreateConstantFactor(u2);
        var action1 = us.CreateAction(factor1);
        var action2 = us.CreateAction(factor2);
        var priorityBucket = us.CreateBucket<InertiaBucket>(action1);
        priorityBucket.BucketThreshold = bucketThreshold;
        var mainBucket = us.CreateBucket<InertiaBucket>(priorityBucket, action2);
        us.ChangeRootNode(mainBucket);
        us.Start();
        us.Update();
        Assert.That(action1.Status, Is.EqualTo(!ignoreBucket ? Status.Running : Status.None));
        Assert.That(action2.Status, Is.EqualTo(ignoreBucket ? Status.Running : Status.None));
    }


    [Test]
    [TestCase(0.5f, 1.0f, 0.5f, 0.8f, false, Description = "Not reach priority threshold")]
    [TestCase(0.8f, 1.0f, 0.5f, 0.8f, true, Description = "Reach priority threshold")]
    [TestCase(0.5f, 0.1f, 0.5f, 0.8f, true, Description = "Not Reach priority threshold but higher utility")]
    [TestCase(0.8f, 1.0f, 0.8f, 0.5f, true, Description = "Reach max bw priority and bucket threshold")]
    public void InertiaBucket_SetPriorityThreshold_EnablePriority(float u1, float u2, float bucketThreshold, float priorityThreshold, bool executePriorityBucket)
    {
        var us = new UtilitySystem();
        var factor1 = us.CreateConstantFactor(u1);
        var factor2 = us.CreateConstantFactor(u2);
        var action1 = us.CreateAction(factor1);
        var action2 = us.CreateAction(factor2);
        var priorityBucket = us.CreateBucket<InertiaBucket>(action1);
        priorityBucket.BucketThreshold = bucketThreshold;
        priorityBucket.PriorityThreshold = priorityThreshold;
        var mainBucket = us.CreateBucket<InertiaBucket>(priorityBucket, action2);
        us.ChangeRootNode(mainBucket);
        us.Start();
        us.Update();
        Assert.That(action1.Status, Is.EqualTo(executePriorityBucket ? Status.Running : Status.None));
        Assert.That(action2.Status, Is.EqualTo(!executePriorityBucket ? Status.Running : Status.None));
    }

    [Test]
    public void LockBucket_ExecuteLockCandidateUntilFinish()
    {
        var status = Status.Running;
        var us = new UtilitySystem();
        var factor1 = us.CreateConstantFactor(1f);
        var factor2 = us.CreateConstantFactor(0f);
        var action1 = us.CreateAction(factor1, new CustomActionTask {OnUpdate = () => status});
        var action2 = us.CreateAction(factor2);
        var bucket = us.CreateBucket<LockBucket>(action1, action2);
        us.ChangeRootNode(bucket);
        us.Start();
        us.Update();
        Assert.That(action1.Status, Is.EqualTo(Status.Running));
        Assert.That(action2.Status, Is.EqualTo(Status.None));
        factor1.Value = 0f;
        factor2.Value = 1f;
        status = Status.Success;
        us.Update();
        Assert.That(action1.Status, Is.EqualTo(Status.Success));
        Assert.That(action2.Status, Is.EqualTo(Status.None));
        us.Update();
        Assert.That(action1.Status, Is.EqualTo(Status.None));
        Assert.That(action2.Status, Is.EqualTo(Status.Running));
    }
}