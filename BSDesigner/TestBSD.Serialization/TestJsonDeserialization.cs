using BSDesigner.JsonSerialization;
using TestBSD.Mocks;

namespace TestBSD.Serialization;

public class TestJsonDeserialization
{
    private const string GRAPH_TYPE_TOKEN = "TestBSD.Mocks.MockGraph, TestBSD.Mocks";
    private const string NODE_TYPE_TOKEN = "TestBSD.Mocks.MockNode, TestBSD.Mocks";

    [Test]
    public void Deserialization_NullSystem_MatchExpectedResult()
    {
        const string jsonData = "null";
        var system = JsonSerialization.Deserialize(jsonData);
        Assert.That(system, Is.Null);
    }

    [Test]
    public void Deserialization_EmptySystem_MatchExpectedResult()
    {
        const string jsonData = "{\"engines\":[]}";
        var system = JsonSerialization.Deserialize(jsonData);
        Assert.That(system, Is.Not.Null);
        Assert.That(system?.engines, Has.Count.EqualTo(0));
    }

    [Test]
    public void Deserialization_SystemWithEmptyEngine_MatchExpectedResult()
    {
        const string jsonData = $"{{\"Engines\":[{{\"Engine\":{{\"$type\":\"{GRAPH_TYPE_TOKEN}\",\"Name\":\"\"}}}}]}}";
        var system = JsonSerialization.Deserialize(jsonData);
        Assert.That(system, Is.Not.Null);
        Assert.That(system?.engines, Has.Count.EqualTo(1));
        var engine = system?.engines.First();
        Assert.That(engine, Is.InstanceOf<MockGraph>());
        var graph = engine as MockGraph;
        Assert.That(graph?.Nodes, Has.Count.EqualTo(0));
    }

    [Test]
    public void Deserialization_SystemWithOneNodeGraph_MatchExpectedResult()
    {
        const string jsonData = $"{{\"Engines\":[{{\"Engine\":{{\"$type\":\"{GRAPH_TYPE_TOKEN}\",\"Name\":\"\"}},\"Nodes\":[{{\"$type\":\"{NODE_TYPE_TOKEN}\"}}]}}]}}";
        var system = JsonSerialization.Deserialize(jsonData);
        Assert.That(system, Is.Not.Null);
        Assert.That(system?.engines, Has.Count.EqualTo(1));

        var graph = system?.engines.First() as MockGraph;
        Assert.That(graph?.Nodes, Has.Count.EqualTo(1));

        var node = graph?.Nodes.First() as MockNode;
        Assert.That(node?.intValue, Is.EqualTo(0));
    }

    [Test]
    public void Deserialization_SystemWithConnectedNodesGraph_MatchExpectedResult()
    {
        const string jsonData = $"{{\"Engines\":[{{\"Engine\":{{\"$type\":\"{GRAPH_TYPE_TOKEN}\",\"Name\":\"\"}},\"Nodes\":[{{\"$type\":\"{NODE_TYPE_TOKEN}\"}},{{\"$type\":\"{NODE_TYPE_TOKEN}\"}}],\"Connections\":[{{\"SourceId\":0,\"TargetId\":1}}]}}]}}";
        var system = JsonSerialization.Deserialize(jsonData);
        Assert.That(system, Is.Not.Null);
        Assert.That(system?.engines, Has.Count.EqualTo(1));

        var graph = system?.engines.First() as MockGraph;
        Assert.That(graph?.Nodes, Has.Count.EqualTo(2));

        var node = graph?.Nodes[0] as MockNode;
        var node2 = graph?.Nodes[1] as MockNode;
        Assert.That(graph?.AreNodesConnected(node, node2), Is.True);
    }
}