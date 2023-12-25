using BSDesigner.Core;
using BSDesigner.Core.Actions;
using BSDesigner.Core.Perceptions;
using BSDesigner.StateMachines;
using BSDesigner.Core.Exceptions;
using TestBSD.StateMachines.Mocks;
using ExecutionContext = BSDesigner.Core.ExecutionContext;

namespace TestBSD.StateMachines
{
    public class Tests
    {
        [Test]
        public void CreateStateMachine_CreateTransitions_Correct()
        {
            var fsm = new StateMachine();
            var state1 = fsm.CreateState<ActionState>();
            var state2 = fsm.CreateState<ActionState>();
            var transition = fsm.CreateTransition(state1, state2);
            var exitTransition = fsm.CreateExitTransition(state2, Status.Success);
            Assert.That(state1.Graph, Is.EqualTo(fsm));
            Assert.That(transition.Graph, Is.EqualTo(fsm));
            Assert.That(fsm.AreNodesConnected(state1, state2));
        }

        [Test]
        public void ActionState_ExecuteEvents_PropagateEvents()
        {
            bool paused = false;
            var fsm = new StateMachine();
            ActionTask action = new CustomActionTask
            {
                OnUpdate = () => Status.Running,
                OnPause = () => paused = true,
                OnResume = () => paused = false
            };

            var state = fsm.CreateState(action);
            fsm.Start();
            Assert.That(state.Status, Is.EqualTo(Status.Running));
            fsm.Pause();
            Assert.That(paused, Is.True);
            fsm.Update();
            Assert.That(paused, Is.False);
            Assert.That(state.Status, Is.EqualTo(Status.Running));
            fsm.Stop();
            Assert.That(state.Status, Is.EqualTo(Status.None));
        }

        [Test]
        public void ActionState_ExecuteOnLoop_KeepStatusOnRunning()
        {
            var fsm = new StateMachine();
            var action = new CustomActionTask
            {
                OnUpdate = () => Status.Success,
            };

            var state = fsm.CreateState(action, executeOnLoop: true);
            fsm.Start();
            Assert.That(state.Status, Is.EqualTo(Status.Running));
            fsm.Update();
            Assert.That(state.Status, Is.EqualTo(Status.Running));
        }

        [Test]
        public void AnyState_ExecuteEvents_PropagateEvents()
        {
            var fsm = new StateMachine();
            var state1 = fsm.CreateState<ActionState>();
            var anyState = fsm.CreateAnyState();
            var state2 = fsm.CreateState<ActionState>();
            fsm.CreateTransition(anyState, state2);

            fsm.Start();
            Assert.That(state1.Status, Is.EqualTo(Status.Running));
            Assert.That(state2.Status, Is.EqualTo(Status.None));
            Assert.That(anyState.Status, Is.EqualTo(Status.Running));
            fsm.Update();
            Assert.That(state1.Status, Is.EqualTo(Status.None));
            Assert.That(state2.Status, Is.EqualTo(Status.Running));
            Assert.That(anyState.Status, Is.EqualTo(Status.Running));
            fsm.Stop();
            Assert.That(state1.Status, Is.EqualTo(Status.None));
            Assert.That(state2.Status, Is.EqualTo(Status.None));
            Assert.That(anyState.Status, Is.EqualTo(Status.None));
        }


        [Test]
        public void AnyState_ConnectInputTransition_ThrowException()
        {
            var fsm = new StateMachine();
            var anyState = fsm.CreateAnyState();
            var transition = new StateTransition();
            fsm.AddNode(transition);
            Assert.That(() => fsm.ConnectNodes(transition, anyState), Throws.InstanceOf<ConnectionException>());
        }

        [Test]
        public void Transition_ExecuteEvents_PropagateEvents()
        {
            bool paused = false;
            var lastEvent = string.Empty;
            var fsm = new StateMachine();
            ActionTask action = new CustomActionTask
            {
                OnUpdate = () => Status.Running,
            };
            PerceptionTask perception = new CustomPerceptionTask()
            {
                OnBegin = () => lastEvent = "BEGIN",
                OnCheck = () => false,
                OnPause = () => paused = true,
                OnResume = () => paused = false,
                OnEnd = () => lastEvent = "END"
            };

            var state = fsm.CreateState(action);
            var state2 = fsm.CreateState(action);
            fsm.CreateTransition(state, state2, perception);
            fsm.Start();
            Assert.That(lastEvent, Is.EqualTo("BEGIN"));
            fsm.Pause();
            Assert.That(paused, Is.True);
            fsm.Update();
            Assert.That(paused, Is.False);
            fsm.Stop();
            Assert.That(lastEvent, Is.EqualTo("END"));
        }

