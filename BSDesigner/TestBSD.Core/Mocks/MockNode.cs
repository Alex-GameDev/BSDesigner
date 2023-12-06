using BSDesigner.Core;
using System.Diagnostics.CodeAnalysis;
using ExecutionContext = BSDesigner.Core.ExecutionContext;

namespace TestBSD.Core.Mocks
{
    [ExcludeFromCodeCoverage]
    public class MockNode : Node
    {
        public override Type GraphType => SupportedGraphType;
        public override Type ChildType => SupportedChildType;
        public override int MaxInputConnections => MaxParents;
        public override int MaxOutputConnections => MaxChildren;

        public int MaxParents = -1;
        public int MaxChildren = -1;
        public Type SupportedGraphType = typeof(MockGraph);
        public Type SupportedChildType = typeof(MockNode);

        public ExecutionContext Context { get; private set; } = null!;

        public MockNode() { }

        public MockNode(int maxParents, int maxChildren, Type supportedGraphType, Type supportedChildType)
        {
            MaxParents = maxParents;
            MaxChildren = maxChildren;
            SupportedGraphType = supportedGraphType;
            SupportedChildType = supportedChildType;
        }

        public override void SetContext(ExecutionContext context)
        {
            Context = context;
        }
    }
}