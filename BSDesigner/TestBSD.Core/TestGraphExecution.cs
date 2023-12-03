using BSDesigner.Core;
using BSDesigner.Core.Exceptions;
using TestBSD.Core.Mocks;

namespace TestBSD.Core
{
    [TestFixture]
    public class TestGraphExecution
    {
        private MockGraph _behaviourEngine = null!;
        private string eventName = string.Empty;

        [SetUp]
        public void SetUp()
        {
            _behaviourEngine = new MockGraph();
            _behaviourEngine.OnEvent += (str) => eventName = str;
        }

        [Test]
        public void ExecuteBehaviour_StartAndStop_ChangeStatus()
        {
            _behaviourEngine.SetStatus(Status.None);
            Assert.That(_behaviourEngine.Status, Is.EqualTo(Status.None));
            _behaviourEngine.Start();
            Assert.That(_behaviourEngine.Status, Is.EqualTo(Status.Running));
            Assert.That(eventName, Is.EqualTo(MockGraph.START_EVENT));
            _behaviourEngine.Stop();
            Assert.That(_behaviourEngine.Status, Is.EqualTo(Status.None));
            Assert.That(eventName, Is.EqualTo(MockGraph.STOP_EVENT));
        }

        [Test]
        public void ExecuteBehaviour_PauseAndResume_ChangeStatus()
        {
            _behaviourEngine.SetStatus(Status.None);
            Assert.That(_behaviourEngine.Status, Is.EqualTo(Status.None));
            _behaviourEngine.Start();
            _behaviourEngine.Pause();
            Assert.That(_behaviourEngine.Status, Is.EqualTo(Status.Paused));
            Assert.That(eventName, Is.EqualTo(MockGraph.PAUSE_EVENT));
            _behaviourEngine.Resume();
            Assert.That(_behaviourEngine.Status, Is.EqualTo(Status.Running));
            Assert.That(eventName, Is.EqualTo(MockGraph.RESUME_EVENT));
            _behaviourEngine.Stop();
        }

        [Test]
        public void ExecuteBehaviour_StartWithTheEngineRunning_ThrowExecutionStatusException()
        {
            _behaviourEngine.SetStatus(Status.None);
            Assert.That(_behaviourEngine.Status, Is.EqualTo(Status.None));
            _behaviourEngine.Start();
            Assert.That(() => _behaviourEngine.Start(), Throws.InstanceOf<ExecutionStatusException>());
            _behaviourEngine.Stop();
        }

        [Test]
        public void ExecuteBehaviour_StopWithTheEngineStopped_ThrowExecutionStatusException()
        {
            _behaviourEngine.SetStatus(Status.None);
            Assert.That(() => _behaviourEngine.Stop(), Throws.InstanceOf<ExecutionStatusException>());
        }

        [Test]
        public void ExecuteBehaviour_Update_StatusIsNotModified()
        {
            eventName = string.Empty;
            _behaviourEngine.SetStatus(Status.None);

            _behaviourEngine.Update();
            Assert.That(eventName, Is.EqualTo(string.Empty));
            Assert.That(_behaviourEngine.Status, Is.EqualTo(Status.None));
            _behaviourEngine.Start();
            _behaviourEngine.Update();
            Assert.That(_behaviourEngine.Status, Is.EqualTo(Status.Running));
            Assert.That(eventName, Is.EqualTo(MockGraph.UPDATE_EVENT));
        }

        [Test]
        public void ExecuteBehaviour_PauseWithTheEngineStopped_StatusIsNotModified()
        {
            eventName = string.Empty;
            _behaviourEngine.SetStatus(Status.None);
            _behaviourEngine.Pause();
            Assert.That(eventName, Is.EqualTo(string.Empty));
        }

        [Test]
        public void ExecuteBehaviour_ResumeWithTheEngineRunning_StatusIsNotModified()
        {
            _behaviourEngine.SetStatus(Status.None);
            _behaviourEngine.Start();
            eventName = string.Empty;
            _behaviourEngine.Resume();
            Assert.That(eventName, Is.EqualTo(string.Empty));
        }
    }
}
