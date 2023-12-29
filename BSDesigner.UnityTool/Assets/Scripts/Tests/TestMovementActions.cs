using System.Collections;
using BSDesigner.Unity.Runtime;
using NUnit.Framework;
using UnityEngine.TestTools;
using UnityEngine;

public class TestMovementActions
{
    private MockBSRunner runner;

    [UnityTest]
    public IEnumerator TestMovementActionsWithEnumeratorPasses()
    {
        var go = new GameObject("TestObject");
        go.AddComponent<TransformMovement>();
        runner = go.AddComponent<MockBSRunner>();

        var action = new MoveToPointAction { Point = new Vector3(1f, 0f, 0f) };

        runner.Engine = new MockBehaviourEngine{ ActionTask = action };

        runner.Initialize();

        runner.StartEngine();

        Assert.That(runner.ExecutionEngine, Is.Not.Null);

        var time = 0f;
        while (time < 1f)
        {
            time += Time.deltaTime;
            yield return null;
        }

        Assert.That(runner.transform.position.x, Is.EqualTo(1f).Within(0.001f));
        yield return null;

        Object.Destroy(go);
    }
}
