using BSDesigner.Core;
using BSDesigner.JsonSerialization;
using TestBSD.Mocks;

namespace TestBSD.Serialization;

[TestFixture]
public class TestJsonSerialization
{
    [Test]
    public void Serialization_NullSystem_MatchExpectedResult()
    {
        BehaviourSystem? system = null;

        var jsonData = JsonSerialization.Serialize(system);
        Assert.That(jsonData.ToLower(), Is.EqualTo("null"));
    }

    [Test]
    public void Serialization_EmptySystem_MatchExpectedResult()
    {
        var system = new BehaviourSystem();

        var jsonData = JsonSerialization.Serialize(system);
        Assert.That(jsonData.ToLower(), Does.Match("{.*}"));
    }

    [Test]
    public void Serialization_SystemWithEmptyEngine_MatchExpectedResult()
    {
        var system = new BehaviourSystem();
        var engine = new MockGraph();
        system.engines.Add(engine);

        var jsonData = JsonSerialization.Serialize(system);
        Assert.That(jsonData.ToLower(), Does.Match("{*.\"engines\":\\[{.*\\}]*.}"));
    }

    [Test]
    public void Serialization_SystemWithOneNodeGraph_MatchExpectedResult()
    {
        var system = new BehaviourSystem();
        var engine = new MockGraph();
        engine.CreateNode(0, 1);
        system.engines.Add(engine);

        var jsonData = JsonSerialization.Serialize(system);
        Assert.That(jsonData.ToLower(), Does.Match("{.*}"));
    }

    [Test]
    public void Serialization_SystemWithConnectedNodesGraph_MatchExpectedResult()
    {
        var system = new BehaviourSystem();
        var engine = new MockGraph();
        var n1 = engine.CreateNode(0, 1);
        var n2 = engine.CreateNode(1, 0);
        engine.ConnectNodes(n1, n2);
        system.engines.Add(engine);

        var jsonData = JsonSerialization.Serialize(system);
        Assert.That(jsonData.ToLower(), Does.Match("{.*}"));
    }
}