using BSDesigner.Core;
using BSDesigner.Core.Graphs;
using BSDesigner.Core.Serialization;

namespace TestBSD.Core
{

    [TestFixture]
    public class GraphSerializationTests
    {
        [Test]
        public void SerializeNode_SimpleFields_ValidResult()
        {
            var node = new TestSerializableNode()
            {
                numericField = 1,
                textField = "test",
            };
            string json = JsonUtilities.SerializeNode(node);
            var generatedNode = JsonUtilities.DeserializeNode<TestSerializableNode>(json);
            
            Assert.IsNotNull(generatedNode);
            Assert.That(generatedNode.numericField, Is.EqualTo(node.numericField));
            Assert.That(generatedNode.textField, Is.EqualTo(node.textField));
            Assert.That(generatedNode.Name, Is.Empty);
        }

        [Test]
        public void SerializeNode_WithActionTask_ValidResult()
        {
            var action = new TestActionTask()
            {
                numericField = 1,
                textField = "test",
            };
            var node = new TestSerializableNode()
            {
                action = action,
            };
            string json = JsonUtilities.SerializeNode(node);
            var generatedNode = JsonUtilities.DeserializeNode<TestSerializableNode>(json);
            var generatedAction = generatedNode?.action as TestActionTask;

            Assert.IsNotNull(generatedNode);

            Assert.IsNotNull(generatedAction);
            Assert.That(generatedAction.numericField, Is.EqualTo(action.numericField));
            Assert.That(generatedAction.textField, Is.EqualTo(action.textField));
        }

        [Test]
        public void SerializeGraph_Empty_ValidResult()
        {
            var graph = new TestSerializableGraph()
            {
                numericField = 1,
                textField = "test_graph",
            };
            string json = JsonUtilities.Serialize(graph);
            var generatedGraph = JsonUtilities.Deserialize(json) as TestSerializableGraph;

            Assert.IsNotNull(generatedGraph);
            Assert.That(generatedGraph.numericField, Is.EqualTo(graph.numericField));
            Assert.That(generatedGraph.textField, Is.EqualTo(graph.textField));
        }

        [Test]
        public void SerializeGraph_SingleNode_ValidResult()
        {
            var graph = new TestSerializableGraph()
            {
                numericField = 1,
                textField = "test_graph",
            };
            var node = new TestSerializableNode()
            {
                numericField = 1,
                textField = "test",
            };
            graph.AddNode(node);
            string json = JsonUtilities.Serialize(graph);

            var generatedGraph = JsonUtilities.Deserialize(json) as TestSerializableGraph;
            Assert.IsNotNull(generatedGraph);
            Assert.That(generatedGraph.numericField, Is.EqualTo(graph.numericField));
            Assert.That(generatedGraph.textField, Is.EqualTo(graph.textField));
            var generatedNode = generatedGraph?.Nodes.FirstOrDefault() as TestSerializableNode;
            Assert.IsNotNull(generatedNode);
            Assert.That(generatedNode.numericField, Is.EqualTo(node.numericField));
            Assert.That(generatedNode.textField, Is.EqualTo(node.textField));
        }
    }

    class TestSerializableGraph : BehaviourGraph
    {
        public override Type NodeType => typeof(TestSerializableNode);

        public override bool CanCreateLoops => true;

        public int numericField;
        public string? textField;

        protected override void OnStarted() { }
        protected override void OnUpdated() { }
        protected override void OnStopped() { }
        protected override void OnPaused() { }
    }

    class TestSerializableNode : Node
    {
        public override Type GraphType => typeof(TestSerializableGraph);
        public override Type ChildType => typeof(TestSerializableNode);
        public override int MaxInputConnections => -1;
        public override int MaxOutputConnections => -1;

        public int numericField;
        public string? textField;

        public ActionTask? action;
    }

    class TestActionTask : ActionTask
    {
        public int numericField;
        public string? textField;

        public override string GetInfo() => "";
        protected override void OnBeginTask() { }
        protected override void OnEndTask() { }
        protected override void OnPauseTask() { }
        protected override void OnResumeTask() { }
        protected override Status OnUpdateTask() => Status.Success;
    }
}
