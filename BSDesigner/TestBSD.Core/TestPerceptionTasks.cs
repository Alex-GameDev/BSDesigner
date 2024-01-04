using BSDesigner.Core;
using BSDesigner.Core.Perceptions;
using TestBSD.Core.Mocks;

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

        [Test]
        [TestCase(false, false, false)]
        [TestCase(false, true, false)]
        [TestCase(true, true, true)]
        public void AndPerception_HasSubPerceptions_CorrectResult(bool v1, bool v2, bool expectedResult)
        {
            var p1 = new CustomPerceptionTask { OnCheck = () => v1 };
            var p2 = new CustomPerceptionTask { OnCheck = () => v2 };

            var andPerception = new AndPerception(p1, p2);

            andPerception.Start();
            Assert.That(andPerception.Check(), Is.EqualTo(expectedResult));
        }

        [Test]
        public void AndPerception_NoSubPerceptions_ReturnsFalse()
        {
            var andPerception = new AndPerception();

            andPerception.Start();
            Assert.That(andPerception.Check(), Is.False);
        }


        [Test]
        public void AndPerception_GetInfo_ValidString()
        {
            var andPerception1 = new AndPerception();
            var andPerception2 = new AndPerception(new CustomPerceptionTask());
            var andPerception3 = new AndPerception(new CustomPerceptionTask(), new CustomPerceptionTask());
            Assert.That(andPerception1.GetInfo(), Is.EqualTo("()"));
            Assert.That(andPerception2.GetInfo(), Does.Match("(.*)"));
            Assert.That(andPerception3.GetInfo(), Does.Match("(.* && .*)"));
        }

        [Test]
        [TestCase(false, false, false)]
        [TestCase(false, true, true)]
        [TestCase(true, true, true)]
        public void OrPerception_HasSubPerceptions_CorrectResult(bool v1, bool v2, bool expectedResult)
        {
            var p1 = new CustomPerceptionTask { OnCheck = () => v1 };
            var p2 = new CustomPerceptionTask { OnCheck = () => v2 };

            var orPerception = new OrPerception(p1, p2);

            orPerception.Start();
            Assert.That(orPerception.Check(), Is.EqualTo(expectedResult));
        }

        [Test]
        public void OrPerception_NoSubPerceptions_ReturnsFalse()
        {
            var orPerception = new OrPerception();

            orPerception.Start();
            Assert.That(orPerception.Check(), Is.False);
        }
        
        [Test]
        public void OrPerception_GetInfo_ValidString()
        {
            var orPerception1 = new OrPerception();
            var orPerception2 = new OrPerception(new CustomPerceptionTask());
            var orPerception3 = new OrPerception(new CustomPerceptionTask(), new CustomPerceptionTask());
            Assert.That(orPerception1.GetInfo(), Is.EqualTo("()"));
            Assert.That(orPerception2.GetInfo(), Does.Match("(.*)"));
            Assert.That(orPerception3.GetInfo(), Does.Match("(.* || .*)"));
        }

        [Test]
        [TestCase(Status.Running, StatusFlags.Running, true)]
        [TestCase(Status.Running, StatusFlags.Active, true)]
        [TestCase(Status.Running, StatusFlags.None, false)]
        [TestCase(Status.Success, StatusFlags.NotFailure, true)]
        public void StatusPerception_HandlerSet_CorrectResult(Status handlerStatus, StatusFlags flags, bool expectedResult)
        {
            var handler = new MockGraph();
            handler.SetStatus(handlerStatus);
            var p = new StatusPerception(handler, flags);
            p.Start();
            Assert.That(p.Check(), Is.EqualTo(expectedResult));
        }

        [Test]
        public void StatusPerception_NotHandlerSet_ReturnFalse()
        {
            var p = new StatusPerception(null, StatusFlags.None);
            p.Start();
            Assert.That(p.Check(), Is.False);
        }

        [Test]
        public void StatusPerception_GetInfo_ValidString()
        {
            var p = new StatusPerception(null, StatusFlags.None);
            Assert.That(p.GetInfo(), Is.Not.Empty);
        }
    }
}