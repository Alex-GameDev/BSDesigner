using BSDesigner.Core;

namespace TestBSD.Core.Mocks
{
    public class MockGraph : BehaviourGraph
    {
        public static readonly string START_EVENT = "started";
        public static readonly string UPDATE_EVENT = "Updated";
        public static readonly string STOP_EVENT = "stopped";
        public static readonly string PAUSE_EVENT = "paused";
        public static readonly string RESUME_EVENT = "resumed";

        public MockNode CreateNode(int maxParents, int maxChildren)
        {
            var mockNode = CreateNode<MockNode>();
            mockNode.MaxParents = maxParents;
            mockNode.MaxChildren = maxChildren;
            return mockNode;
        }

        public event Action<string> OnEvent = delegate { };

        public override Type NodeType => SupportedNodeType;

        public Type SupportedNodeType = typeof(MockNode);

        public MockGraph()
        {
        }

        public MockGraph(Type supportedNodeType)
        {
            SupportedNodeType = supportedNodeType;
        }

        protected override void OnStarted() => OnEvent(START_EVENT);

        protected override void OnUpdated() => OnEvent(UPDATE_EVENT);

        protected override void OnStopped() => OnEvent(STOP_EVENT);

        protected override void OnPaused() => OnEvent(PAUSE_EVENT);

        protected override void OnResumed() => OnEvent(RESUME_EVENT);

        public void SetStatus(Status status) => Status = status;
    }
}