using BSDesigner.Core;
using BSDesigner.JsonSerialization;
using TestBSD.Mocks;

namespace TestBSD.Serialization;

[TestFixture]
public class TestJsonSerialization
{
    #region Behaviour engines

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
    #endregion

    #region Blackboard

    [Test]
    public void Serialization_EmptyBlackboard_MatchExpectedResult()
    {
        var system = new BehaviourSystem
        {
            blackboard = new Blackboard()
        };

        var jsonData = JsonSerialization.Serialize(system);
        Assert.That(jsonData.ToLower(), Does.Match("{\"blackboard\":\\[\\].*}"));
    }

    [Test]
    public void Serialization_BlackboardWithFields_MatchExpectedResult()
    {
        var blackboard = new Blackboard();
        blackboard.CreateField<int>("intField", 2);
        var system = new BehaviourSystem() { blackboard = blackboard };

        var jsonData = JsonSerialization.Serialize(system);
        Assert.That(jsonData.ToLower(), Does.Match("{\"blackboard\":\\[{\"id\":\"intfield\",\"fieldtype\":\".*\",\"value\":2}\\].*}"));
    }

    [Test]
    public void Serialization_BlackboardWithLongField_MatchExpectedResult()
    {
        var blackboard = new Blackboard();
        blackboard.CreateField("longField", 2L);
        var system = new BehaviourSystem() { blackboard = blackboard };

        var jsonData = JsonSerialization.Serialize(system);
        Assert.That(jsonData.ToLower(), Does.Match("{\"blackboard\":\\[{\"id\":\"longfield\",\"fieldtype\":\".*\",\"value\":2}\\].*}"));
    }

    [Test]
    public void Serialization_BlackboardWithFewField_MatchExpectedResult()
    {
        var blackboard = new Blackboard();
        blackboard.CreateField("bool", true);
        blackboard.CreateField("string", "str");
        blackboard.CreateField("int", 2);
        var system = new BehaviourSystem() { blackboard = blackboard };

        var jsonData = JsonSerialization.Serialize(system);
        Assert.That(jsonData.ToLower(), Does.Match(".*{\"id\":\"int\",\"fieldtype\":.*\",\"value\":2}.*"));
        Assert.That(jsonData.ToLower(), Does.Match("{\"id\":\"string\",\"fieldtype\":.*\",\"value\":\"str\"}"));
        Assert.That(jsonData.ToLower(), Does.Match(".*{\"id\":\"bool\",\"fieldtype\":.*\",\"value\":true}.*"));
    }

    [Test]
    public void Serialization_BlackboardWithPolymorficField_MatchExpectedResult()
    {
        var blackboard = new Blackboard();
        blackboard.CreateField<ExampleData>("data", new DerivedAExampleData{boolValue = true, doubleValue = 5.0, intValue = 2});
        var system = new BehaviourSystem() { blackboard = blackboard };

        var jsonData = JsonSerialization.Serialize(system);
        Assert.That(jsonData.ToLower(), Does.Contain("{\"id\":\"data\",\"fieldtype\":\"testbsd.mocks.exampledata, testbsd.mocks\",\"value\":{\"$type\":\"testbsd.mocks.derivedaexampledata, testbsd.mocks\",\"doublevalue\":5.0,\"boolvalue\":true,\"intvalue\":2}}"));
    }

    #endregion

    #region Parameters

    [Test]
    public void Serialization_NodeWithValueParameter_MatchExpectedResult()
    {
        var system = new BehaviourSystem();
        var engine = new MockGraph();
        var node = engine.CreateNode<ParameterNode>(0, 1);
        node.intParameter = 2;
        system.engines.Add(engine);

        var jsonData = JsonSerialization.Serialize(system);
        Assert.That(jsonData.ToLower(), Does.Match("{.*\"intparameter\":2}.*}"));
    }

    [Test]
    public void Serialization_NodeWithBoundParameter_MatchExpectedResult()
    {
        var system = new BehaviourSystem();
        var blackboard = new Blackboard();
        var field = blackboard.CreateField("intField", 2);
        var engine = new MockGraph();
        var node = engine.CreateNode<ParameterNode>(0, 1);
        node.intParameter = field;
        system.blackboard = blackboard;
        system.engines.Add(engine);

        var jsonData = JsonSerialization.Serialize(system);
        Assert.That(jsonData.ToLower(), Does.Match("{.*\"intparameter\":{\"id\":\"intfield\"}}.*}"));
    }

    #endregion
}