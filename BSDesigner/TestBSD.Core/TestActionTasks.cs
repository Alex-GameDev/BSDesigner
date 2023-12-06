using BSDesigner.Core;
using BSDesigner.Core.Tasks;

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
    }
}