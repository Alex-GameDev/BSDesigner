using BSDesigner.Core;
using BSDesigner.Core.Exceptions;
using TestBSD.Core.Mocks;

namespace TestBSD.Core
{
    [TestFixture]
    public class TestGraphExecution
    {
        private MockGraph _behaviourEngine = null!;
        private string _eventName = string.Empty;

        [SetUp]
        public void SetUp()
        {
            _behaviourEngine = new MockGraph();
            _behaviourEngine.OnEvent += (str) => _eventName = str;
        }
        
        [Test]
        public void StartBehaviour_WhenIsStopped_ChangeStatus()
        {
            _behaviourEngine.SetStatus(Status.None);
            _behaviourEngine.Start();
            Assert.That(_behaviourEngine.Status, Is.EqualTo(Status.Running));
            Assert.That(_eventName, Is.EqualTo(MockGraph.START_EVENT));
        }

        [Test]
        public void StartBehaviour_WhenIsAlreadyRunning_ThrowException()
        {
            _behaviourEngine.SetStatus(Status.Running);
            Assert.That(() => _behaviourEngine.Start(), Throws.TypeOf<ExecutionStatusException>());
        }

        [Test]
        public void StartBehaviour_WhenIsFinished_ThrowException()
        {
            _behaviourEngine.SetStatus(Status.Success);
            Assert.That(() => _behaviourEngine.Start(), Throws.TypeOf<ExecutionStatusException>());

            _behaviourEngine.SetStatus(Status.Failure);
            Assert.That(() => _behaviourEngine.Start(), Throws.TypeOf<ExecutionStatusException>());
        }

        [Test]
        public void StopBehaviour_WhenIsRunning_ChangeStatus()
        {
            _behaviourEngine.SetStatus(Status.Running);
            _behaviourEngine.Stop();
            Assert.That(_behaviourEngine.Status, Is.EqualTo(Status.None));
            Assert.That(_eventName, Is.EqualTo(MockGraph.STOP_EVENT));
        }

        [Test]
        public void StopBehaviour_WhenIsFinished_ChangeStatusButNotCallEvent()
        {
            _eventName = string.Empty;
            _behaviourEngine.SetStatus(Status.Success);
            _behaviourEngine.Stop();
            Assert.That(_behaviourEngine.Status, Is.EqualTo(Status.None));
            Assert.That(_eventName, Is.EqualTo(MockGraph.STOP_EVENT));

            _eventName = string.Empty;
            _behaviourEngine.SetStatus(Status.Failure);
            _behaviourEngine.Stop();
            Assert.That(_behaviourEngine.Status, Is.EqualTo(Status.None));
            Assert.That(_eventName, Is.EqualTo(MockGraph.STOP_EVENT));
        }

        [Test]
        public void StopBehaviour_WhenIsAlreadyStopped_ThrowException()
        {
            _behaviourEngine.SetStatus(Status.None);
            Assert.That(() => _behaviourEngine.Stop(), Throws.TypeOf<ExecutionStatusException>());
        }

        [Test]
        public void UpdateBehaviour_WhenIsRunning_CallEvent()
        {
            _behaviourEngine.Start();
            _eventName = string.Empty;
            _behaviourEngine.Update();
            Assert.That(_behaviourEngine.Status, Is.EqualTo(Status.Running));
            Assert.That(_eventName, Is.EqualTo(MockGraph.UPDATE_EVENT));
        }

        [Test]
        public void UpdateBehaviour_WhenIsFinished_NotCallEvent()
        {
            _behaviourEngine.SetStatus(Status.Success);
            _eventName = string.Empty;
            _behaviourEngine.Update();
            Assert.That(_behaviourEngine.Status, Is.EqualTo(Status.Success));
            Assert.That(_eventName, Is.EqualTo(string.Empty));

            _behaviourEngine.SetStatus(Status.Failure);
            _eventName = string.Empty;
            _behaviourEngine.Update();
            Assert.That(_behaviourEngine.Status, Is.EqualTo(Status.Failure));
            Assert.That(_eventName, Is.EqualTo(string.Empty));
        }

        [Test]
        public void UpdateBehaviour_WhenIsStopped_ThrowException()
        {
            _behaviourEngine.SetStatus(Status.None);
            Assert.That(() => _behaviourEngine.Update(), Throws.TypeOf<ExecutionStatusException>());
        }

        [Test]
        public void PauseBehaviour_WhenIsRunning_Paused()
        {
            _eventName = string.Empty;
            _behaviourEngine.SetStatus(Status.Running);
            _behaviourEngine.Pause();
            Assert.That(_eventName, Is.EqualTo(MockGraph.PAUSE_EVENT));
        }

        [Test]
        public void PauseBehaviour_WhenIsAlreadyPaused_Paused()
        {
            _behaviourEngine.SetPause(true);
            _eventName = string.Empty;
            _behaviourEngine.SetStatus(Status.Running);
            _behaviourEngine.Pause();
            Assert.That(_eventName, Is.EqualTo(string.Empty));
        }

        [Test]
        public void PauseBehaviour_WhenIsStopped_NotPaused()
        {
            _behaviourEngine.SetPause(false);
            _eventName = string.Empty;
            _behaviourEngine.SetStatus(Status.None);
            _behaviourEngine.Pause();
            Assert.That(_eventName, Is.EqualTo(string.Empty));
            Assert.That(_behaviourEngine.IsPaused, Is.EqualTo(false));
        }

        [Test]
        public void PauseBehaviour_WhenIsFinished_NotPaused()
        {
            _behaviourEngine.SetPause(false);
            _eventName = string.Empty;

            _behaviourEngine.SetStatus(Status.Success);
            _behaviourEngine.Pause();
            Assert.That(_eventName, Is.EqualTo(string.Empty));
            Assert.That(_behaviourEngine.IsPaused, Is.EqualTo(false));

            _behaviourEngine.SetStatus(Status.Failure);
            _behaviourEngine.Pause();
            Assert.That(_eventName, Is.EqualTo(string.Empty));
            Assert.That(_behaviourEngine.IsPaused, Is.EqualTo(false));
        }
    }
}
