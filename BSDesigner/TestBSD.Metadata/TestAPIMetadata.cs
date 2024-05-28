using BSDesigner.BehaviourTrees;
using BSDesigner.Core;
using BSDesigner.Reflection;
using BSDesigner.StateMachines;
using BSDesigner.UtilitySystems;
using Task = BSDesigner.Core.Task;

namespace TestBSD.Metadata
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void GetTypeNode_ValidResult()
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

        [Test]
        public void GetSubtypesRecursively_ValidResult()
        {
            var tree = new BehaviourTree();
            var us = new UtilitySystem();
            var fsm = new StateMachine();
            var metadata = new MockAPIMetadata();
            Assert.That(metadata, Is.Not.Null);

            var typeNode = metadata.GetTypeNode(typeof(BehaviourEngine));
            var flatTypes = typeNode.GetConcreteSubtypesRecursively();

            Assert.That(flatTypes, Is.Not.Null);
            Assert.That(flatTypes.Count(), Is.GreaterThan(0));
            Assert.That(flatTypes.Select(n => n.Type), Has.Some.EqualTo(tree.GetType()));
            Assert.That(flatTypes.Select(n => n.Type), Has.Some.EqualTo(us.GetType()));
            Assert.That(flatTypes.Select(n => n.Type), Has.Some.EqualTo(fsm.GetType()));
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