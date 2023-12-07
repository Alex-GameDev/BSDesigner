using BSDesigner.Core;
using BSDesigner.Core.Exceptions;
using BSDesigner.Core.Tasks;
using TestBSD.Core.Mocks;

namespace TestBSD.Core
{
    [TestFixture]
    public class TestActionTasks
    {
        [Test]
        public void CustomAction_ExecuteOnRunning_InvokeEvents()
        {
            var lastEventCalled = string.Empty;
            var action = new CustomActionTask
            {
                OnBegin = () => lastEventCalled = "BEGIN",
                OnUpdate = () => { lastEventCalled = "UPDATE"; return Status.Running;},
                OnEnd = () => lastEventCalled = "END"
            };

            action.Start();
            Assert.That(lastEventCalled, Is.EqualTo("BEGIN"));
            action.Update();
            Assert.That(lastEventCalled, Is.EqualTo("UPDATE"));
            action.Stop();
            Assert.That(lastEventCalled, Is.EqualTo("END"));
        }

        [Test]
        public void CustomAction_ExecuteOnFinish_InvokeEvents()
        {
            var lastEventCalled = string.Empty;
            var action = new CustomActionTask
            {
                OnBegin = () => lastEventCalled = "BEGIN",
                OnUpdate = () => { lastEventCalled = "UPDATE"; return Status.Success; },
                OnEnd = () => lastEventCalled = "END"
            };

            action.Start();
            Assert.That(lastEventCalled, Is.EqualTo("BEGIN"));
            action.Update();
            Assert.That(lastEventCalled, Is.EqualTo("END"));
        }

        [Test]
        public void CustomAction_PauseAndResume_InvokeEvents()
        {
            var lastEventCalled = string.Empty;
            var action = new CustomActionTask
            {
                OnPause = () => lastEventCalled = "PAUSE",
                OnResume = () => lastEventCalled = "RESUME"
            };

            action.Start();
            action.Pause();
            Assert.That(lastEventCalled, Is.EqualTo("PAUSE"));
            action.Update();
            Assert.That(lastEventCalled, Is.EqualTo("RESUME"));
        }

        [Test]
        public void CustomAction_StopWhenIsNotActive_NotInvokeEvents()
        {
            var lastEventCalled = string.Empty;
            var action = new CustomActionTask
            {
                OnEnd = () => lastEventCalled = "END"
            };

            action.Start();
            action.Stop();
            Assert.That(lastEventCalled, Is.EqualTo("END"));
            lastEventCalled = string.Empty;
            action.Stop();
            Assert.That(lastEventCalled, Is.EqualTo(string.Empty));
        }

        [Test]
        public void CustomAction_PauseWhenIsPaused_NotInvokeEvents()
        {
            var lastEventCalled = string.Empty;
            var action = new CustomActionTask
            {
                OnPause = () => lastEventCalled = "PAUSE"
            };

            action.Start();
            action.Pause();
            Assert.That(lastEventCalled, Is.EqualTo("PAUSE"));
            lastEventCalled = string.Empty;
            action.Pause();
            Assert.That(lastEventCalled, Is.EqualTo(string.Empty));
        }

        [Test]
        public void CustomAction_StartWhenIsActive_NotInvokeEvents()
        {
            var lastEventCalled = string.Empty;
            var action = new CustomActionTask
            {
                OnPause = () => lastEventCalled = "INIT"
            };

            action.Start();
            lastEventCalled = string.Empty;
            action.Start();
            Assert.That(lastEventCalled, Is.EqualTo(string.Empty));
        }

        [Test]
        public void CustomAction_UpdateWhenIsNotActive_NotInvokeEvents()
        {
            var lastEventCalled = string.Empty;
            var action = new CustomActionTask
            {
                OnUpdate = () => {lastEventCalled = "INIT"; return Status.Success; }
            };

            Status result = action.Update();
            Assert.That(result, Is.EqualTo(Status.None));
            Assert.That(lastEventCalled, Is.EqualTo(string.Empty));
        }

        [Test]
        public void CustomAction_GetInfo_GetString()
        {
            var action = new CustomActionTask();
            Assert.That(!string.IsNullOrWhiteSpace(action.GetInfo()), Is.True);
        }

        [Test]
        public void SimpleAction_OnStart_InvokeEvent()
        {
            var lastEventCalled = string.Empty;
            var action = new SimpleActionTask
            {
                Action = () => lastEventCalled = "INIT"
            };
            action.Start();
            Assert.That(lastEventCalled, Is.EqualTo("INIT"));
            Assert.That(action.Update(), Is.EqualTo(Status.Success));

            Assert.That(() => action.Pause(), Throws.Nothing);
            Assert.That(() => action.Update(), Throws.Nothing);
            Assert.That(() => action.Stop(), Throws.Nothing);
        }

        [Test]
        public void SubsystemAction_WithSubsystem_PropagateEvents()
        {
            var graph = new MockGraph();
            var action = new SubsystemAction()
            {
                SubSystem = graph
            };

            Assert.That(action.GetInfo(), Is.Not.Null);
            action.Start();
            Assert.That(graph.Status, Is.EqualTo(Status.Running));
            action.Pause();
            Assert.That(graph.IsPaused, Is.EqualTo(true));
            action.Update();
            Assert.That(graph.IsPaused, Is.EqualTo(false));
            action.Stop();
            Assert.That(graph.Status, Is.EqualTo(Status.None));
        }

        [Test]
        public void SubsystemAction_WithNoSubsystem_ThrowExceptions()
        {
            var graph = new MockGraph();
            var action = new SubsystemAction();

            Assert.That(() => action.Start(), Throws.TypeOf<MissingBehaviourSystemException>());
            Assert.That(() => action.Update(), Throws.TypeOf<MissingBehaviourSystemException>());
            Assert.That(() => action.Pause(), Throws.TypeOf<MissingBehaviourSystemException>());
            Assert.That(() => action.Stop(), Throws.TypeOf<MissingBehaviourSystemException>());
        }
    }
}