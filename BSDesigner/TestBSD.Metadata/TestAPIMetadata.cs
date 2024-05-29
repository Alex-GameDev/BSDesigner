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
            // Load assemblies
            var tree = new BehaviourTree();
            var us = new UtilitySystem();
            var fsm = new StateMachine();
        }

        [Test]
        public void GetTypeNode_ValidResult()
        {
            var metadata = new MockAPIMetadata();
            Assert.That(metadata, Is.Not.Null);

            var typeNode = metadata.GetTypeNode(typeof(Node));
            Assert.That(typeNode, Is.Not.Null);
            Assert.That(typeNode.SubTypes, Is.Not.Empty);
        }

        [Test]
        public void GetSubtypesRecursively_ValidResult()
        {
            var metadata = new MockAPIMetadata();
            var typeNode = metadata.GetTypeNode(typeof(BehaviourEngine));
            var flatTypes = typeNode.GetConcreteSubtypesRecursively();

            Assert.That(flatTypes, Is.Not.Null);
            Assert.That(flatTypes.Count(), Is.GreaterThan(0));
            Assert.That(flatTypes.Select(n => n.Type), Has.Some.EqualTo(typeof(BehaviourTree)));
            Assert.That(flatTypes.Select(n => n.Type), Has.Some.EqualTo(typeof(StateMachine)));
            Assert.That(flatTypes.Select(n => n.Type), Has.Some.EqualTo(typeof(UtilitySystem)));
        }

        [Test]
        public void GetRelatedType_ValidResult()
        {
            var metadata = new MockAPIMetadata();
            var relatedType1 = metadata.GetRelatedTypeof<TargetClass>(typeof(BtNode));
            Assert.That(relatedType1, Is.EqualTo(typeof(BtTargetClass)));
            var relatedType2 = metadata.GetRelatedTypeof<TargetClass>(typeof(FsmNode));
            Assert.That(relatedType2, Is.EqualTo(typeof(FsmTargetClass)));
            var relatedType3 = metadata.GetRelatedTypeof<TargetClass>(typeof(UtilityNode));
            Assert.That(relatedType3, Is.EqualTo(typeof(UsTargetClass)));
        }
    }

    public class MockAPIMetadata : APIMetadata
    {
        protected override HashSet<Type> GetRequiredRootTypes() =>
            new HashSet<Type>
            {
                typeof(Node),
                typeof(BehaviourEngine),
                typeof(Task),
                typeof(Node),
                typeof(TargetClass)
            };

        protected override IEnumerable<Type> GetTargetAssemblies()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => a.FullName?.StartsWith("BSDesigner") ?? false)
                .SelectMany(a => a.GetTypes())
                .Append(typeof(BtTargetClass))
                .Append(typeof(FsmTargetClass))
                .Append(typeof(UsTargetClass));
        }

        protected override void Initialize()
        {
            RegisterTargetType<MockTypeRelatedAttribute>(typeof(TargetClass), typeof(Node));
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class MockTypeRelatedAttribute : TypeRelationAttribute
    {
        public MockTypeRelatedAttribute(Type type) : base(type)
        {
        }
    }

    public abstract class TargetClass { }

    [MockTypeRelated(typeof(BtNode))] 
    public class BtTargetClass : TargetClass{ }

    [MockTypeRelated(typeof(FsmNode))] 
    public class FsmTargetClass : TargetClass{ }

    [MockTypeRelated(typeof(UtilityNode))] 
    public class UsTargetClass : TargetClass{ }
}