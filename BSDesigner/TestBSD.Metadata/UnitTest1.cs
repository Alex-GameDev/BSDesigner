using BSDesigner.BehaviourTrees;
using BSDesigner.Core;
using BSDesigner.Reflection;
using BSDesigner.StateMachines;
using BSDesigner.UtilitySystems;
using Task = BSDesigner.Core.Task;

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

            var typeNode = metadata.GetTypeNode(typeof(Node));
            Assert.That(typeNode, Is.Not.Null);
            Assert.That(typeNode.SubTypes, Is.Not.Empty);
        }
    }

    public class MockAPIMetadata : APIMetadata
    {
        protected override HashSet<Type> GetRequiredRootTypes()
        {
            HashSet<Type> types = new HashSet<Type>();
            types.Add(typeof(Node));
            types.Add(typeof(BehaviourEngine));
            types.Add(typeof(Task));
            return types;
        }

        protected override IEnumerable<Type> GetTargetAssemblies()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => a.FullName?.StartsWith("BSDesigner") ?? false)
                .SelectMany(a => a.GetTypes());
        }
    }
}