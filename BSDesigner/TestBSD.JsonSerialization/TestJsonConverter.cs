using BehaviourDesigner.JsonSerialization;
using BSDesigner.BehaviourTrees;
using BSDesigner.Core;
using BSDesigner.Core.Tasks;

namespace TestBSD.JsonSerialization
{
    public class TestJsonSerializer
    {

        [Test]
        public void SerializeGraph_DefaultSettings_ResultIsNotEmpty()
        {
            var bt = new BehaviourTree();
            var action = bt.CreateActionNode(new CustomActionTask());
            var dec = bt.CreateDecorator<LoopNode>(action);
            bt.ChangeRootNode(dec);
            var jsonData = JsonUtilities.Serialize(bt);
            Assert.That(jsonData, Is.Not.Empty);
        }

        [Test]
        public void SerializeGraph_DeserializeResult_EqualStructure()
        {
            var bt = new BehaviourTree();
            var action = bt.CreateActionNode(new CustomActionTask());
            var dec = bt.CreateDecorator<LoopNode>(action);
            bt.ChangeRootNode(dec);
            var jsonData = JsonUtilities.Serialize(bt);

            var graphs = JsonUtilities.Deserialize(jsonData);

            Assert.That(graphs.Count, Is.EqualTo(1));
            var graph1 = graphs[0];
            Assert.That(graph1, Is.InstanceOf<BehaviourTree>());

            var bt2 = (BehaviourTree)graph1;
            Assert.That(bt2.Nodes, Has.Count.EqualTo(2));
        }

        [Test]
        public void SerializeGraph_HierarchicalSystem_ExecuteSubsystem()
        {
            var bt = new BehaviourTree();
            var subBt = new BehaviourTree();
            bt.CreateActionNode(new SubsystemAction { SubSystem = subBt });
            subBt.CreateActionNode(new CustomActionTask());

            var jsonData = JsonUtilities.Serialize( new List<BehaviourEngine>{bt, subBt});

            var graphs = JsonUtilities.Deserialize(jsonData);
            bt = (BehaviourTree)graphs[0];
            subBt = (BehaviourTree)graphs[1];
            bt.Start();
            Assert.That(subBt.Status, Is.EqualTo(Status.Running));
            bt.Stop();
            Assert.That(subBt.Status, Is.EqualTo(Status.None));
        }
    }
}