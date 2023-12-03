using BSDesigner.Core;

namespace TestBSD.Core.Mocks
{
    public class MockGraph : BehaviourGraph
    {
        public MockNode CreateNode(int maxParents, int maxChildren)
        {
            var mockNode = CreateNode<MockNode>();
            mockNode.MaxParents = maxParents;
            mockNode.MaxChildren = maxChildren;
            return mockNode;
        }

        public override Type NodeType => SupportedNodeType;

        public Type SupportedNodeType = typeof(MockNode);

        public MockGraph()
        {
        }

        public MockGraph(Type supportedNodeType)
        {
            SupportedNodeType = supportedNodeType;
        }
    }
}