using BSDesigner.Core.Perceptions;

namespace TestBSD.Core
{
    [TestFixture]
    public class TestPerceptionTasks
    {
        [Test]
        public void CustomPerception_ExecuteOnRunning_InvokeEvents()
        {
            var lastEventCalled = string.Empty;
            var perception = new CustomPerceptionTask()
            {
                OnBegin = () => lastEventCalled = "BEGIN",
                OnCheck = () => { lastEventCalled = "CHECK"; return false; },
                OnEnd = () => lastEventCalled = "END"
            };

            perception.Start();
            Assert.That(lastEventCalled, Is.EqualTo("BEGIN"));
            perception.Check();
            Assert.That(lastEventCalled, Is.EqualTo("CHECK"));
            perception.Stop();
            Assert.That(lastEventCalled, Is.EqualTo("END"));
        }

        [Test]
        public void CustomPerception_PauseAndResume_InvokeEvents()
        {
            var lastEventCalled = string.Empty;
            var perception = new CustomPerceptionTask
            {
                OnPause = () => lastEventCalled = "PAUSE",
                OnResume = () => lastEventCalled = "RESUME"
            };

            perception.Start();
            perception.Pause();
            Assert.That(lastEventCalled, Is.EqualTo("PAUSE"));
            perception.Check();
            Assert.That(lastEventCalled, Is.EqualTo("RESUME"));
        }

        [Test]
        public void CustomPerception_StopWhenIsNotActive_NotInvokeEvents()
        {
            var lastEventCalled = string.Empty;
            var perception = new CustomPerceptionTask
            {
                OnEnd = () => lastEventCalled = "END"
            };

            perception.Start();
            perception.Stop();
            Assert.That(lastEventCalled, Is.EqualTo("END"));
            lastEventCalled = string.Empty;
            perception.Stop();
            Assert.That(lastEventCalled, Is.EqualTo(string.Empty));
        }

        [Test]
        public void CustomPerception_PauseWhenIsPaused_NotInvokeEvents()
        {
            var lastEventCalled = string.Empty;
            var perception = new CustomPerceptionTask
            {
                OnPause = () => lastEventCalled = "PAUSE"
            };

            perception.Start();
            perception.Pause();
            Assert.That(lastEventCalled, Is.EqualTo("PAUSE"));
            lastEventCalled = string.Empty;
            perception.Pause();
            Assert.That(lastEventCalled, Is.EqualTo(string.Empty));
        }

        [Test]
        public void CustomPerception_StartWhenIsActive_NotInvokeEvents()
        {
            var lastEventCalled = string.Empty;
            var perception = new CustomPerceptionTask
            {
                OnPause = () => lastEventCalled = "INIT"
            };

            perception.Start();
            lastEventCalled = string.Empty;
            perception.Start();
            Assert.That(lastEventCalled, Is.EqualTo(string.Empty));
        }

        [Test]
        public void CustomPerception_CheckWhenIsNotActive_NotInvokeEvents()
        {
            var lastEventCalled = string.Empty;
            var perception = new CustomPerceptionTask
            {
                OnCheck = () => { lastEventCalled = "INIT"; return true; }
            };

            bool result = perception.Check();
            Assert.That(result, Is.EqualTo(false));
            Assert.That(lastEventCalled, Is.EqualTo(string.Empty));
        }

        [Test]
        public void CustomPerception_GetInfo_GetString()
        {
            var perception = new CustomPerceptionTask();
            Assert.That(!string.IsNullOrWhiteSpace(perception.GetInfo()), Is.True);
        }
    }
}