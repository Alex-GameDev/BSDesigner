using BSDesigner.BehaviourTrees;
using BSDesigner.Reflection;
using BSDesigner.StateMachines;
using BSDesigner.UtilitySystems;

namespace TEstBSD.Metadata
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            var tree = new BehaviourTree();
            var us = new UtilitySystem();
            var fsm = new StateMachine();
            var metadata = new MockAPIMetadata();
            Assert.That(metadata, Is.Not.Null);

            var compositeTypeNode = metadata.GetTypeNode(typeof(CompositeNode));
            Assert.That(compositeTypeNode, Is.Not.Null);
            Assert.That(compositeTypeNode.SubTypes, Is.Not.Empty);
        }
    }

    public class MockAPIMetadata : APIMetadata
    {
        protected override IEnumerable<Type> GetTargetAssemblies()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => a.FullName?.StartsWith("BSDesigner") ?? false)
                .SelectMany(a => a.GetTypes());
        }
    }
}