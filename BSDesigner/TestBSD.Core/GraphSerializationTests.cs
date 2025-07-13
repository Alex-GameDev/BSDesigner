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
            string expectedJson = "{\"numericField\":1,\"textField\":\"test\"}";
            Assert.That(json, Is.Not.Null);
            Assert.That(json, Is.EqualTo(expectedJson));
        }

        [Test]
        public void SerializeNode_WithActionTask_ValidResult()
        {
            var node = new TestSerializableNode()
            {
                action = new TestActionTask()
                {
                    numericField = 1,
                    textField = "test",
                }
            };
            string json = JsonUtilities.SerializeNode(node);
            string expectedJson = "{" +
                "\"action\":{" +
                "\"$type\":\"TestBSD.Core.TestActionTask, TestBSD.Core\"," +
                "\"numericField\":1," +
                "\"textField\":\"test\"}}";

            Assert.That(json, Is.Not.Null);
            Assert.That(json, Is.EqualTo(expectedJson));
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
            string expectedJson =
                "{" +
                    "\"Graph\":" +
                    "{" +
                        "\"$type\":\"TestBSD.Core.TestSerializableGraph, TestBSD.Core\"," +
                        "\"numericField\":1," +
                        "\"textField\":\"test_graph\"" +
                    "}" +
                "}";

            Assert.That(json, Is.Not.Null);
            Assert.That(json, Is.EqualTo(expectedJson));
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
            string expectedJson =
                "{" +
                    "\"Graph\":" +
                    "{" +
                        "\"$type\":\"TestBSD.Core.TestSerializableGraph, TestBSD.Core\"," +
                        "\"numericField\":1," +
                        "\"textField\":\"test_graph\"" +
                    "}," +
                    "\"Nodes\":[" +
                        "{" +
                                "\"$type\":\"TestBSD.Core.TestSerializableNode, TestBSD.Core\"," +
                                "\"numericField\":1," +
                                "\"textField\":\"test\"" +
                         "}" +
                    "]" +
                "}";

            Assert.That(json, Is.Not.Null);
            Assert.That(json, Is.EqualTo(expectedJson));
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
