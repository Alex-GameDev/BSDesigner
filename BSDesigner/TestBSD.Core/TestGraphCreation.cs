using BSDesigner.Core;
using BSDesigner.Core.Exceptions;
using TestBSD.Core.Mocks;

namespace TestBSD.Core
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void CreateMockGraph_DefaultConstructor_TypeMatches()
        {
            var graph = new MockGraph();
            var n1 = new MockNode();

            Assert.That(graph.NodeType, Is.EqualTo(typeof(MockNode)));
            Assert.That(n1.MaxInputConnections, Is.EqualTo(-1));
            Assert.That(n1.MaxOutputConnections, Is.EqualTo(-1));
            Assert.That(n1.GraphType, Is.EqualTo(typeof(MockGraph)));
            Assert.That(n1.ChildType, Is.EqualTo(typeof(MockNode)));

        }

        [Test]
        public void CreateMockGraph_ParameterConstructor_TypeMatches()
        {
            var graph = new MockGraph(typeof(Node));
            var n1 = new MockNode(1, 2, typeof(BehaviourGraph), typeof(Node));

            Assert.That(graph.NodeType, Is.EqualTo(typeof(Node)));
            Assert.That(n1.MaxInputConnections, Is.EqualTo(1));
            Assert.That(n1.MaxOutputConnections, Is.EqualTo(2));
            Assert.That(n1.GraphType, Is.EqualTo(typeof(BehaviourGraph)));
            Assert.That(n1.ChildType, Is.EqualTo(typeof(Node)));
        }
        
        [Test]
        public void AddNode_FromCreateNode_AssignedToGraph()
        {
            var graph = new MockGraph();
            var n1 = graph.CreateNode(-1, -1);
            Assert.That(graph.Nodes, Has.Count.EqualTo(1));
            Assert.That(graph.Nodes, Does.Contain(n1));
            Assert.That(n1.Graph, Is.EqualTo(graph));
        }

        [Test]
        public void AddNode_FromConstructor_AssignedToGraph()
        {
            var graph = new MockGraph();
            var n1 = new MockNode();
            graph.AddNode(n1);
            Assert.That(graph.Nodes, Has.Count.EqualTo(1));
            Assert.That(graph.Nodes, Does.Contain(n1));
            Assert.That(n1.Graph, Is.EqualTo(graph));
        }

        [Test]
        public void AddNode_AddFromOtherGraph_ThrowException()
        {
            var graph1 = new MockGraph(typeof(MockNode));
            var graph2 = new MockGraph(typeof(MockNode));
            var n1 = graph1.CreateNode(-1, -1);
            Assert.That(() => graph2.AddNode(n1), Throws.InstanceOf<NodeException>());
        }

        [Test]
        public void AddNode_AddTwiceToTheSameGraph_ThrowException()
        {
            var graph1 = new MockGraph(typeof(MockNode));
            var n1 = graph1.CreateNode(-1, -1);
            Assert.That(() => graph1.AddNode(n1), Throws.InstanceOf<NodeException>());
        }

        [Test]
        public void AddNode_UnsupportedNodeType_ThrowException()
        {
            var graph = new MockGraph(typeof(int));
            var node = new MockNode();
            Assert.That(() => graph.AddNode(node), Throws.InstanceOf<NodeException>());
        }

        [Test]
        public void AddNode_UnsupportedGraphType_ThrowException()
        {
            var graph = new MockGraph();
            var node = new MockNode(-1, -1, typeof(int), typeof(MockNode));
            Assert.That(() => graph.AddNode(node), Throws.InstanceOf<NodeException>());
        }

        [Test]
        public void ConnectNodes_UseConnectNodes_AddedToParentAndChildLists()
        {
            var graph = new MockGraph();
            var n1 = graph.CreateNode(-1, -1);
            var n2 = graph.CreateNode(-1, -1);
            graph.ConnectNodes(n1, n2);
            Assert.That(n1.Children, Has.Count.EqualTo(1));
            Assert.That(n1.Parents, Has.Count.EqualTo(0));
            Assert.That(n1.Children[0], Is.EqualTo(n2));
            Assert.That(n2.Parents, Has.Count.EqualTo(1));
            Assert.That(n2.Children, Has.Count.EqualTo(0));
            Assert.That(n2.Parents[0], Is.EqualTo(n1));
        }

        [Test]
        public void CreateConnections_MaxInputConnectionIsOne_ThrowException()
        {
            var graph = new MockGraph();
            var n1 = graph.CreateNode(-1, -1);
            var n2 = graph.CreateNode(+1, -1);
            var n3 = graph.CreateNode(-1, -1);
            graph.ConnectNodes(n1, n2);
            Assert.That(() => graph.ConnectNodes(n3, n2), Throws.InstanceOf<ConnectionException>());
        }

        [Test]
        public void CreateConnections_MaxOutputConnectionIsOne_ThrowException()
        {
            var graph = new MockGraph();
            var n1 = graph.CreateNode(-1, +1);
            var n2 = graph.CreateNode(-1, -1);
            var n3 = graph.CreateNode(-1, -1);
            graph.ConnectNodes(n1, n2);
            Assert.That(() => graph.ConnectNodes(n1, n3), Throws.InstanceOf<ConnectionException>());
        }

        [Test]
        public void CreateConnection_SourceIsNull_ThrowException()
        {
            var graph = new MockGraph();
            var n1 = graph.CreateNode(-1, +1);
            Assert.That(() => graph.ConnectNodes(null!, n1), Throws.InstanceOf<ConnectionException>());
        }

        [Test]
        public void CreateConnection_TargetIsNull_ThrowException()
        {
            var graph = new MockGraph();
            var n1 = graph.CreateNode(-1, +1);
            Assert.That(() => graph.ConnectNodes(n1, null!), Throws.InstanceOf<ConnectionException>());
        }

        [Test]
        public void CreateConnection_SourceIsFromOtherGraph_ThrowException()
        {
            var graph1 = new MockGraph();
            var n1 = graph1.CreateNode(-1, +1);

            var graph2 = new MockGraph();
            var n2 = graph2.CreateNode(-1, +1);
            Assert.That(() => graph1.ConnectNodes(n1, n2), Throws.InstanceOf<ConnectionException>());
        }

        [Test]
        public void CreateConnection_TargetIsFromOtherGraph_ThrowException()
        {
            var graph1 = new MockGraph();
            var n1 = graph1.CreateNode(-1, +1);

            var graph2 = new MockGraph(typeof(MockNode));
            var n2 = graph2.CreateNode(-1, +1);
            Assert.That(() => graph1.ConnectNodes(n2, n1), Throws.InstanceOf<ConnectionException>());
        }

        [Test]
        public void CreateConnection_NotCompatibleNodes_ThrowException()
        {
            var graph = new MockGraph();
            var n1 = graph.CreateNode(-1, -1);
            n1.SupportedChildType = typeof(int);
            var n2 = graph.CreateNode(-1, -1);
            Assert.That(() => graph.ConnectNodes(n1, n2), Throws.InstanceOf<ConnectionException>());
        }

        [Test]
        public void CheckConnections_IsParentOrChild_ReturnCorrectValue()
        {
            var graph = new MockGraph();
            var n1 = graph.CreateNode(-1, -1);
            var n2 = graph.CreateNode(-1, -1);
            graph.ConnectNodes(n1, n2);
            Assert.That(n1.IsParentOf(n2) , Is.True);
            Assert.That(n1.IsParentOf(n1), Is.False);
            Assert.That(n2.IsParentOf(n1), Is.False);
            Assert.That(n1.IsChildOf(n2), Is.False);
            Assert.That(n1.IsChildOf(n1), Is.False);
            Assert.That(n2.IsChildOf(n1), Is.True);
        }

        [Test]
        public void CheckConnections_AreConnected_ReturnCorrectValue()
        {
            var graph = new MockGraph();
            var n1 = graph.CreateNode(-1, -1);
            var n2 = graph.CreateNode(-1, -1);
            var n3 = graph.CreateNode(-1, -1);
            graph.ConnectNodes(n1, n2);
            graph.ConnectNodes(n1, n3);
            graph.ConnectNodes(n2, n3);
            Assert.That(graph.AreNodesConnected(n1, n2), Is.True);
            Assert.That(graph.AreNodesConnected(n2, n1), Is.False);
            Assert.That(graph.AreNodesConnected(n1, n3), Is.True);
            Assert.That(graph.AreNodesConnected(n3, n1), Is.False);
        }

        [Test]
        public void DisconnectNodes_RemoveConnection_RemovedToParentAndChildList()
        {
            var graph = new MockGraph();
            var n1 = graph.CreateNode(-1, -1);
            var n2 = graph.CreateNode(-1, -1);
            graph.ConnectNodes(n1, n2);
            graph.Disconnect(n1, n2);
            Assert.That(n1.Children, Has.Count.EqualTo(0));
            Assert.That(n1.Parents, Has.Count.EqualTo(0));
            Assert.That(n2.Parents, Has.Count.EqualTo(0));
            Assert.That(n2.Children, Has.Count.EqualTo(0));
        }

        [Test]
        public void DisconnectNodes_NotConnectedNodes_ThrowsException()
        {
            var graph = new MockGraph();
            var n1 = graph.CreateNode(-1, -1);
            var n2 = graph.CreateNode(-1, -1);
            Assert.That(() => graph.Disconnect(n1, n2), Throws.Exception);
        }

        [Test]
        public void DisconnectChildByIndex_ValidIndex_RemovedToParentAndChildList()
        {
            var graph = new MockGraph();
            var n1 = graph.CreateNode(-1, -1);
            var n2 = graph.CreateNode(-1, -1);
            var n3 = graph.CreateNode(-1, -1);
            graph.ConnectNodes(n1, n2);
            graph.ConnectNodes(n1, n3);
            graph.DisconnectChild(n1, 0);
            Assert.That(n1.Children, Has.Count.EqualTo(1));
            Assert.That(n1.Parents, Has.Count.EqualTo(0));
            Assert.That(n2.Children, Has.Count.EqualTo(0));
            Assert.That(n2.Parents, Has.Count.EqualTo(0));
            Assert.That(n3.Children, Has.Count.EqualTo(0));
            Assert.That(n3.Parents, Has.Count.EqualTo(1));
            Assert.That(n1.Children, Does.Contain(n3));
            Assert.That(n3.Parents, Does.Contain(n1));
            Assert.That(n1.Children, Does.Not.Contain(n2));
            Assert.That(n2.Parents, Does.Not.Contain(n1));
        }

        [Test]
        public void DisconnectChildByIndex_IndexOutOfBounds_ThrowsArgumentException()
        {
            var graph = new MockGraph();
            var n1 = graph.CreateNode(-1, -1);
            var n2 = graph.CreateNode(-1, -1);
            graph.ConnectNodes(n1, n2);
            Assert.That(() => graph.DisconnectChild(n1, 1), Throws.TypeOf<ConnectionException>());
        }

        [Test]
        public void DisconnectParentByIndex_ValidIndex_RemovedToParentAndChildList()
        {
            var graph = new MockGraph();
            var n1 = graph.CreateNode(-1, -1);
            var n2 = graph.CreateNode(-1, -1);
            var n3 = graph.CreateNode(-1, -1);
            graph.ConnectNodes(n1, n3);
            graph.ConnectNodes(n2, n3);
            graph.DisconnectParent(n3, 0);

            Assert.That(n1.Children, Has.Count.EqualTo(0));
            Assert.That(n1.Parents, Has.Count.EqualTo(0));
            Assert.That(n2.Children, Has.Count.EqualTo(1));
            Assert.That(n2.Parents, Has.Count.EqualTo(0));
            Assert.That(n3.Children, Has.Count.EqualTo(0));
            Assert.That(n3.Parents, Has.Count.EqualTo(1));
            Assert.That(n1.Children, Does.Not.Contain(n3));
            Assert.That(n3.Parents, Does.Not.Contain(n1));
            Assert.That(n2.Children, Does.Contain(n3));
            Assert.That(n3.Parents, Does.Contain(n2));
        }

        [Test]
        public void DisconnectParentByIndex_IndexOutOfBounds_ThrowsArgumentException()
        {
            var graph = new MockGraph();
            var n1 = graph.CreateNode(-1, -1);
            var n2 = graph.CreateNode(-1, -1);
            graph.ConnectNodes(n1, n2);
            Assert.That(() => graph.DisconnectParent(n2, 1), Throws.TypeOf<ConnectionException>());
        }

        [Test]
        public void DisconnectNodes_DisconnectAndRemoveNode_UpdateNodeLists()
        {
            var graph = new MockGraph();
            var n1 = graph.CreateNode(-1, -1);
            var n2 = graph.CreateNode(-1, -1);
            var n3 = graph.CreateNode(-1, -1);
            graph.ConnectNodes(n1, n2);
            graph.ConnectNodes(n2, n1);
            graph.ConnectNodes(n1, n3);
            graph.DisconnectAndRemove(n1);

            Assert.That(graph.Nodes, Has.Count.EqualTo(2));
            Assert.That(graph.Nodes, Does.Not.Contain(n1));

            Assert.That(n1.Parents, Has.Count.EqualTo(0));
            Assert.That(n2.Children, Has.Count.EqualTo(0));
            Assert.That(n2.Parents, Has.Count.EqualTo(0));
            Assert.That(n3.Parents, Has.Count.EqualTo(0));
        }


        [Test]
        public void RemoveNode_NotConnectedNode_UpdateNodeList()
        {
            var graph = new MockGraph();
            var n1 = graph.CreateNode(-1, -1);
            graph.RemoveNode(n1);

            Assert.That(graph.Nodes, Has.Count.EqualTo(0));
            Assert.That(n1.Graph, Is.Null);
        }

        [Test]
        public void RemoveNode_ConnectedNode_ThrowException()
        {
            var graph = new MockGraph();
            var n1 = graph.CreateNode(-1, -1);
            var n2 = graph.CreateNode(-1, -1);
            graph.ConnectNodes(n1, n2);
            Assert.That(() => graph.RemoveNode(n1), Throws.InstanceOf<NodeException>());
        }

        [Test]
        public void GetNodeIdMap_NotRepeatedNames_GetMap()
        {
            var graph = new MockGraph();
            var n1 = graph.CreateNode(-1, -1);
            n1.Name = "A";
            var n2 = graph.CreateNode(-1, -1);
            n2.Name = "B";
            var map = graph.GetNodeMap();
            Assert.That(map["A"], Is.EqualTo(n1));
            Assert.That(map["B"], Is.EqualTo(n2));
        }

        [Test]
        public void GetNodeIdMap_RepeatedNames_ThrowException()
        {
            var graph = new MockGraph();
            var n1 = graph.CreateNode(-1, -1);
            n1.Name = "A";
            var n2 = graph.CreateNode(-1, -1);
            n2.Name = "A";
            Assert.That(() => graph.GetNodeMap(ignoreRepeatedNames: false), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void GetNodeIdMap_RepeatedNamesButIgnored_NotIncludeInMap()
        {
            var graph = new MockGraph();
            var n1 = graph.CreateNode(-1, -1);
            n1.Name = "A";
            var n2 = graph.CreateNode(-1, -1);
            n2.Name = "A";
            var map = graph.GetNodeMap(ignoreRepeatedNames: true);
            Assert.That(map["A"], Is.EqualTo(n1));
        }

        [Test]
        public void GetNodeIdMap_NullOrEmptyNames_NotIncludeInMap()
        {
            var graph = new MockGraph();
            graph.CreateNode(-1, -1);
            var n2 = graph.CreateNode(-1, -1);
            n2.Name = "";
            var map = graph.GetNodeMap(ignoreRepeatedNames: true);
            Assert.That(map, Has.Count.EqualTo(0));
        }
    }
}