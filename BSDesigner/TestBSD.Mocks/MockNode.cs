using System;
using BSDesigner.Core;

namespace TestBSD.Mocks
{
    public class MockNode : Node
    {
        public override Type GraphType => SupportedGraphType;
        public override Type ChildType => SupportedChildType;
        public override int MaxInputConnections => MaxParents;
        public override int MaxOutputConnections => MaxChildren;

        public int MaxParents { get; set; } = -1;
        public int MaxChildren { get; set; } = -1;
        public Type SupportedGraphType { get; set; } = typeof(MockGraph);
        public Type SupportedChildType { get; set; } = typeof(MockNode);

        public int intValue;

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