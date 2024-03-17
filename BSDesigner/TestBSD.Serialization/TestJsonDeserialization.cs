using BSDesigner.Core;
using BSDesigner.JsonSerialization;
using TestBSD.Mocks;

namespace TestBSD.Serialization;

public class TestJsonDeserialization
{
    private const string GRAPH_TYPE_TOKEN = "TestBSD.Mocks.MockGraph, TestBSD.Mocks";
    private const string NODE_TYPE_TOKEN = "TestBSD.Mocks.MockNode, TestBSD.Mocks";

    #region Behaviour engines

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
    #endregion

    #region Blackboard

    [Test]
    public void Serialization_EmptyBlackboard_MatchExpectedResult()
    {
        const string jsonData = "{\"blackboard\":[]}";

        var system = JsonSerialization.Deserialize(jsonData);
        Assert.That(system?.blackboard, Is.Not.Null);
        Assert.That(system?.blackboard?.GetAllFields().ToList(), Has.Count.EqualTo(0));
    }

    [Test]
    public void Serialization_BlackboardWithFields_MatchExpectedResult()
    {
        const string jsonData = "{\"blackboard\":[{\"id\":\"intfield\",\"value\":2}]}";

        var system = JsonSerialization.Deserialize(jsonData);
        Assert.That(system?.blackboard, Is.Not.Null);
        Assert.That(system?.blackboard?.GetAllFields().ToList(), Has.Count.EqualTo(1));
    }

    [Test]
    public void Serialization_BlackboardWithLongField_MatchExpectedResult()
    {
        const string jsonData = "{\"blackboard\":[{\"id\":\"longfield\",\"$type\":\"System.Private.CoreLib, System.Int64\",\"value\":2}]}";

        var system = JsonSerialization.Deserialize(jsonData);
        Assert.That(system?.blackboard, Is.Not.Null);
        Assert.That(system?.blackboard?.GetAllFields().ToList(), Has.Count.EqualTo(1));
        var field = system?.blackboard?.GetFieldById<long>("longfield");
        Assert.That(field?.Value, Is.EqualTo(2L));
    }

    [Test]
    public void Serialization_BlackboardWithFewField_MatchExpectedResult()
    {
        const string jsonData = "{\"Blackboard\":[{\"id\":\"bool\",\"value\":true},{\"id\":\"string\",\"value\":\"str\"},{\"id\":\"int\",\"value\":2}]}";

        var system = JsonSerialization.Deserialize(jsonData);
        Assert.That(system?.blackboard, Is.Not.Null);
        Assert.That(system?.blackboard?.GetAllFields().ToList(), Has.Count.EqualTo(3));

        var field1 = system?.blackboard?.GetFieldById<int>("int");
        Assert.That(field1?.Value, Is.EqualTo(2));

        var field2 = system?.blackboard?.GetFieldById<string>("string");
        Assert.That(field2?.Value, Is.EqualTo("str"));

        var field3 = system?.blackboard?.GetFieldById<bool>("bool");
        Assert.That(field3?.Value, Is.True);
    }

    [Test]
    public void Serialization_BlackboardWithPolymorficField_MatchExpectedResult()
    {
        const string jsonData = "{\"blackboard\":[{\"id\":\"data\",\"$type\":\"TestBSD.Mocks.ExampleData, TestBSD.Mocks\",\"value\":{\"$type\":\"TestBSD.Mocks.DerivedAExampleData, TestBSD.Mocks\",\"doubleValue\":5.0,\"boolValue\":true,\"intValue\":2}}]}";

        var system = JsonSerialization.Deserialize(jsonData);
        Assert.That(system?.blackboard, Is.Not.Null);
        Assert.That(system?.blackboard?.GetAllFields().ToList(), Has.Count.EqualTo(1));
        var field = system?.blackboard?.GetFieldById<ExampleData>("data");
        Assert.That(field?.Value, Is.InstanceOf<DerivedAExampleData>());
    }

    #endregion
}