using BSDesigner.Core;
using BSDesigner.Core.Graphs;
using System.Diagnostics.CodeAnalysis;
using ExecutionContext = BSDesigner.Core.ExecutionContext;

namespace TestBSD.Core.Mocks
{
    [ExcludeFromCodeCoverage]
    public class MockNode : Node, ISubsystem
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

        public BehaviourEngine? Subsystem => subGraph;

        public BehaviourGraph? subGraph;



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