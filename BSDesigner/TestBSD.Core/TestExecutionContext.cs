using TestBSD.Core.Mocks;
using ExecutionContext = BSDesigner.Core.ExecutionContext;

namespace TestBSD.Core
{
    [TestFixture]
    public class TestExecutionContext
    {
        [Test]
        public void SetContext_GraphWithNodes_PropagateInNodes()
        {
            var context = new ExecutionContext();
            var behaviourEngine = new MockGraph();
            var n1 = behaviourEngine.CreateNode(-1, -1);
            behaviourEngine.SetContext(context);
            Assert.That(n1.Context, Is.EqualTo(context));

        }
    }
}