        [Test]
        public void StateTransitions_TriggerTransition_ChangeCurrentState()
        {
            var fsm = new StateMachine();
            var state1 = fsm.CreateState(new CustomActionTask());
            var state2 = fsm.CreateState(new CustomActionTask());
            fsm.CreateTransition(state1, state2);

            fsm.Start();
            Assert.That(fsm.CurrentState, Is.EqualTo(state1));
            Assert.That(state1.Status, Is.EqualTo(Status.Running));
            Assert.That(state2.Status, Is.EqualTo(Status.None));
            fsm.Update();
            Assert.That(fsm.CurrentState, Is.EqualTo(state2));
            Assert.That(state1.Status, Is.EqualTo(Status.None));
            Assert.That(state2.Status, Is.EqualTo(Status.Running));
        }

        [Test]
        [TestCase(Status.Success)]
        [TestCase(Status.Failure)]
        public void ExitTransition_FinalStatus_ExitMachineWithStatusGiven(Status finalStatus)
        {
            var fsm = new StateMachine();
            var state = fsm.CreateState(new CustomActionTask{OnUpdate = () => Status.Success});
            fsm.CreateExitTransition(state, finalStatus, flags: StatusFlags.Success);

            fsm.Start();
            fsm.Update();
            Assert.That(fsm.Status, Is.EqualTo(finalStatus));
        }

        [Test]
        public void ExitTransition_ConnectTarget_ThrowException()
        {
            var fsm = new StateMachine();
            var state = fsm.CreateState<ActionState>();
            var et = fsm.CreateExitTransition(state, Status.Success, flags: StatusFlags.Success);
            Assert.That(() => fsm.ConnectNodes(et, state), Throws.InstanceOf<ConnectionException>());
        }

        [Test]
        public void Transition_PerformWhenIsNotValid_ThrowException()
        {
            var fsm = new StateMachine();
            var state1 = fsm.CreateState<ActionState>();
            var state2 = fsm.CreateState<ActionState>();
            var et = fsm.CreateTransition(state2, state1);
            Assert.That(() => et.Perform(), Throws.InstanceOf<InvalidTransitionException>());
        }

        [Test]
        [TestCase(0.5f, 0.5f, 0.3f, 0)]
        [TestCase(0.5f, 0.5f, 0.5f, 1)]
        [TestCase(0.5f, 0.5f, 0.6f, 1)]
        public void ProbabilityTransition_NoProbabilitiesAssigned_SelectState(float prob1, float prob2, float randomValue, int expectedSelectedState)
        {
            var fsm = new StateMachine();
            var state1 = fsm.CreateState<ActionState>();
            var state2 = fsm.CreateState<ActionState>();
            var state3 = fsm.CreateState<ActionState>();
            var t = fsm.CreateProbabilityTransition(state1, perception: null, StatusFlags.Active, state2, state3);
            var context = new ExecutionContext
            {
                RandomProvider = new MockedRandom()
                {
                    DoubleValue = randomValue
                }
            };
            fsm.SetContext(context);

            t.Probabilities[state2] = prob1;
            t.Probabilities[state3] = prob2;
            fsm.Start();
            fsm.Update();
            Assert.That(state2.Status, Is.EqualTo(expectedSelectedState == 0 ? Status.Running : Status.None));
            Assert.That(state3.Status, Is.EqualTo(expectedSelectedState == 1 ? Status.Running : Status.None));
        }

        [Test]
        [TestCase(0)]
        [TestCase(1)]
        public void ProbabilityTransition_ProbabilitiesAssigned_SelectState(int expectedSelectedState)
        {
            var fsm = new StateMachine();
            var state1 = fsm.CreateState<ActionState>();
            var state2 = fsm.CreateState<ActionState>();
            var state3 = fsm.CreateState<ActionState>();
            var t = fsm.CreateProbabilityTransition(state1, perception: null, StatusFlags.Active, state2, state3);
            var context = new ExecutionContext
            {
                RandomProvider = new MockedRandom()
                {
                    IntValue = expectedSelectedState
                }
            };
            fsm.SetContext(context);
            fsm.Start();
            fsm.Update();
            Assert.That(state2.Status, Is.EqualTo(expectedSelectedState == 0 ? Status.Running : Status.None));
            Assert.That(state3.Status, Is.EqualTo(expectedSelectedState == 1 ? Status.Running : Status.None));
        }

        [Test]
        public void ProbabilityTransition_NoTargetStates_ThrowException()
        {
            var fsm = new StateMachine();
            var state1 = fsm.CreateState<ActionState>();
            var state2 = fsm.CreateState<ActionState>();
            var state3 = fsm.CreateState<ActionState>();
            var et = fsm.CreateProbabilityTransition(state1);
            fsm.Start();
            Assert.That(() => fsm.Update(), Throws.InstanceOf<MissingConnectionException>());
        }
    }
}